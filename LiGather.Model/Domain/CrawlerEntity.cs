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
        public string 搜索名称 { get; set; }

        #region 正文
        public string 名称 { set; get; }
        public string 全局唯一编号 { set; get; }
        public string 统一社会信用代码 { set; get; }
        public string 注册号 { get; set; }
        public string 类型 { get; set; }
        public string 负责人 { get; set; }
        public string 法定代表人 { set; get; }
        public string 营业场所 { get; set; }
        public string 住所 { set; get; }
        public DateTime? 营业期限自 { get; set; }
        public DateTime? 营业期限至 { get; set; }
        public string 经营范围 { get; set; }
        public string 登记机关 { get; set; }
        public DateTime? 核准日期 { get; set; }
        public DateTime? 成立日期 { get; set; }
        public string 登记状态 { get; set; }

        public string 纳税人名称 { get; set; }
        public string 税务登记类型 { get; set; }
        public string 税务登记证号 { get; set; }
        public string 法人姓名 { get; set; }
        public string 组织机构代码 { set; get; }
        public string 登记受理类型 { set;get;}
        public string 经营地址 { get; set; }
        public string 经营地址联系电话 { get; set; }
        public string 经营地址邮编 { get; set; }
        public string 企业主页网址 { get; set; }
        public string 所处街乡 { get; set; }
        public string 国地税共管户标识 { get; set; }
        public string 登记注册类型 { get; set; }
        public string 隶属关系 { get; set; }
        public string 国家标准行业 { get; set; }
        public DateTime? 税务登记日期 { get; set; }
        public string 主管税务机关 { get; set; }
        public string 纳税人状态 { get; set; }


        public string 注册资本 { get; set; }
        public string 实收资本 { get; set; }
        public string 实缴出资金额 { get; set; }
        public DateTime? 最终实缴出资时间 { get; set; }
        public DateTime? 最终认缴出资时间 { get; set; }

        #endregion

        public Guid TaskGuid { set; get; }
        public string 操作人姓名 { get; set; }
        public DateTime? 入爬行库时间 { set; get; }
        public DateTime? 爬行更新时间 { set; get; }
    }
}
