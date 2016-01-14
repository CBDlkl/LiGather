using System;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.WebDomain
{
    public class TaskEntity
    {
        [Key]
        public int Id { set; get; }
        [Display(Name = "唯一编号")]
        public Guid Unique { set; get; }
        [Display(Name = "任务名称")]
        public string TaskName { set; get; }
        [Display(Name = "任务总数")]
        public int TaskNum { set; get; }
        [Display(Name = "创建时间")]
        public DateTime CreateTime { set; get; }
        [Display(Name = "完成时间")]
        public DateTime? EndTime { set; get; }
        [Display(Name = "操作者姓名")]
        public string OperatorName { set; get; }
        [Display(Name = "任务状态")]
        public TaskStateDic TaskStateDic { set; get; }
        public int TaskStateDicId { set; get; }
    }
}
