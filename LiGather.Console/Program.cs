
namespace LiGather.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //Proxy.Proxy.GetInstance().ProxySave(100, 10);
            //Proxy.Proxy.GetInstance().ValidateCanUse(5);
            new Crawler.Bjqyxy.BjCrawler().CrawlerWork("百度鹏寰资产管理（北京）有限公司");
            System.Console.ReadKey();
        }
    }
}
