using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Domain
{
    public class QgOrgCodeDomain
    {
        private readonly object _obj = new object();

        public void Add(QgOrgCodeEntity entity)
        {
            lock (_obj)
            {
                using (LiGatherContext db = new LiGatherContext())
                {
                    if (!db.QgOrgCodeEntities.Any(t => t.companyName.Equals(entity.companyName)))
                    {
                        db.QgOrgCodeEntities.Add(entity);
                        db.SaveChanges();
                    }
                }
            }
        }

        public List<QgOrgCodeEntity> Get(Expression<Func<QgOrgCodeEntity, bool>> expression)
        {
            lock (_obj)
            {
                using (LiGatherContext db = new LiGatherContext())
                {
                    return db.QgOrgCodeEntities.Where(expression).ToList();
                }
            }
        }
    }
}
