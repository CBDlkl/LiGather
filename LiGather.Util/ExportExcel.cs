using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace LiGather.Util
{
    public static class ExportExcel
    {
        /// <summary>
        /// 导出Excel(03-07) 泛型集合操作
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="lists">数据源</param>
        /// <param name="isOptimize">是否自动列宽，默认否</param>
        /// <returns></returns>
        public static byte[] ListToExcel<T>(this IList<T> lists, bool isOptimize = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                //创建一个名称为Payment的工作表
                ISheet paymentSheet = workbook.CreateSheet("sheet");
                //头部标题
                IRow paymentHeaderRow = paymentSheet.CreateRow(0);

                PropertyInfo[] propertys = lists[0].GetType().GetProperties();
                //循环添加标题
                for (int i = 0; i < propertys.Length; i++)
                    paymentHeaderRow.CreateCell(i).SetCellValue(propertys[i].Name);
                // 内容
                int paymentRowIndex = 1;
                for (int index = 0; index < lists.Count; index++)
                {
                    IRow newRow = paymentSheet.CreateRow(paymentRowIndex);

                    //列宽自适应，只对英文和数字有效
                    if (isOptimize)
                        paymentSheet.AutoSizeColumn(index);
                    //循环添加列的对应内容
                    for (int i = 0; i < propertys.Length; i++)
                    {
                        var obj = propertys[i].GetValue(lists[index], null);
                        newRow.CreateCell(i).SetCellValue((obj ?? "").ToString());
                    }
                    paymentRowIndex++;
                }

                //将表内容写入流 等待下一步操作
                workbook.Write(ms);
                return ms.ToArray();
            }
        }
    }
}
