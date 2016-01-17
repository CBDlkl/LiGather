using System;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Domain
{
    public class CrawlerDomain
    {
        private static readonly object Obj = new object();
        private readonly LiGatherContext _db = new LiGatherContext();

        public void Add(CrawlerEntity model)
        {
            lock (Obj)
            {
                _db.CrawlerEntities.Add(model);
                _db.SaveChanges();
            }
        }

        public IQueryable<CrawlerEntity> Get(Expression<Func<CrawlerEntity, bool>> expression)
        {
            lock (Obj)
            {
                return _db.CrawlerEntities.Where(expression);
            }
        }
    }
}
