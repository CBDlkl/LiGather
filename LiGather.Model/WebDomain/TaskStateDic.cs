using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.WebDomain
{
    public class TaskStateDic
    {
        [Key]
        public int Id { set; get; }
        [Display(Name = "任务状态名称")]
        public string TaskStateName { set; get; }
        [Display(Name = "label样式")]
        public string LabelClass { set; get; }
    }
}
