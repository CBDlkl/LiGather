using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Proxy
{
    /// <summary>
    /// 代理领域服务
    /// </summary>
    public class ProxyDomain
    {
        private static readonly LiGatherContext Db = new LiGatherContext();

        public static bool IsExist(ProxyEntity model)
        {
            lock (Db)
                return Db.ProxyEntities.Any(t => t.IpAddress == model.IpAddress);
        }

        public static ProxyEntity GetById(int id)
        {
            lock (Db)
            {
                return Db.ProxyEntities.OrderBy(t => t.LastUseTime).SingleOrDefault(t => t.Id == id);
            }
        }

        public static ProxyEntity GetByRandom()
        {
            lock (Db)
            {
                //随机取
                //var idLists = Db.ProxyEntities.Where(t => t.CanUse == true).Select(t => t.Id).ToList();
                //var id = new Random().Next(0, idLists.Count);
                return Db.ProxyEntities.OrderBy(t => t.LastUseTime).FirstOrDefault();
            }
        }

        public static int GetMaxId()
        {
            lock (Db)
            {
                return Db.ProxyEntities.Max(t => t.Id);
            }
        }

        public static ProxyEntity Get(ProxyEntity model)
        {
            lock (Db)
            {
                return Db.ProxyEntities.SingleOrDefault(t => t.Id == model.Id);
            }
        }

        public static void Add(ProxyEntity model)
        {
            lock (Db)
            {
                Db.ProxyEntities.Add(model);
                Db.SaveChanges();
            }
        }

        public static void Update(ProxyEntity model)
        {
            lock (Db)
            {
                Db.ProxyEntities.AddOrUpdate(model);
                Db.SaveChanges();
            }
        }
    }
}
