using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LiGather.Model.Domain;
using LiGather.Model.Log;
using LiGather.Model.WebDomain;

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
        public DbSet<TargeCompanyEntity> TrCompanyEntities { set; get; }
        public DbSet<LogEntity> LogEntities { set; get; }
        public DbSet<TaskEntity> TaskEntities { set; get; }
        public DbSet<TaskStateDic> TaskStateDics { set; get; }
        public DbSet<QgOrgCodeEntity> QgOrgCodeEntities { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); //移除复数表名
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();//移除联级删除
        }
    }
}
