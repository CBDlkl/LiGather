using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace LiGather.DataPersistence
{
    internal sealed class ReportingDbMigrationsConfiguration<TContext> : DbMigrationsConfiguration<TContext>
       where TContext : DbContext
    {
        public ReportingDbMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;          //获取或设置 指示迁移数据库时是否可使用自动迁移的值。
            AutomaticMigrationDataLossAllowed = true;   //获取或设置 指示是否可接受自动迁移期间的数据丢失的值。如果设置为false，则将在数据丢失可能作为自动迁移一部分出现时引发异常。
        }

        protected override void Seed(TContext context)
        {
            //
        }
    }
}
