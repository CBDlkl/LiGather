using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
        /// <summary>
        /// 操作人
        /// </summary>
        private static string Operator { set; get; }
        /// <summary>
        /// 企业名单检索条件
        /// </summary>
        private Expression<Func<TargeCompanyEntity, bool>> QueryCondition { set; get; }
        /// <summary>
        /// HTTP访问对象
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// 北京企业信用信息网 爬虫
        /// 维护时间：2016年1月14日 09:57:04
        /// </summary>
        /// <param name="operatorName">操作人</param>
        /// <param name="queryCondition">企业名单检索条件</param>
        public BjCrawler(string operatorName, Expression<Func<TargeCompanyEntity, bool>> queryCondition)
        {
            Operator = operatorName;
            QueryCondition = queryCondition;
            SearchTime = DateTime.Now;
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
                tasks[i] = task;
                task.Start();
            }
            Task.WaitAll(tasks);
            Console.WriteLine("所有任务已经完成 {0}", DateTime.Now);
        }

        private void BaseWork()
        {
            bool isReloadCompany = true; //是否重新获取新的企业名称
            string companyOld = "";
            var companyEntity = new TargeCompanyEntity();
            while (true)
            {
                var targetModel = new CrawlerEntity { 操作人姓名 = Operator, 入爬行库时间 = SearchTime };
                try
                {
                    Thread.Sleep(250);
                    //查询资源预处理
                    if (isReloadCompany)
                    {
                        companyEntity = TargeCompanyDomain.GetSingel(QueryCondition);
                        if (companyEntity == null)
                            break;
                        targetModel.搜索名称 = companyEntity.CompanyName; //搜索名称，直接持久化
                        companyOld = targetModel.搜索名称;
                        isReloadCompany = false;
                    }
                    else
                    {
                        targetModel.搜索名称 = companyOld;
                    }

                    //IP处理
                    var proxyEntity = ProxyDomain.GetByRandom(); //代理IP
                    if (proxyEntity == null)
                    {
                        Console.WriteLine("在线代理临时获取策略启动。");
                        proxyEntity = Proxy.Proxy.GetInstance().GetHttProxyEntity();
                        Console.WriteLine("线上获取到了代理：{0}:{1}", proxyEntity.IpAddress, proxyEntity.Port);
                    }

                    _client.Setting.Proxy = new WebProxy(proxyEntity.IpAddress, proxyEntity.Port);
                    var resultBody = _client.Create<string>(HttpMethod.Post, targetUrl, data: new
                    {
                        queryStr = targetModel.搜索名称,
                        module = "",
                        idFlag = "qyxy"
                    }).Send();
                    if (!resultBody.IsValid())
                    {
                        RemoveOldIp(proxyEntity);
                        continue;
                    }
                    if (ValidText(resultBody.Result))
                    {
                        RemoveOldIp(proxyEntity);
                        continue;
                    }
                    //提取二级连接
                    var parser = new JumonyParser();
                    var urls = parser.Parse(resultBody.Result).Find("li a").ToList();
                    var nextUrl = "";
                    if (urls.Count < 1)
                    {
                        isReloadCompany = true;
                        AddNull(targetModel);
                        continue;
                    }
                    foreach (var htmlElement in urls)
                    {
                        targetModel.名称 = htmlElement.InnerText();
                        nextUrl = url + htmlElement.Attribute("href").AttributeValue;
                    }
                    //提取目标正文
                    var resultsecondBody =
                        _client.Create<string>(HttpMethod.Get, zhuUrl + new Uri(firsturl + nextUrl).Query).Send();
                    var nameValueCollection =
                        new NameValueCollection(URL.GetQueryString(new Uri(firsturl + nextUrl).Query));
                    if (!resultsecondBody.IsValid())
                    {
                        RemoveOldIp(proxyEntity);
                        continue;
                    }
                    if (ValidText(resultsecondBody.Result))
                    {
                        RemoveOldIp(proxyEntity);
                        continue;
                    }

                    //正文处理
                    var sorceIhtml = new JumonyParser().Parse(resultsecondBody.Result.Replace("<th", "<td"));
                    var tableLists = sorceIhtml.Find("table[class='f-lbiao']").ToList();
                    var listall = new List<string>();
                    foreach (var tableList in tableLists)
                        tableList.Find("tr td")
                            .ForEach(t => listall.Add(t.InnerText().TrimEnd(':').TrimEnd('：').Trim()));
                    var fillModel = FillModel(listall);
                    fillModel.全局唯一编号 = nameValueCollection["reg_bus_ent_id"];
                    CrawlerDomain.Add(StrategyNo1(fillModel, targetModel));
                    //后续其他处理 包括了IP使用状态，以查询列表状态
                    proxyEntity.Usage = proxyEntity.Usage + 1;
                    ProxyDomain.Update(proxyEntity);
                    Console.WriteLine("{0} 抓取到：{1}", Task.CurrentId, targetModel.搜索名称);
                }
                catch (Exception e)
                {
                    companyEntity.IsAbnormal = true;
                    TargeCompanyDomain.Update(companyEntity);
                    LogDomain.Add(new LogEntity { ErrorDetails = e.Message, TriggerTime = DateTime.Now });
                    AddNull(targetModel);
                }
                isReloadCompany = true;
            }
        }

        /// <summary>
        /// 因为异常或者没有检索到值 执行空插入
        /// </summary>
        private void AddNull(CrawlerEntity targetModel)
        {
            CrawlerDomain.Add(targetModel);
            Console.WriteLine("{0} 空对象：{1}", Task.CurrentId, targetModel.搜索名称);
        }

        /// <summary>
        /// 移除失效代理
        /// </summary>
        private void RemoveOldIp(ProxyEntity model)
        {
            model.CanUse = false;
            model.LastUseTime = DateTime.Now;
            ProxyDomain.LockUpdate(model);
        }

        private bool ValidText(string html)
        {
            return html.Contains("访问");
        }

        /// <summary>
        /// 生成抓取的充血模型
        /// </summary>
        /// <param name="scoreList"></param>
        /// <returns></returns>
        private CrawlerEntity FillModel(List<string> scoreList)
        {
            var model = new CrawlerEntity();
            var info = model.GetType().GetProperties();
            foreach (var item in info)
            {
                var name = item.Name;
                var index = scoreList.FindIndex(c => c.Equals(name));
                if (index < 0)
                    continue;
                if (item.PropertyType == typeof(DateTime?))
                    item.SetValue(model, Conv.ToDateOrNull(scoreList[index + 1]), null);
                else
                    item.SetValue(model, scoreList[index + 1], null);
            }
            return model;
        }

        /// <summary>
        /// 生成策略
        /// </summary>
        /// <param name="fillModel">数据容器</param>
        /// <param name="appendEntity">补充用数据容器</param>
        /// <returns></returns>
        private CrawlerEntity StrategyNo1(CrawlerEntity fillModel, CrawlerEntity appendEntity)
        {
            fillModel.搜索名称 = appendEntity.搜索名称;
            fillModel.操作人姓名 = appendEntity.操作人姓名;
            fillModel.名称 = string.IsNullOrWhiteSpace(fillModel.名称) ? appendEntity.名称 : fillModel.名称;
            fillModel.入爬行库时间 = appendEntity.入爬行库时间;
            fillModel.爬行更新时间 = DateTime.Now;
            return fillModel;
        }
    }
}
