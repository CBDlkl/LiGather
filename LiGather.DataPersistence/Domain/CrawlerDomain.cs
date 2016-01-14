using System;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Domain
{
    public class CrawlerDomain
    {
        private static readonly object Obj = new object();
        private static readonly LiGatherContext Db = new LiGatherContext();

        public static void Add(CrawlerEntity model)
        {
            lock (Obj)
            {
                Db.CrawlerEntities.Add(model);
                Db.SaveChanges();
            }
        }

        public static IQueryable<CrawlerEntity> Get(Expression<Func<CrawlerEntity, bool>> expression)
        {
            lock (Obj)
            {
                return Db.CrawlerEntities.Where(expression);
            }
        }
    }
}
