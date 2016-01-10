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
        public string Port { set; get; }
        /// <summary>
        /// 使用次数
        /// </summary>
        [Description("使用次数")]
        public int Usage { set; get; }
    }
}
