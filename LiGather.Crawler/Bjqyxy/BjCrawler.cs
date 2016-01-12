using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using FSLib.Network.Http;
using Ivony.Html;
using Ivony.Html.ExpandedAPI;
using Ivony.Html.Parser;
using LiGather.DataPersistence.Domain;
using LiGather.DataPersistence.Proxy;
using LiGather.Model.Domain;
using LiGather.Util;

namespace LiGather.Crawler.Bjqyxy
{
    public class BjCrawler
    {
        #region URL种子
        /// <summary>
        /// 根URL
        /// </summary>
        const string url = "http://211.94.187.236";
        /// <summary>
        /// CookieURL
        /// </summary>
        const string firsturl = "http://211.94.187.236/wap";
        /// <summary>
        /// 查询列表
        /// </summary>
        const string targetUrl = "http://211.94.187.236/wap/creditWapAction!QueryEnt20130412.dhtml";
        /// <summary>
        /// 组织机构代码 URL种子
        /// </summary>
        const string orgCodeUrl = "http://211.94.187.236/wap/creditWapAction!view_zj_wap.dhtml";
        /// <summary>
        /// 工行注册登记号 URL种子
        /// </summary>
        const string gSzcUrl = "http://211.94.187.236/wap/creditWapAction!qr.dhtml";
        /// <summary>
        /// 税务 URL种子
        /// </summary>
        const string sWUrl = "http://211.94.187.236/wap/creditWapAction!qy.dhtml";
        /// <summary>
        /// 投资人 URL种子
        /// </summary>
        const string tzrUrl = "http://211.94.187.236/wap/creditWapAction!view_tzr_wap.dhtml";
        /// <summary>
        /// 出资历史 URL种子
        /// </summary>
        const string czlsUrl = "http://211.94.187.236/wap/creditWapAction!view_czlsxx_wap.dhtml";
        /// <summary>
        /// 主要人员 URL种子
        /// </summary>
        const string zyryUrl = "http://211.94.187.236/wap/creditWapAction!view_zyry_wap.dhtml";
        const string zhuUrl = "http://211.94.187.236/xycx/queryCreditAction!qyxq_view.dhtml";
        #endregion

        readonly HttpClient _client;

        public BjCrawler()
        {
            _client = new HttpClient();
            _client.Setting.Timeout = 1000 * 5;
            _client.Create<string>(HttpMethod.Post, firsturl).Send();
        }

        /// <summary>
        /// 爬虫逻辑
        /// </summary>
        /// <param name="companyName">企业名称</param>
        /// <returns></returns>
        public void CrawlerWork(string companyName)
        {
            while (true)
            {
                try
                {
                    var proxyEntity = ProxyDomain.GetByRandom(); //代理IP
                    if (proxyEntity == null)
                    {
                        Console.WriteLine("没有可用代理，在线代理临时获取策略启动");
                        proxyEntity = Proxy.Proxy.GetInstance().GetHttProxyEntity();
                        Console.WriteLine("线上获取到了代理：{0}:{1}", proxyEntity.IpAddress, proxyEntity.Port);
                    }
                    Thread.Sleep(250);
                    _client.Setting.Proxy = new WebProxy(proxyEntity.IpAddress, proxyEntity.Port);
                    var resultBody = _client.Create<string>(HttpMethod.Post, targetUrl, data: new
                    {
                        queryStr = companyName,
                        module = "",
                        idFlag = "qyxy"
                    }).Send();
                    var nextUrl = "";
                    if (!resultBody.IsValid())
                    {
                        RemoveIp(proxyEntity); continue;
                    }
                    if (ValidText(resultBody.Result))
                    {
                        RemoveIp(proxyEntity); continue;
                    }
                    //提取二级连接
                    var parser = new JumonyParser();
                    var document = parser.Parse(resultBody.Result).Find("li a");
                    foreach (var htmlElement in document)
                    {
                        companyName = htmlElement.InnerText();
                        nextUrl = url + htmlElement.Attribute("href").AttributeValue;
                    }
                    //提取目标正文
                    var resultsecondBody =
                        _client.Create<string>(HttpMethod.Get, zhuUrl + new Uri(firsturl + nextUrl).Query).Send();
                    if (!resultsecondBody.IsValid())
                    {
                        RemoveIp(proxyEntity); continue;
                    }
                    if (ValidText(resultBody.Result))
                    {
                        RemoveIp(proxyEntity); continue;
                    }
                    var sorceIhtml = new JumonyParser().Parse(resultsecondBody.Result.Replace("<th", "<td"));
                    var tableLists = sorceIhtml.Find("table").ToList();
                    var listall = new List<string>();
                    foreach (var htmlElement in tableLists)
                        htmlElement.Find("tr td").ForEach(t => listall.Add(t.InnerText().TrimEnd(':').TrimEnd('：').Trim()));
                    var model = FillModel(listall, companyName);

                    proxyEntity.Usage = proxyEntity.Usage + 1;
                    ProxyDomain.Update(proxyEntity);
                    CrawlerDomain.Add(model);
                    break;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 移除失效代理
        /// </summary>
        private void RemoveIp(ProxyEntity model)
        {
            model.CanUse = false;
            model.LastUseTime = DateTime.Now;
            Console.WriteLine("移除失效代理:{0}:{1}", model.IpAddress, model.Port);
            ProxyDomain.Update(model);
        }

        private bool ValidText(string html)
        {
            return html.Contains("访问");
        }

        /// <summary>
        /// 填充
        /// </summary>
        /// <param name="scoreList"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        private static CrawlerEntity FillModel(List<string> scoreList, string searchName)
        {
            var model = new CrawlerEntity();
            var info = model.GetType().GetProperties();
            foreach (var item in info)
            {
                var name = item.Name;
                var index = scoreList.FindIndex(c => c.Contains(name));
                if (index < 0)
                    continue;
                if (item.PropertyType == typeof(DateTime?))
                    item.SetValue(model, Conv.ToDateOrNull(scoreList[index + 1]), null);
                else
                    item.SetValue(model, scoreList[index + 1], null);
            }
            model.搜索名称 = searchName;
            model.更新时间 = DateTime.Now;
            model.入库时间 = DateTime.Now;
            model.是否为历史名称 = model.搜索名称.Contains(model.名称 ?? "");

            return model;
        }
    }
}
