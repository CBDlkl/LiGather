using System.Data.Entity.Migrations;
using LiGather.Model.Log;

namespace LiGather.DataPersistence.Domain
{
    public class LogDomain
    {
        public void Add(LogEntity model)
        {
            using (LiGatherContext db = new LiGatherContext())
            {
                db.LogEntities.AddOrUpdate(model);
                db.SaveChanges();
            }
        }
    }
}
