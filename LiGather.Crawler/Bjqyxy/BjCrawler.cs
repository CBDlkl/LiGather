using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FSLib.Network.Http;
using Ivony.Html;
using Ivony.Html.Parser;
using LiGather.DataPersistence.Domain;
using LiGather.DataPersistence.Proxy;
using LiGather.Model;
using LiGather.Model.Domain;
using LiGather.Model.Log;
using LiGather.Util;
using LiGather.Util.URL;

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

        /// <summary>
        /// 本次查询统计时间
        /// </summary>
        private static DateTime SearchTime { set; get; }
        readonly HttpClient _client;

        public BjCrawler(DateTime searchTime)
        {
            SearchTime = searchTime;
            _client = new HttpClient();
            _client.Setting.Timeout = 1000 * 5;
            _client.Create<string>(HttpMethod.Post, firsturl).Send();
        }

        /// <summary>
        /// 爬虫逻辑
        /// </summary>
        /// <returns></returns>
        public void CrawlerWork(int taskNum)
        {
            var tasks = new Task[taskNum];
            for (var i = 0; i < taskNum; i++)
            {
                var task = new Task(BaseWork);
                task.Start();
                tasks[i] = task;
            }
            Task.WaitAll(tasks);
            //Console.WriteLine("所有任务已经完成 {0}", DateTime.Now);
        }

        private void BaseWork()
        {
            var companyEntity = new TargeCompanyEntity();
            while (true)
            {
                try
                {
                    Thread.Sleep(250);
                    companyEntity = TargeCompanyDomain.GetSingel();
                    if (companyEntity == null)
                        break;
                    var companyName = companyEntity.CompanyName;
                    var proxyEntity = ProxyDomain.GetByRandom(); //代理IP
                    if (proxyEntity == null)
                    {
                        Console.WriteLine("没有可用代理，在线代理临时获取策略启动");
                        proxyEntity = Proxy.Proxy.GetInstance().GetHttProxyEntity();
                        Console.WriteLine("线上获取到了代理：{0}:{1}", proxyEntity.IpAddress, proxyEntity.Port);
                    }
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
                    NameValueCollection nameValueCollection = new NameValueCollection(URL.GetQueryString(new Uri(firsturl + nextUrl).Query));
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
                    foreach (var tableList in tableLists)
                        tableList.Find("tr td").ForEach(t => listall.Add(t.InnerText().TrimEnd(':').TrimEnd('：').Trim()));
                    var model = FillModel(listall, companyName);
                    model.全局唯一编号 = nameValueCollection["reg_bus_ent_id"]; //全局唯一编号
                    CrawlerDomain.Add(model);

                    //后续其他处理 包括了IP使用状态，以查询列表状态
                    proxyEntity.Usage = proxyEntity.Usage + 1;
                    ProxyDomain.Update(proxyEntity);

                    Console.WriteLine("{0} 抓取到：{1}", Task.CurrentId, model.搜索名称);

                }
                catch (Exception e)
                {
                    companyEntity.IsAbnormal = true;
                    TargeCompanyDomain.Update(companyEntity);
                    LogDomain.Add(new LogEntity { ErrorDetails = e.Message, TriggerTime = DateTime.Now });
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
            model.入库时间 = SearchTime;
            model.是否为历史名称 = model.搜索名称.Contains(model.名称 ?? "");

            return model;
        }
    }
}
