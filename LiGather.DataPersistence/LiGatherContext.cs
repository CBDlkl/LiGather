using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LiGather.Model.Domain;

namespace LiGather.DataPersistence
{
    public class LiGatherContext : DbContext
    {
        public LiGatherContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LiGatherContext, ReportingDbMigrationsConfiguration<LiGatherContext>>());
        }

        public DbSet<ProxyEntity> ProxyEntities { set; get; }
        public DbSet<CrawlerEntity> CrawlerEntities { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); //移除复数表名
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();//移除联级删除
        }
    }
}
