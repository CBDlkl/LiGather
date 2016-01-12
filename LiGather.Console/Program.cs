
using System.IO;
using System.Linq;
using System.Text;

namespace LiGather.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //Proxy.Proxy.GetInstance().ProxySave(100, 10);
            //Proxy.Proxy.GetInstance().ValidateCanUse(7);
            var lists = File.ReadAllLines("E:/1.txt", Encoding.Default).ToList();
            var bjqyxy = new Crawler.Bjqyxy.BjCrawler();
            foreach (var list in lists)
            {
                if (string.IsNullOrWhiteSpace(list.Trim()))
                    continue;
                bjqyxy.CrawlerWork(list);
            }
            System.Console.ReadKey();
        }
    }
}
