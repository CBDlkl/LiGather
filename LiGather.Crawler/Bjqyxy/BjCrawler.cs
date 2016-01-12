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
using LiGather.DataPersistence.Proxy;
using LiGather.Model.Domain;

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
        /// 爬虫逻辑
        /// </summary>
        /// <param name="companyName">企业名称</param>
        /// <returns></returns>
        public string CrawlerWork(string companyName)
        {
            try
            {
                ProxyEntity proxyEntity = ProxyDomain.GetByRandom(); //代理IP
                var client = new HttpClient();
                client.Create<string>(HttpMethod.Post, firsturl).Send();
                while (true)
                {
                    Thread.Sleep(250);
                    //client.Setting.Proxy = new WebProxy(proxyEntity.IpAddress, proxyEntity.Port);
                    var resultBody = client.Create<string>(HttpMethod.Post, targetUrl, data: new
                    {
                        queryStr = companyName,
                        module = "",
                        idFlag = "qyxy"
                    }).Send();
                    var nextUrl = "";
                    if (!resultBody.IsValid())
                        continue;
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
                        client.Create<string>(HttpMethod.Get, zhuUrl + new Uri(firsturl + nextUrl).Query).Send();
                    if (!resultsecondBody.IsValid())
                    {
                        continue;
                    }
                    var sorceIhtml = new JumonyParser().Parse(resultsecondBody.Result);
                    var tableCount = sorceIhtml.Find("table");


                    //int i = 0;
                    //foreach (var htmlElement in tableCount)
                    //{
                    //    Console.WriteLine("**********第{0}号表**********", i++);
                    //    var trs = htmlElement.Find("tr");
                    //    foreach (var element in trs)
                    //    {
                    //        var tds = element.Find("td");
                    //        foreach (var td in tds)
                    //        {
                    //            Console.WriteLine(td.InnerText());
                    //        }
                    //    }
                    //}
                    return "";
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        ///// <summary>
        ///// 解析HTML文本信息
        ///// </summary>
        ///// <param name="SourceHtml"></param>
        ///// <returns></returns>
        //private CrawlerEntity HtmlAnalytical(string SourceHtml)
        //{
        //    CrawlerEntity model = new CrawlerEntity(); //模型容器
        //    //model.HtmlScore = SourceHtml; //存储源码
        //    var sorceIhtml = new JumonyParser().Parse(SourceHtml);
        //    var tdHtmlBases = sorceIhtml.Find(".f-lan tr");
        //    var list = new List<string>();
        //    var elements = tdHtmlBases as IHtmlElement[] ?? tdHtmlBases.ToArray();
        //    for (int i = 0; i < elements.Count(); i++)
        //    {
        //        Console.WriteLine(elements[i].Find("td").FirstOrDefault().InnerText());
        //        var text = elements[i].Find("td").FirstOrDefault().InnerText();
        //        switch (text.Trim())
        //        {
        //            case "工商登记注册基本信息":
        //                Modular01(sorceIhtml, model, i);
        //                break;
        //            case "资本相关信息":
        //                Modular02(sorceIhtml, model, i);
        //                break;
        //            case "组织机构代码信息":
        //                Modular03(sorceIhtml, model, i);
        //                break;
        //            case "税务登记信息":
        //                Modular04(sorceIhtml, model, i);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return model;
        //}

        //#region HTML解析帮助
        //public static CrawlerEntity Modular01(IHtmlDocument sorceIhtml, CrawlerEntity model, int i)
        //{
        //    //基础信息
        //    var tdHtml_base = sorceIhtml.Find(".f-lbiao").ElementAt(i).Find("tr td").ToList();
        //    //foreach (var td in tdHtml_base)
        //    //{
        //    //    Console.WriteLine(td.InnerText());
        //    //}
        //    model.basicsInfo = FillModel<BasicsInfo>(tdHtml_base);
        //    return model;
        //}

        //public static TargetModel Modular02(IHtmlDocument sorceIhtml, TargetModel model, int i)
        //{
        //    //资本相关信息
        //    var tdHtml_zb = sorceIhtml.Find(".f-lbiao").ElementAt(i).Find("tr td").ToList();
        //    //foreach (var td in tdHtml_zb)
        //    //{
        //    //    Console.WriteLine(td.InnerText());
        //    //}
        //    model.capitalInfo = FillModel<CapitalInfo>(tdHtml_zb);
        //    return model;
        //}

        //public static TargetModel Modular03(IHtmlDocument sorceIhtml, TargetModel model, int i)
        //{
        //    //组织机构代码信息
        //    var tdHtml_orgCode = sorceIhtml.Find(".f-lbiao").ElementAt(i).Find("tr td").ToList();
        //    //foreach (var td in tdHtml_orgCode)
        //    //{
        //    //    Console.WriteLine(td.InnerText());
        //    //}
        //    model.orgCodeInfo = FillModel<OrgCodeInfo>(tdHtml_orgCode);
        //    return model;
        //}

        //public static TargetModel Modular04(IHtmlDocument sorceIhtml, TargetModel model, int x)
        //{
        //    //税务登记信息
        //    var trHtml_tax = sorceIhtml.Find(".f-lbiao").ElementAt(x).Find("tr").ToList();
        //    foreach (var tr in trHtml_tax)
        //    {
        //        var sorceth = tr.Find("th");
        //        var sorcetd = tr.Find("td");
        //        //for (var i = 0; i < sorceth.Count(); i++)
        //        //{
        //        //    Console.WriteLine(sorceth.ElementAt(i).InnerText());
        //        //    Console.WriteLine(sorcetd.ElementAt(i).InnerText());
        //        //}
        //    }
        //    model.taxInfo = FillModel<TaxInfo>(trHtml_tax.Find("th").ToList(), trHtml_tax.Find("td").ToList());
        //    return model;
        //}

        ///// <summary>
        ///// List 映射到 模型，针对td
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="scoreList"></param>
        ///// <returns></returns>
        //private static T FillModel<T>(List<IHtmlElement> scoreList) where T : new()
        //{
        //    T t = new T();
        //    PropertyInfo[] info = t.GetType().GetProperties();
        //    foreach (var item in info)
        //    {
        //        string name = item.Name.Split('_')[1];
        //        for (int i = 0; i < scoreList.Count; i++)
        //        {
        //            if (i % 2 == 0)
        //            {
        //                var trName = scoreList[i].InnerText().Replace("：", "");
        //                var isContent = trName.Equals(name);
        //                if (isContent) item.SetValue(t, scoreList[i + 1].InnerText(), null);
        //            }
        //        }
        //    }
        //    return t;
        //}

        ///// <summary>
        ///// List 映射到 模型，针对th和td
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="scoreTh"></param>
        ///// <param name="scoreTd"></param>
        ///// <returns></returns>
        //private static T FillModel<T>(List<IHtmlElement> scoreTh, List<IHtmlElement> scoreTd) where T : new()
        //{
        //    T t = new T();
        //    PropertyInfo[] info = t.GetType().GetProperties();
        //    foreach (var item in info)
        //    {
        //        string name = item.Name.Split('_')[1];
        //        for (int i = 0; i < scoreTh.Count; i++)
        //        {
        //            var trName = scoreTh[i].InnerText().Replace("：", "");
        //            var isContent = trName.Equals(name);
        //            if (isContent)
        //            {
        //                item.SetValue(t, scoreTd[i].InnerText(), null); break;
        //            }
        //        }
        //    }
        //    return t;
        //}

        //#endregion
    }
}
