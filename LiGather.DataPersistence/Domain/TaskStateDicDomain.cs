using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LiGather.Model.WebDomain;

namespace LiGather.DataPersistence.Domain
{
    public class TaskStateDicDomain
    {
        private readonly LiGatherContext _db = new LiGatherContext();
        private readonly object _obj = new object();

        public TaskStateDic Get(Expression<Func<TaskStateDic, bool>> expression)
        {
            lock (_obj)
            {
                return _db.TaskStateDics.SingleOrDefault(expression);
            }
        }
    }
}
