using System.Web.Mvc;

namespace LiGather.Web.App_Code
{
    public static class LaberHtmlHelper
    {
        /// <summary>
        /// 状态标签
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="text">状态名称</param>
        /// <param name="labelClass">状态样式</param>
        /// <returns></returns>
        public static MvcHtmlString LabelForState(this HtmlHelper helper, string text, string labelClass)
        {
            var builder = new TagBuilder("span");
            builder.MergeAttribute("class", "label " + labelClass);
            builder.InnerHtml = text;
            builder.ToString(TagRenderMode.SelfClosing);
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}