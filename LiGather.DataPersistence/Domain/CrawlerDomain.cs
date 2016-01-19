using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Domain
{
    public class CrawlerDomain
    {
        public void Add(CrawlerEntity model)
        {
            using (LiGatherContext db = new LiGatherContext())
            {
                db.CrawlerEntities.Add(model);
                db.SaveChanges();
            }
        }

        public List<CrawlerEntity> Get(Expression<Func<CrawlerEntity, bool>> expression)
        {
            using (LiGatherContext db = new LiGatherContext())
            {
                return db.CrawlerEntities.Where(expression).ToList();
            }
        }

    }
}
