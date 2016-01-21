using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiGather.Crawler;
using LiGather.Crawler.QgOrgCode;
using LiGather.Model;
using LiGather.Model.WebDomain;

namespace LiGather.Console
{
    class Program
    {
        private static readonly DateTime TimeStamp = DateTime.Now;
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //BjCrawler()
            //QgCrawler();

            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            System.Console.WriteLine("总共花费{0}分钟.", ts2.TotalMinutes);
            System.Console.ReadKey();
        }

        private static void QgCrawler()
        {
            //List<string> lists = new List<string>
            //{
            //    "北京方晟建筑智能化工程有限公司",
            //    "北京泰瑞思装饰工程有限公司",
            //    "北京凯莹装饰工程有限公司",
            //    "北京盈果建筑工程有限公司",
            //    "北京博晟装饰设计工程有限公司",
            //    "北京欣兴奥建筑结构工程技术有限公司"
            //};
            //var tasklist = TaskList.GetInstance();
            //tasklist.AddTask(lists);
            //new QgCrawler().RunCrawler(tasklist, 1);
        }

        private static void BjCrawler()
        {
            TaskEntity model = new TaskEntity
            {
                OperatorName = "张雪艳",
                Unique = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                TaskName = "测试任务",
                TaskStateDicId = 1,
            };

            //Proxy.Proxy.GetInstance().ProxySave(100, 10);
            //Proxy.Proxy.GetInstance().ValidateCanUse(7);
            var OperatorName = "张雪艳"; //操作人

            //待查数据初始化
            var lists = File.ReadAllLines("E:/1.txt", Encoding.Default).ToList();
            new BaseData(model).InsertMetadata(lists, OperatorName, model, taskEntity =>
            {
                //抓取数据
                var bjqyxy = new Crawler.Bjqyxy.BjCrawler(taskEntity, t => t.TaskGuid.Equals(taskEntity.Unique));
                bjqyxy.CrawlerWork();
            });
        }
    }
}
