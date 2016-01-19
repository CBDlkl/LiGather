using System.Collections.Generic;
using System.Data.Entity;
using LiGather.Model.WebDomain;

namespace LiGather.DataPersistence
{
    /// <summary>
    /// 数据库初始化
    /// </summary>
    public class DbContextInitializer : DropCreateDatabaseIfModelChanges<LiGatherContext>
    {
        protected override void Seed(LiGatherContext context)
        {
            var taskStateDic = new List<TaskStateDic>
                {
                   new TaskStateDic { TaskStateName = "入库初始化", LabelClass = "label-primary"},
                   new TaskStateDic { TaskStateName = "数据抓取中", LabelClass = "label-warning"},
                   new TaskStateDic { TaskStateName = "采集完成", LabelClass = "label-success"},
                   new TaskStateDic { TaskStateName = "采集错误", LabelClass = "label-danger"}
                };
            taskStateDic.ForEach(m => context.TaskStateDics.Add(m));
            context.SaveChanges();
        }
    }
}
