
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiGather.Model;

namespace LiGather.Console
{
    class Program
    {
        private static readonly DateTime TimeStamp = DateTime.Now;
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Proxy.Proxy.GetInstance().ProxySave(100, 10);
            //Proxy.Proxy.GetInstance().ValidateCanUse(7);

            //待查数据初始化
            //var lists = File.ReadAllLines("E:/1.txt", Encoding.Default).ToList();
            //InsertMetadata(lists);

            //抓取数据
            var bjqyxy = new Crawler.Bjqyxy.BjCrawler(TimeStamp);
            bjqyxy.CrawlerWork(4);

            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            System.Console.WriteLine("总共花费{0}分钟.", ts2.TotalMinutes);
            System.Console.ReadKey();
        }

        /// <summary>
        /// 待查询数据初始化
        /// </summary>
        /// <param name="lists"></param>
        private static void InsertMetadata(List<string> lists)
        {
            var tasks = new Task[3];
            for (var i = 0; i < 3; i++)
            {
                var task = new Task(() =>
                   {
                       while (true)
                       {
                           lock (lists)
                           {
                               if (lists.Count <= 0)
                                   break;
                               var companyName = lists.Last();
                               DataPersistence.Domain.TargeCompanyDomain.Add(new TargeCompanyEntity { CompanyName = companyName, CreateTime = TimeStamp, IsSearched = false });
                               System.Console.WriteLine("成功插入：{0} 线程 {1}", Task.CurrentId, companyName); //Thread.CurrentThread.ManagedThreadId
                               lists.Remove(companyName);
                           }
                       }
                   });
                task.Start();
                tasks[i] = task;
            }
            Task.WaitAll(tasks);
            System.Console.WriteLine("数据导入完毕");
        }

    }
}
