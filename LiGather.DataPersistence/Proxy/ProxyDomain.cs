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
        private static readonly object Obj = new object();
        private readonly LiGatherContext _db = new LiGatherContext();

        public bool IsExist(ProxyEntity model)
        {
            lock (Obj)
                return _db.ProxyEntities.Any(t => t.IpAddress == model.IpAddress);
        }

        public ProxyEntity GetById(int id)
        {
            lock (Obj)
            {
                return _db.ProxyEntities.OrderBy(t => t.LastUseTime).SingleOrDefault(t => t.Id == id);
            }
        }

        public ProxyEntity GetByRandom()
        {
            lock (Obj)
            {
                //随机取
                //var idLists = _db.ProxyEntities.Where(t => t.CanUse == true).Select(t => t.Id).ToList();
                //var id = new Random().Next(0, idLists.Count);
                return _db.ProxyEntities.Where(t => t.CanUse.Value).OrderByDescending(t => t.LastUseTime).FirstOrDefault();
            }
        }

        public int GetMaxId()
        {
            lock (Obj)
            {
                return _db.ProxyEntities.Max(t => t.Id);
            }
        }

        public ProxyEntity Get(ProxyEntity model)
        {
            lock (Obj)
            {
                return _db.ProxyEntities.SingleOrDefault(t => t.Id == model.Id);
            }
        }

        public void Add(ProxyEntity model)
        {
            lock (Obj)
            {
                _db.ProxyEntities.Add(model);
                _db.SaveChanges();
            }
        }

        public void Update(ProxyEntity model)
        {
            lock (Obj)
            {
                _db.ProxyEntities.AddOrUpdate(model);
                _db.SaveChanges();
            }
        }

        public void LockUpdate(ProxyEntity model)
        {
            lock (Obj)
            {
                var proxyEntity = _db.ProxyEntities.Where(t => t.Id == model.Id && t.CanUse == true).ToList();
                if (!proxyEntity.Any()) return;
                _db.ProxyEntities.AddOrUpdate(model);
                var i = _db.SaveChanges();
                if (i == 0)
                {
                    Console.WriteLine("EF 内存泄露，启动清扫策略。");
                    //日志记录
                    new LogDomain().Add(new LogEntity { ErrorDetails = "EF 内存泄露，启动清扫策略。", TriggerTime = DateTime.Now });
                    _db.Database.ExecuteSqlCommand("update ProxyEntity set CanUse=0 where CanUse!=0");
                }
                Console.WriteLine("移除失效代理:{0}:{1}", model.IpAddress, model.Port);
            }
        }
    }
}
