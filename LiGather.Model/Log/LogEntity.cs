using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.Log
{
    public class LogEntity
    {
        [Key]
        public int Id { set; get; }
        [Display(Name = "任务名称")]
        public string TaskName { set; get; }
        [Description("错误详情")]
        public string ErrorDetails { set; get; }
        [Display(Name = "错误详情")]
        public string Details { set; get; }
        [Description("触发时间")]
        public DateTime TriggerTime { set; get; }
    }
}
