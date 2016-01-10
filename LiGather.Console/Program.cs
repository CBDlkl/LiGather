using LiGather.DataPersistence.Proxy;
using LiGather.Model.Domain;

namespace LiGather.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new ProxyEntity { IpAddress = "10.165.10.20", Port = "80", Usage = 2 };
            new ProxyDomain().Add(model);
        }
    }
}
