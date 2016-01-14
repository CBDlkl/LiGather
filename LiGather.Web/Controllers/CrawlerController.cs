using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiGather.Model.WebDomain;
using LiGather.Crawler;
using LiGather.DataPersistence.Domain;

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
            new BaseData(DateTime.Now).InsertMetadata(companyList.Take(2000).ToList(), model.TaskName);
            return Json(new { msg = $"成功上传了任务文件，系统接受到{companyList.Count}条任务."});
        }
    }
}