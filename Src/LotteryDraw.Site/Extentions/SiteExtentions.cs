using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LotteryDraw.Component.Tools;

namespace LotteryDraw.Site.Extentions
{
    public static partial class SiteExtentions
    {

        /// <summary>
        /// 将枚举转换到Select的扩展方法
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="enumObj">枚举对象</param>
        /// <returns>SelectList</returns>
        public static SelectList ToNameSelectList<TEnum>(this TEnum enumObj) where TEnum : struct
        {
            
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = Convert.ToInt32(e), Name = e.ToString() };

            return new SelectList(values, "Id", "Name", Convert.ToInt32(enumObj));
        }

        /// <summary>
        /// 将枚举转换到Select的扩展方法
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="enumObj">枚举对象</param>
        /// <returns>SelectList</returns>
        public static SelectList ToDescriptionSelectList<TEnum>(this TEnum enumObj) where TEnum : struct
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = Convert.ToInt32(e), Name = e.GetDescription() };

            return new SelectList(values, "Id", "Name", Convert.ToInt32(enumObj));
        }
    }
}
