using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using LiGather.Model.WebDomain;

namespace LiGather.DataPersistence.Domain
{
    public class TaskDomain
    {
        private readonly LiGatherContext _db = new LiGatherContext();
        private readonly object _obj = new object();

        public void Add(TaskEntity model)
        {
            lock (_obj)
            {
                _db.TaskEntities.Add(model);
                _db.SaveChanges();
            }
        }

        public IQueryable<TaskEntity> Get(Expression<Func<TaskEntity, bool>> expression = null)
        {
            lock (_obj)
            {
                if (expression != null)
                    return _db.TaskEntities.Include(t => t.TaskStateDic).Where(expression);
                else
                    return _db.TaskEntities.Include(t => t.TaskStateDic);
            }
        }

        public void Update(TaskEntity model)
        {
            lock (_obj)
            {
                _db.TaskEntities.AddOrUpdate(model);
            }
        }
    }
}
