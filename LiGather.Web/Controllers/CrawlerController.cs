using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LiGather.Model.WebDomain;
using LiGather.Crawler;
using LiGather.DataPersistence.Domain;
using LiGather.Model.Log;
using LiGather.Util;

namespace LiGather.Web.Controllers
{
    public class CrawlerController : Controller
    {

        public ActionResult TaskList()
        {
            return View(new TaskDomain().Get().ToList());
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult CreateTask()
        {
            ViewBag.Unique = Guid.NewGuid();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskEntity model, HttpPostedFileBase txtfile)
        {
            var stream = txtfile.InputStream;
            var streamread = new StreamReader(stream, Encoding.Default);
            var companyList = new List<string>();
            while (!streamread.EndOfStream)
                companyList.Add(streamread.ReadLine());
            model.TaskStateDicId = 1;
            model.TaskNum = companyList.Count;
            model.CreateTime = DateTime.Now;
            new TaskDomain().Add(model);
            new Task(() =>
            {
                new BaseData(model).InsertMetadata(companyList.ToList(), model.TaskName, model, taskEntity =>
                {
                    var bjqyxy = new Crawler.Bjqyxy.BjCrawler(taskEntity, t => t.TaskGuid.Equals(taskEntity.Unique));
                    new Task(() => { bjqyxy.CrawlerWork(); }).Start();
                });
            }).Start();
            return Json(new { msg = $"成功上传了任务文件，系统接受到{companyList.Count}条记录，正在导入系统中。。。" });
        }

        public ActionResult CheckInsertMetadata(TaskEntity model)
        {
            var insertNum = new TargeCompanyDomain().GetInt(t => t.TaskGuid == model.Unique);
            return Json(new { state = "doing", num = insertNum });
        }

        public ActionResult GoGather(TaskEntity model)
        {
            //Thread.Sleep(1000 * 3); //默认等待三秒
            ////抓取数据
            //var bjqyxy = new Crawler.Bjqyxy.BjCrawler(model, t => t.TaskGuid.Equals(model.Unique));
            //new Task(() => { bjqyxy.CrawlerWork(); }).Start();
            return Json(new { state = "nothion" });
        }

        public ActionResult CheckGoGather(TaskEntity model)
        {
            var searchNum =
                new TargeCompanyDomain().GetInt(
                    t => t.TaskGuid == model.Unique && t.IsSearched);
            return Json(new { state = "doing", num = searchNum });
        }

        public ActionResult Export(TaskEntity model, bool isOptimize)
        {
            var crawlerlists = new CrawlerDomain().Get(t => t.TaskGuid == model.Unique).OrderByDescending(t => t.爬行更新时间).ToList();
            if (crawlerlists.Count < 1)
                return Content("<script>alert('未找到内容');</script>");
            var bytes = crawlerlists.ListToExcel(isOptimize);
            return File(bytes, "application/vnd.ms-excel",
                "导出北京企业采集信息[" + DateTime.Now.ToString("yyyy-M-d dddd") + "].xls");
        }
    }
}