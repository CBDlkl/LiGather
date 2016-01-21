using System;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.Domain
{
    public class TargeCompanyEntity
    {
        [Key]
        public int Id { set; get; }
        [Display(Name = "任务唯一编号")]
        public Guid TaskGuid { set; get; }
        [Display(Name = "企业名称")]
        public string CompanyName { set; get; }
        [Display(Name = "操作人姓名")]
        public string OperatorName { set; get; }
        [Display(Name = "录入时间")]
        public DateTime? CreateTime { set; get; }
        [Display(Name = "是否检索完毕")]
        public bool IsSearched { set; get; }
    }
}
