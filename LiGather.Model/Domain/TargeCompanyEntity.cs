using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.Domain
{
    public class TargeCompanyEntity
    {
        [Key]
        public int Id { set; get; }
        [Display(Name = "任务唯一编号")]
        public Guid TaskGuid { set; get; }
        [Description("企业名称")]
        public string CompanyName { set; get; }
        [Description("操作人姓名")]
        public string OperatorName { set; get; }
        [Description("录入时间")]
        public DateTime? CreateTime { set; get; }
        [Description("是否检索完毕")]
        public bool IsSearched { set; get; }
        /// <summary>
        /// 是否异常，true：异常
        /// </summary>
        [Description("查询是否异常")]
        public bool IsAbnormal { set; get; }
    }
}
