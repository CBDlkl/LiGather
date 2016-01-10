using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSLib.Network.Http;
using LiHttp = FSLib.Network.Http.HttpClient;

namespace LiGather.HttpClient
{
    /// <summary>
    /// 创建http请求
    /// </summary>
    public sealed class HttpClient
    {
        /// <summary>
        /// 创建Post请求
        /// </summary>
        public string ClientPost()
        {
            var client = new LiHttp();
            var context = client.Create<string>(HttpMethod.Post, "", data: new { }).Send();
            return context.IsValid() ? context.Result : "";
        }
    }
}
