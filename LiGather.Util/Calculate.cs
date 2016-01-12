using System;
using System.Collections.Generic;

namespace LiGather.Util
{
    public class Calculate
    {
        /// <summary>
        /// 将一个集合均等份
        /// </summary>
        /// <param name="target">目标数值</param>
        /// <param name="avgNum">平分成多少等分</param>
        /// <returns></returns>
        [Obsolete("这个方法执行不正确，而且我没有时间去修改", true)]
        public static List<List<int>> CutAvg(int target, int avgNum)
        {
            var lists = new List<List<int>>();
            var thresholdNum = (int) Math.Ceiling(Conv.ToDecimal(target)/Conv.ToDecimal(avgNum)) + 1;
            var start = 1;
            for (var i = 0; i < avgNum; i++)
            {
                var end = (thresholdNum * (i + 1) - 1) >= target ? target - 1 : (thresholdNum * (i + 1) - 1);
                var ints = new List<int>();
                for (var j = start; j < end; j++)
                    ints.Add(j);
                start = end;
                lists.Add(ints);
            }
            return lists;
        }
    }
}
