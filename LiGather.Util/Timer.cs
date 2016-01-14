using System;

namespace LiGather.Util
{
    /// <summary>
    /// 时间计算
    /// </summary>
    public class Timer
    {
        /// <summary>  
        /// 已重载.计算两个日期的时间间隔,返回的是时间间隔的日期差的绝对值.  
        /// </summary>  
        /// <param name="dateTime1">第一个日期和时间</param>  
        /// <param name="dateTime2">第二个日期和时间</param>  
        /// <returns></returns>  
        public static string DateDiff(DateTime dateTime1, DateTime dateTime2)
        {
            TimeSpan ts1 = new TimeSpan(dateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(dateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            var dateDiff = ts.Days + "天"
                              + ts.Hours + "小时"
                              + ts.Minutes + "分钟"
                              + ts.Seconds + "秒";
            return dateDiff;
        }
    }
}
