using System;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.Domain
{
    /// <summary>
    /// 北京企业信用信息网
    /// </summary>
    public class CrawlerEntity
    {
        [Key]
        public int Id { set; get; }
        [Display(Name = "搜索名称")]
        public string 搜索名称 { get; set; }

        #region 正文
        [Display(Name = "名称")]
        public string 名称 { set; get; }
        [Display(Name = "全局唯一编号")]
        public string 全局唯一编号 { set; get; }
        [Display(Name = "统一社会信用代码")]
        public string 统一社会信用代码 { set; get; }
        [Display(Name = "注册号")]
        public string 注册号 { get; set; }
        [Display(Name = "类型")]
        public string 类型 { get; set; }
        [Display(Name = "负责人")]
        public string 负责人 { get; set; }
        [Display(Name = "法定代表人")]
        public string 法定代表人 { set; get; }
        [Display(Name = "营业场所")]
        public string 营业场所 { get; set; }
        [Display(Name = "住所")]
        public string 住所 { set; get; }
        [Display(Name = "营业期限自")]
        public string 营业期限自 { get; set; }
        [Display(Name = "营业期限至")]
        public string 营业期限至 { get; set; }
        [Display(Name = "经营范围")]
        public string 经营范围 { get; set; }
        [Display(Name = "登记机关")]
        public string 登记机关 { get; set; }
        [Display(Name = "核准日期")]
        public string 核准日期 { get; set; }
        [Display(Name = "成立日期")]
        public string 成立日期 { get; set; }
        [Display(Name = "登记状态")]
        public string 登记状态 { get; set; }

        [Display(Name = "纳税人名称")]
        public string 纳税人名称 { get; set; }
        [Display(Name = "税务登记类型")]
        public string 税务登记类型 { get; set; }
        [Display(Name = "税务登记证号")]
        public string 税务登记证号 { get; set; }
        [Display(Name = "法人姓名")]
        public string 法人姓名 { get; set; }
        [Display(Name = "组织机构代码")]
        public string 组织机构代码 { set; get; }
        [Display(Name = "登记受理类型")]
        public string 登记受理类型 { set;get;}
        [Display(Name = "经营地址")]
        public string 经营地址 { get; set; }
        [Display(Name = "经营地址联系电话")]
        public string 经营地址联系电话 { get; set; }
        [Display(Name = "经营地址邮编")]
        public string 经营地址邮编 { get; set; }
        [Display(Name = "企业主页网址")]
        public string 企业主页网址 { get; set; }
        [Display(Name = "所处街乡")]
        public string 所处街乡 { get; set; }
        [Display(Name = "国地税共管户标识")]
        public string 国地税共管户标识 { get; set; }
        [Display(Name = "登记注册类型")]
        public string 登记注册类型 { get; set; }
        [Display(Name = "隶属关系")]
        public string 隶属关系 { get; set; }
        [Display(Name = "国家标准行业")]
        public string 国家标准行业 { get; set; }
        [Display(Name = "税务登记日期")]
        public string 税务登记日期 { get; set; }
        [Display(Name = "主管税务机关")]
        public string 主管税务机关 { get; set; }
        [Display(Name = "纳税人状态")]
        public string 纳税人状态 { get; set; }


        [Display(Name = "注册资本")]
        public string 注册资本 { get; set; }
        [Display(Name = "实收资本")]
        public string 实收资本 { get; set; }
        [Display(Name = "实缴出资金额")]
        public string 实缴出资金额 { get; set; }
        [Display(Name = "资金单位")]
        public string 资金单位 { get; set; }
        [Display(Name = "币种")]
        public string 币种 { get; set; }
        [Display(Name = "最终实缴出资时间")]
        public string 最终实缴出资时间 { get; set; }
        [Display(Name = "最终认缴出资时间")]
        public string 最终认缴出资时间 { get; set; }

        #endregion

        [Display(Name = "TaskGuid")]
        public Guid TaskGuid { set; get; }
        [Display(Name = "操作人姓名")]
        public string 操作人姓名 { get; set; }
        [Display(Name = "入爬行库时间")]
        public DateTime? 入爬行库时间 { set; get; }
        [Display(Name = "爬行更新时间")]
        public DateTime? 爬行更新时间 { set; get; }
    }
}
