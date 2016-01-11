using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FSLib.Network.Http;
using LiGather.DataPersistence.Proxy;
using LiGather.Model.Domain;
using LiGather.Util;
using static System.Int32;

namespace LiGather.Proxy
{
    /// <summary>
    /// 代理操作
    /// </summary>
    public sealed class Proxy
    {
        private static Proxy _proxy = null;
        private static readonly object SyncRoot = new object();
        private List<string> _proxyPool = new List<string>(); //代理IP连接池
        private Proxy() { }

        /// <summary>
        /// 创建唯一实例
        /// </summary>
        /// <returns></returns>
        public static Proxy GetInstance()
        {
            //双重锁定
            if (_proxy == null)
            {
                lock (SyncRoot)
                {
                    if (_proxy == null)
                    {
                        _proxy = new Proxy();
                    }
                }
            }
            return _proxy;
        }

        /// <summary>
        /// 剩余代理数
        /// </summary>
        /// <returns></returns>
        public string GetSurplus()
        {
            return "";
        }

        /// <summary>
        /// 通过IP商获取代理IP
        /// </summary>
        /// <param name="nums">每次提取的IP数目</param>
        /// <returns></returns>
        private string GetProxyByHttp(int nums = 1)
        {
            var client = new HttpClient();
            while (true)
            {
                var content = client.Create<string>(HttpMethod.Get, "http://vxer.daili666.com/ip/", data: new
                {
                    tid = 557541152620047,
                    num = nums,
                    delay = 5,
                    category = 2,
                    sortby = "time",
                    foreign = "none",
                    filter = "on"
                }).Send();
                if (content.IsValid())
                {
                    return content.Result;
                }
                Thread.Sleep(1000 * 1); //一秒提取一次
            }
        }

        /// <summary>
        /// 更新代理到数据库
        /// </summary>
        /// <param name="countNum">采集IP总数</param>
        /// <param name="getNum">每次提取数量</param>
        /// <param name="isValidate">是否对代理验证</param>
        public void ProxySave(int countNum = 1000, int getNum = 5, bool isValidate = false)
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (countNum == 0)
                        break;
                    var ipLists = GetProxyByHttp(getNum).Split(Environment.NewLine.ToCharArray());
                    foreach (var ipList in ipLists)
                    {
                        if (string.IsNullOrWhiteSpace(ipList))
                            continue;
                        var ipAndPort = ipList.Split(':');
                        var model = new ProxyEntity();
                        model.IpAddress = ipAndPort[0];
                        model.Port = Conv.ToInt(ipAndPort[1]);
                        model.Usage = 0;
                        model.CreateTime = DateTime.Now;
                        if (isValidate)
                        {
                            if (ThreadValidate.VerificationIp(model.IpAddress, model.Port))
                            {
                                if (!ProxyDomain.IsExist(model))
                                    ProxyDomain.Add(model);
                                countNum--;
                            }
                        }
                        if (!ProxyDomain.IsExist(model))
                            ProxyDomain.Add(model);
                        countNum--;
                    }
                }
                Console.WriteLine("IP采集完毕");
            })
            { IsBackground = false }.Start();
        }

        /// <summary>
        /// 验证Ip可用性
        /// </summary>
        /// <param name="maxThreadNum">启用多少线程验证</param>
        public void ValidateCanUse(int maxThreadNum = 1)
        {
            
            var tasks = new Task[maxThreadNum];
            for (var i = 0; i < maxThreadNum; i++)
            {
                tasks[i] = new Task(() =>
                {
                    for (var j = 0; j < 20; j++)
                    {
                        var id = new Random().Next(1, ProxyDomain.GetMaxId());
                        var proxyEntity = ProxyDomain.GetById(id);
                        if (proxyEntity.Id == 0)
                            continue;
                        if (ThreadValidate.Doit(proxyEntity))
                            break;
                    }
                    Console.WriteLine("完成");
                });
                tasks[i].Start();
            }
            Console.WriteLine("主线程等待");
            Task.WaitAll(tasks);
            Console.WriteLine("执行完成");
            Console.ReadKey();
        }

    }
}
