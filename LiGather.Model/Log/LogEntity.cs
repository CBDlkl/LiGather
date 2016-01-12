using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.Log
{
    public class LogEntity
    {
        [Key]
        public int Id { set; get; }
        [Description("错误详情")]
        public string ErrorDetails { set; get; }
        [Description("触发时间")]
        public DateTime TriggerTime { set; get; }
    }
}
