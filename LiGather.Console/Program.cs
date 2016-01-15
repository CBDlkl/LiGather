using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiGather.Crawler;
using LiGather.Model;
using LiGather.Model.WebDomain;

namespace LiGather.Console
{
    class Program
    {
        private static readonly DateTime TimeStamp = DateTime.Now;
        static void Main(string[] args)
        {
            TaskEntity model = new TaskEntity
            {
                OperatorName = "张雪艳",
                Unique = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                TaskName = "测试任务",
                TaskStateDicId = 1,
            };

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Proxy.Proxy.GetInstance().ProxySave(100, 10);
            //Proxy.Proxy.GetInstance().ValidateCanUse(7);
            var OperatorName = "张雪艳"; //操作人

            //待查数据初始化
            var lists = File.ReadAllLines("E:/1.txt", Encoding.Default).ToList();
            new BaseData(TimeStamp).InsertMetadata(lists, OperatorName, model);

            //抓取数据
            var bjqyxy = new Crawler.Bjqyxy.BjCrawler(model, t => t.TaskGuid.Equals(model.Unique));
            bjqyxy.CrawlerWork(4, model);

            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            System.Console.WriteLine("总共花费{0}分钟.", ts2.TotalMinutes);
            System.Console.ReadKey();
        }
    }
}
