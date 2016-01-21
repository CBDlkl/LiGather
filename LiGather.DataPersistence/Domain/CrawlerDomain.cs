using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Domain
{
    public class CrawlerDomain
    {
        /// <summary>
        /// 写入新内容，若是存在就不写入
        /// </summary>
        /// <param name="model"></param>
        public void Add(CrawlerEntity model)
        {
            using (LiGatherContext db = new LiGatherContext())
            {
                if (!db.CrawlerEntities.Any(t => t.搜索名称.Equals(model.搜索名称)))
                {
                    db.CrawlerEntities.Add(model);
                    db.SaveChanges();
                }
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
