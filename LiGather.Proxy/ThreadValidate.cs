using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using FSLib.Network.Http;
using LiGather.DataPersistence.Proxy;
using LiGather.Model.Domain;

namespace LiGather.Proxy
{
    /// <summary>
    /// 验证IP可用性
    /// </summary>
    internal class ThreadValidate
    {
        /// <summary>
        /// 验证IP可用性
        /// </summary>
        public ThreadValidate()
        {
        }

        /// <summary>
        /// 验证代理IP
        /// </summary>
        /// <returns></returns>
        public static bool VerificationIp(string ip, int port)
        {
            var client = new HttpClient();
            client.Setting.Proxy = new WebProxy(ip, port);
            var content = client.Create<string>(HttpMethod.Get, "https://www.baidu.com").Send();
            if (content.IsValid())
            {
                //Console.WriteLine(content.Result);
            }
            return content.IsValid();
        }

        public static bool Doit(object o)
        {
            var model = (ProxyEntity)o;
            Console.WriteLine("线程{0}开始验证{1}:{2}", Thread.CurrentThread.ManagedThreadId, model.IpAddress, model.Port);
            if (!VerificationIp(model.IpAddress, model.Port)) return false;
            model.CanUse = true;
            model.LastUseTime = DateTime.Now;
            new ProxyDomain().Update(model);
            Console.WriteLine("当前线程{0}更新了第{1}号，{2}可用情况",
                Thread.CurrentThread.ManagedThreadId
                , model.Id, model.IpAddress);
            return true;
        }

    }
}
