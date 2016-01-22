using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FSLib.Network.Http;
using LiGather.DataPersistence.Domain;
using LiGather.Model.Domain;
using LiGather.Model.Log;
using LiGather.Model.WebDomain;
using LiGather.Util;
using Newtonsoft.Json;

namespace LiGather.Crawler.QgOrgCode
{
    public class QgCrawler
    {
        #region Url种子
        private const string TargetUrl = "https://s.nacao.org.cn/dwr/exec/ServiceForNum.getData.dwr";
        #endregion

        public TaskEntity TaskEntity = null;
        public TaskList TaskList = null;

        public QgCrawler(TaskEntity taskEntity)
        {
            TaskEntity = taskEntity;
        }

        /// <summary>
        /// 启动多线程爬行
        /// </summary>
        public void RunCrawler(TaskList taskList, int taskNum = 4)
        {
            TaskList = taskList;
            var tasks = new Task[taskNum];
            for (int i = 0; i < taskNum; i++)
            {
                tasks[i] = new Task(BaseWork);
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            TaskEntity.TaskStateDicId = 3;
            new TaskDomain().Update(TaskEntity);
        }

        /// <summary>
        /// 基单元
        /// </summary>
        private void BaseWork()
        {
            var httpclient = new HttpClient();
            while (TaskList.SurplusNum() > 0)
            {
                var company = TaskList.GetNextTask();
                QgOrgCodeEntity target = new QgOrgCodeEntity { companyName = company, TaskGuid = TaskEntity.Unique, InsertTime = DateTime.Now };
                //正文
                string data = "callCount=1\r\nc0-scriptName=ServiceForNum\r\nc0-methodName=getData\r\nc0-id=2025_1440664403388\r\nc0-e1=string:jgmc%20%3D" + company + "%20%20not%20ZYBZ%3D('2')%20\r\nc0-e2=string:jgmc%20%3D" + company + "%20%20not%20ZYBZ%3D('2')%20\r\nc0-e3=string:" + company + "\r\nc0-e4=string:2\r\nc0-e5=string:" + company + "\r\nc0-e6=string:%E5%85%A8%E5%9B%BD\r\nc0-e7=string:alll\r\nc0-e8=string:\r\nc0-e9=boolean:false\r\nc0-e10=boolean:true\r\nc0-e11=boolean:false\r\nc0-e12=boolean:false\r\nc0-e13=string:\r\nc0-e14=string:\r\nc0-e15=string:\r\nc0-param0=Object:{firststrfind:reference:c0-e1, strfind:reference:c0-e2, key:reference:c0-e3, kind:reference:c0-e4, tit1:reference:c0-e5, selecttags:reference:c0-e6, xzqhName:reference:c0-e7, button:reference:c0-e8, jgdm:reference:c0-e9, jgmc:reference:c0-e10, jgdz:reference:c0-e11, zch:reference:c0-e12, strJgmc:reference:c0-e13, :reference:c0-e14, secondSelectFlag:reference:c0-e15}\r\nxml=true";
                var context = httpclient.Create<string>(HttpMethod.Post, TargetUrl, data: data).Send();
                if (context.IsValid())
                {
                    GetEntityList(context.Result, company, TaskEntity.Unique).ForEach(t =>
                    {
                        new QgOrgCodeDomain().Add(t);
                    });
                }
                else
                {
                    new QgOrgCodeDomain().Add(target);
                }
            }
        }

        /// <summary>
        /// 解析实体
        /// </summary>
        /// <param name="content">正文</param>
        /// <param name="company">查询的企业名称</param>
        /// <param name="taskGuid"></param>
        /// <param name="isAccurateSearch">是否精确搜索</param>
        /// <returns></returns>
        private List<QgOrgCodeEntity> GetEntityList(string content, string company, Guid taskGuid, bool isAccurateSearch = true)
        {
            List<QgOrgCodeEntity> cusList = new List<QgOrgCodeEntity>();
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace("\n", " ") + ";function resultJson(){return JSON.stringify(s0[1]);}";
                object x = null;
                using (var engine = new ScriptEngine("jscript"))
                {
                    var parsed = engine.Parse(TaskList.GetKey() + content);
                    try
                    {
                        x = parsed.CallMethod("resultJson", null);
                    }
                    catch (Exception e)
                    {
                        new LogDomain().Add(new LogEntity { Details = Task.CurrentId + "线程错误:" + e.Message, ErrorDetails = e.ToString(), LogType = "error", TriggerTime = DateTime.Now });
                    }
                    if (x != null) cusList = JsonConvert.DeserializeObject<List<QgOrgCodeEntity>>(x.ToString());
                }
            }
            if (isAccurateSearch)
            {
                cusList = cusList.Where(m => m.jgmc.Equals(company)).ToList();
            }

            cusList.ForEach(m => { m.companyName = company; m.TaskGuid = taskGuid; m.InsertTime = DateTime.Now; });
            if (cusList.Count < 1)
            {
                cusList = new List<QgOrgCodeEntity> { new QgOrgCodeEntity { companyName = company, TaskGuid = taskGuid, InsertTime = DateTime.Now } };
            }
            return cusList;
        }

    }
}
