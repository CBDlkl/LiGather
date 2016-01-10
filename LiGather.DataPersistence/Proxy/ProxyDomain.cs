using LiGather.Model.Domain;

namespace LiGather.DataPersistence.Proxy
{
    /// <summary>
    /// 代理领域服务
    /// </summary>
    public class ProxyDomain
    {
        public void Add(ProxyEntity model)
        {
            using (var db = new LiGatherContext())
            {
                db.ProxyEntities.Add(model);
                db.SaveChanges();
            }
        }
    }
}
