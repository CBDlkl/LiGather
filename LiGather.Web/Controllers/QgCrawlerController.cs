using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LiGather.Crawler.QgOrgCode;
using LiGather.DataPersistence.Domain;
using LiGather.Model.Domain;
using LiGather.Model.WebDomain;
using LiGather.Util;
using LiGather.Web.Models;

namespace LiGather.Web.Controllers
{
    [BrowserVersion]
    public class QgCrawlerController : Controller
    {
        public ActionResult TaskList(string searchInfo = null)
        {
            CheckTime();
            return View(string.IsNullOrWhiteSpace(searchInfo) 
                ? new TaskDomain().Get(t => t.IsSingelSearch == false && t.TaskType == EnumTaskType.QgCrawler).ToList() 
                : new TaskDomain().Get(t => t.IsSingelSearch == false && t.TaskType == EnumTaskType.QgCrawler).Where(t => t.TaskName.Contains(searchInfo)).ToList());
        }

        public ActionResult SingelSearch(string guid = null, string searchInfo = null)
        {
            CheckTime();
            QgOrgCodeEntity qgCrawlerEntity = null;
            if (string.IsNullOrWhiteSpace(guid) && string.IsNullOrWhiteSpace(searchInfo))
            {
                ViewBag.Guid = Guid.NewGuid();
            }
            else
            {
                ViewBag.Guid = guid;

                List<string> companyList = new List<string> { searchInfo };
                TaskEntity taskEntity = new TaskEntity();
                taskEntity.TaskType = EnumTaskType.QgCrawler;
                taskEntity.TaskName = $"单个任务[{DateTime.Now.ToString("G")}]";
                taskEntity.Unique = Conv.ToGuid(guid);
                taskEntity.TaskStateDicId = 2;
                taskEntity.TaskNum = 1;
                taskEntity.CreateTime = DateTime.Now;
                taskEntity.IsSingelSearch = true;
                new TaskDomain().Add(taskEntity);

                var tasklist = Crawler.QgOrgCode.TaskList.GetInstance();
                tasklist.AddTask(companyList);
                new QgCrawler(taskEntity).RunCrawler(tasklist, 1);

                qgCrawlerEntity = new QgOrgCodeDomain().Get(t => t.companyName == searchInfo && t.jgmc != null).FirstOrDefault();
            }
            return View(qgCrawlerEntity);
        }

        public ActionResult CreateTask()
        {
            ViewBag.Unique = Guid.NewGuid();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskEntity taskEntity, HttpPostedFileBase txtfile)
        {
            var stream = txtfile.InputStream;
            var streamread = new StreamReader(stream, Encoding.Default);
            var companyList = new List<string>();
            while (!streamread.EndOfStream)
                companyList.Add(streamread.ReadLine());
            taskEntity.TaskType = EnumTaskType.QgCrawler;
            taskEntity.TaskStateDicId = 2;
            taskEntity.TaskNum = companyList.Count;
            taskEntity.CreateTime = DateTime.Now;
            taskEntity.IsSingelSearch = false;
            var tasklist = Crawler.QgOrgCode.TaskList.GetInstance();
            new TaskDomain().Add(taskEntity);
            new Task(() =>
            {
                tasklist.AddTask(companyList);
                new QgCrawler(taskEntity).RunCrawler(tasklist);
            }).Start();
            return Json(new { msg = $"成功上传了任务文件，系统接受到{companyList.Count}条记录，即将执行查询." });
        }

        public ActionResult CheckGoGather(TaskEntity model)
        {
            var searchNum = new QgOrgCodeDomain().Get(t => t.TaskGuid == model.Unique).Count;
            return Json(new { state = "doing", num = searchNum });
        }

        public ActionResult Export(TaskEntity model, bool isOptimize)
        {
            var crawlerlists = new QgOrgCodeDomain().Get(t => t.TaskGuid == model.Unique).ToList();
            if (crawlerlists.Count < 1)
                return Content("<script>alert('未找到内容');</script>");
            var bytes = crawlerlists.ListToExcel(isOptimize);
            return File(bytes, "application/vnd.ms-excel",
                "导出全国组织机构采集信息[" + DateTime.Now.ToString("yyyy-M-d dddd") + "].xls");
        }

        private void CheckTime()
        {
            var nowHour = DateTime.Now.Hour;
            if (nowHour > 17 || nowHour < 9)
            {
                ViewBag.TimeOut = "<script>var index=layer.open({title:'系统消息',icon:4,shadeClose:true,content:'全国组织机构代码网开放时间是9:00AM只5:00PM，请在指定时间内使用本功能。',btn:['老朽懂了']});</script>";
            }
        }
    }
}