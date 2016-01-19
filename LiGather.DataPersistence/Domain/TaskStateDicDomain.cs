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
        public TaskStateDic Get(Expression<Func<TaskStateDic, bool>> expression)
        {
            using (LiGatherContext _db = new LiGatherContext())
            {
                return _db.TaskStateDics.SingleOrDefault(expression);
            }
        }
    }
}
