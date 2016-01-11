using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LiGather.Model.Domain
{
    public class ProxyEntity
    {
        [Key]
        public int Id { set; get; }
        /// <summary>
        /// IP地址
        /// </summary>
        [Description("IP地址")]
        public string IpAddress { set; get; }
        /// <summary>
        /// 端口
        /// </summary>
        [Description("端口")]
        public int Port { set; get; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Description("是否可用")]
        public bool? CanUse { set; get; }

        /// <summary>
        /// 物理地址
        /// </summary>
        [Description("物理地址")]
        public string AddressName { set; get; }

        /// <summary>
        /// 入库时间
        /// </summary>
        [Description("入库时间")]
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 使用次数
        /// </summary>
        [Description("使用次数")]
        public int Usage { set; get; }

        /// <summary>
        /// 最后使用时间
        /// </summary>
        [Description("最后使用时间")]
        public DateTime? LastUseTime { set; get; }
    }
}
