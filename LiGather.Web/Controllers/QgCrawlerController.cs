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

namespace LiGather.Web.Controllers
{
    public class QgCrawlerController : Controller
    {
        public ActionResult TaskList()
        {
            var nowHour = DateTime.Now.Hour;
            if (nowHour >= 17 || nowHour <= 9)
            {

            }
            return View(new TaskDomain().Get(t => t.IsSingelSearch == false && t.TaskType == EnumTaskType.QgCrawler).ToList());
        }

        public ActionResult SingelSearch(string guid = null, string companyName = null)
        {
            QgOrgCodeEntity qgCrawlerEntity = null;
            if (string.IsNullOrWhiteSpace(guid) && string.IsNullOrWhiteSpace(companyName))
            {
                ViewBag.Guid = Guid.NewGuid();
            }
            else
            {
                ViewBag.Guid = guid;

                List<string> companyList = new List<string> { companyName };
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

                qgCrawlerEntity = new QgOrgCodeDomain().Get(t => t.companyName == companyName).FirstOrDefault();
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
            return Json(new { msg = $"成功上传了任务文件，系统接受到{companyList.Count}条记录，即将执行查询..." });
        }

        public ActionResult CheckGoGather(TaskEntity model)
        {
            var searchNum = new QgOrgCodeDomain().Get(t => t.TaskGuid == model.Unique).Count;
            return Json(new { state = "doing", num = searchNum });
        }

        public ActionResult Export(TaskEntity model, bool isOptimize)
        {
            var crawlerlists = new CrawlerDomain().Get(t => t.TaskGuid == model.Unique).OrderByDescending(t => t.爬行更新时间).ToList();
            if (crawlerlists.Count < 1)
                return Content("<script>alert('未找到内容');</script>");
            var bytes = crawlerlists.ListToExcel(isOptimize);
            return File(bytes, "application/vnd.ms-excel",
                "导出全国组织机构采集信息[" + DateTime.Now.ToString("yyyy-M-d dddd") + "].xls");
        }
    }
}