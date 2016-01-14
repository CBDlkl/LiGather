using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Domain
{
    public class CrawlerDomain
    {
        private static readonly object Obj = new object();
        private static readonly LiGatherContext Db = new LiGatherContext();

        public static void Add(CrawlerEntity model)
        {
            lock (Obj)
            {
                Db.CrawlerEntities.Add(model);
                Db.SaveChanges();
            }
        }
    }
}
