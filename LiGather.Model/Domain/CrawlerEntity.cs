using System;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.Domain
{
    public class CrawlerEntity
    {
        [Key]
        public int Id { set; get; }

        /// <summary> 
        ///  查询名称
        /// </summary> 
        public string 查询名称 { get; set; }
        /// <summary> 
        ///  统一社会信用代码 
        /// </summary> 
        public string 统一社会信用代码 { get; set; }
        public string 类型 { get; set; }
        public string 法定代表人 { get; set; }
        public DateTime? 成立日期 { get; set; }
        public string 住所 { get; set; }
        public DateTime? 营业期限自 { get; set; }
        public DateTime? 营业期限至 { get; set; }
        public string 经营范围 { get; set; }
        public string 登记机关 { get; set; }
        public string 核准日期 { get; set; }
        /// <summary> 
        ///  是否为历史名称
        /// </summary> 
        public string 是否为历史名称 { get; set; }
    }
}
