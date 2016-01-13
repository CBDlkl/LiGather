using System;
using System.Data.Entity.Migrations;
using System.Linq;
using LiGather.DataPersistence.Domain;
using LiGather.Model.Domain;
using LiGather.Model.Log;

namespace LiGather.DataPersistence.Proxy
{
    /// <summary>
    /// 代理领域服务
    /// </summary>
    public class ProxyDomain
    {
        private static readonly object obj = new object();
        private static readonly LiGatherContext Db = new LiGatherContext();

        public static bool IsExist(ProxyEntity model)
        {
            lock (obj)
                return Db.ProxyEntities.Any(t => t.IpAddress == model.IpAddress);
        }

        public static ProxyEntity GetById(int id)
        {
            lock (obj)
            {
                return Db.ProxyEntities.OrderBy(t => t.LastUseTime).SingleOrDefault(t => t.Id == id);
            }
        }

        public static ProxyEntity GetByRandom()
        {
            lock (obj)
            {
                //随机取
                //var idLists = Db.ProxyEntities.Where(t => t.CanUse == true).Select(t => t.Id).ToList();
                //var id = new Random().Next(0, idLists.Count);
                return Db.ProxyEntities.Where(t => t.CanUse.Value).OrderByDescending(t => t.LastUseTime).FirstOrDefault();
            }
        }

        public static int GetMaxId()
        {
            lock (obj)
            {
                return Db.ProxyEntities.Max(t => t.Id);
            }
        }

        public static ProxyEntity Get(ProxyEntity model)
        {
            lock (obj)
            {
                return Db.ProxyEntities.SingleOrDefault(t => t.Id == model.Id);
            }
        }

        public static void Add(ProxyEntity model)
        {
            lock (obj)
            {
                Db.ProxyEntities.Add(model);
                Db.SaveChanges();
            }
        }

        public static void Update(ProxyEntity model)
        {
            lock (obj)
            {
                Db.ProxyEntities.AddOrUpdate(model);
                Db.SaveChanges();
            }
        }

        public static void LockUpdate(ProxyEntity model)
        {
            lock (obj)
            {
                var proxyEntity = Db.ProxyEntities.Where(t => t.Id == model.Id && t.CanUse == true).ToList();
                if (!proxyEntity.Any()) return;
                Db.ProxyEntities.AddOrUpdate(model);
                var i = Db.SaveChanges();
                if (i == 0)
                {
                    Console.WriteLine("EF 内存泄露，启动清扫策略。");
                    LogDomain.Add(new LogEntity { ErrorDetails = "EF 内存泄露，启动清扫策略。", TriggerTime = DateTime.Now }); //日志记录
                    Db.Database.ExecuteSqlCommand("update ProxyEntity set CanUse=0 where CanUse!=0");
                }
                Console.WriteLine("移除失效代理:{0}:{1}", model.IpAddress, model.Port);
            }
        }
    }
}
