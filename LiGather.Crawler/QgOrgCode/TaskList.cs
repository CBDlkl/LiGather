using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiGather.Crawler.QgOrgCode
{
    /// <summary>
    /// 任务目录
    /// </summary>
    public class TaskList
    {
        private static TaskList _taskList = null;
        private static readonly object SyncRoot = new object();
        private List<string> _companyList = new List<string>();
        private TaskList() { }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static TaskList GetInstance()
        {
            if (_taskList == null)
            {
                lock (SyncRoot)
                {
                    if (_taskList == null)
                    {
                        _taskList = new TaskList();
                    }
                }
            }
            return _taskList;
        }

        /// <summary>
        /// 加载待查列表
        /// </summary>
        /// <param name="companyList"></param>
        public void AddTask(List<string> companyList)
        {
            _companyList = companyList;
        }

        /// <summary>
        /// 依次提取一个任务目标
        /// </summary>
        /// <returns></returns>
        public string GetNextTask()
        {
            lock (SyncRoot)
            {
                if (_companyList.Count < 1)
                    return null;
                var targetCompany = _companyList.Last();
                _companyList.Remove(targetCompany);
                return targetCompany;
            }
        }

        /// <summary>
        /// 获取剩余任务数
        /// </summary>
        /// <returns></returns>
        public int SurplusNum()
        {
            lock (SyncRoot)
            {
                return _companyList.Count;
            }
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "OtherPages/CrawlerKey"))
                return File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "OtherPages/CrawlerKey");
            else
                throw new Exception("全国解密密钥文件不存在");
        }
    }
}
