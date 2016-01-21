using System.Data.Entity.Migrations;
using LiGather.Model.Log;

namespace LiGather.DataPersistence.Domain
{
    public class LogDomain
    {
        private readonly object _obj = new object();
        public void Add(LogEntity model)
        {
            lock (_obj)
            {
                using (LiGatherContext db = new LiGatherContext())
                {
                    db.LogEntities.AddOrUpdate(model);
                    db.SaveChanges();
                }
            }
        }
    }
}
