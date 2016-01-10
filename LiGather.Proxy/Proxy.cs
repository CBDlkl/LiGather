using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FSLib.Network.Http;

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
        /// 从代理池提取代理
        /// </summary>
        /// <returns></returns>
        public string GetProxy()
        {
            return "";
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
        /// 验证代理IP
        /// </summary>
        /// <returns></returns>
        private bool VerificationIp()
        {
            var client = new HttpClient();
            var content = client.Create<string>(HttpMethod.Get, "https://www.baidu.com").Send();
            return content.IsValid();
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
                Thread.Sleep(250);
            }
        }


    }
}
