using System;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.Domain
{
    public class QgOrgCodeEntity
    {
        [Key]
        public int Id { set; get; }
        [Display(Name = "查询名称")]
        public string companyName { set; get; }
        [Display(Name = "颁发单位")]
        public string bzjgmcs { set; get; }
        [Display(Name = "备注日期")]
        public string bzrq { set; get; }
        [Display(Name = "机构代码")]
        public string jgdm { set; get; }
        [Display(Name = "机构地址")]
        public string jgdz { set; get; }
        [Display(Name = "机构类型")]
        public string jglx { set; get; }
        [Display(Name = "机构名称")]
        public string jgmc { set; get; }
        [Display(Name = "机构登记证号")]
        public string zch { set; get; }
        [Display(Name = "有效期（起）")]
        public string zcrq { set; get; }
        [Display(Name = "有效期（止）")]
        public string zfrq { set; get; }
        [Display(Name = "任务编号")]
        public Guid TaskGuid { set; get; }
        //public string entryJgdm { set; get; }
        //public string ly { set; get; }
        //public string reservea { set; get; }
        //public string rowNum { set; get; }
    }
}
