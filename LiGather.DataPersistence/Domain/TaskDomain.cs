using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model.WebDomain;

namespace LiGather.DataPersistence.Domain
{
    public class TaskDomain
    {
        public void Add(TaskEntity model)
        {
            using (LiGatherContext _db = new LiGatherContext())
            {
                _db.TaskEntities.Add(model);
                _db.SaveChanges();
            }
        }

        public List<TaskEntity> Get(Expression<Func<TaskEntity, bool>> expression = null)
        {
            using (LiGatherContext _db = new LiGatherContext())
            {
                if (expression != null)
                    return _db.TaskEntities.Include(t => t.TaskStateDic).Where(expression).ToList();
                else
                    return _db.TaskEntities.Include(t => t.TaskStateDic).ToList();
            }
        }

        public void Update(TaskEntity model)
        {
            using (LiGatherContext _db = new LiGatherContext())
            {
                _db.TaskEntities.AddOrUpdate(model);
                _db.SaveChanges();
            }
        }
    }
}
