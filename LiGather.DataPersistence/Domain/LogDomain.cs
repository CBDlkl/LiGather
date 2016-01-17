using System.Data.Entity.Migrations;
using LiGather.Model.Log;

namespace LiGather.DataPersistence.Domain
{
    public class LogDomain
    {
        private static readonly object Obj = new object();
        private readonly LiGatherContext _db = new LiGatherContext();

        public void Add(LogEntity model)
        {
            lock (Obj)
            {
                _db.LogEntities.AddOrUpdate(model);
                _db.SaveChanges();
            }
        }
    }
}
