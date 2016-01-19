using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiGather.DataPersistence.Domain;
using LiGather.Model;
using LiGather.Model.Domain;
using LiGather.Model.WebDomain;

namespace LiGather.Crawler
{
    public class BaseData
    {
        private DateTime TimeStamp { get; }
        /// <summary>
        /// 基础数据操作
        /// </summary>
        /// <param name="timeStamp"></param>
        public BaseData(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// 待查询数据初始化，初始化完成后将更新任务状态
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="operatorName">操作员</param>
        /// <param name="model">任务模型</param>
        public void InsertMetadata(List<string> lists, string operatorName, TaskEntity model)
        {
            var tasks = new Task[3];
            for (var i = 0; i < 3; i++)
            {
                var task = new Task(() =>
                {
                    while (true)
                    {
                        lock (lists)
                        {
                            if (lists.Count <= 0)
                                break;
                            var companyName = lists.Last();
                            new TargeCompanyDomain().Add(new TargeCompanyEntity { TaskGuid = model.Unique, CompanyName = companyName, CreateTime = TimeStamp, IsSearched = false, OperatorName = operatorName });
                            Console.WriteLine("成功插入：{0} 线程 {1}", Task.CurrentId, companyName);
                            lists.Remove(companyName);
                        }
                    }
                });
                task.Start();
                tasks[i] = task;
            }
            Task.WaitAll(tasks);
            //更新任务状态
            model.TaskStateDicId = 2;
            new TaskDomain().Update(model);
            Console.WriteLine("数据导入完毕");
        }
    }
}
