// 源文件头信息：
// <copyright file="EnumExtensions.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Component.Tools
// 最后修改：王金鹏
// 最后修改：2014/09/12 0:51
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;


namespace LotteryDraw.Component.Tools
{
    /// <summary>
    ///     枚举扩展方法类
    /// </summary>
    public static partial class EnumExtensions
    {
        /// <summary>
        ///     获取枚举项的Description特性的描述文字
        /// </summary>
        /// <param name="enumeration"> </param>
        /// <returns> </returns>
        public static string ToDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] members = type.GetMember(enumeration.CastTo<string>());
            if (members.Length > 0)
            {
                return members[0].ToDescription();
            }
            return enumeration.CastTo<string>();
        }

        /// <summary>
        /// 返回枚举类型的中文描述 DescriptionAttribute 指定的名字
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumobj"></param>
        /// <returns></returns>
        public static string Description<TEnum>(this TEnum enumobj) where TEnum : struct
        {
            var returnStr = "";
            var em = enumobj.ToString();
            var emArr = em.Split(',');
            foreach (var s in emArr)
            {
                returnStr += ((TEnum)System.Enum.Parse(typeof(TEnum), s)).GetDescription();
            }
            /*
                  这里可以用linq中的累加器Aggregate来进行替代，鉴于代码易读采用foreach，可被替换为如下更简洁的代码
               return emArr.Aggregate("", (current, s) => current + ((TEnum) System.enumobj.Parse(typeof (TEnum), s)).GetDescription());
            */
            return returnStr;
        }

        public static string GetDescription<TEnum>(this TEnum Enum) where TEnum : struct
        {
            var em = Enum.ToString();
            FieldInfo fieldInfo = Enum.GetType().GetField(em);
            if (fieldInfo == null) return em;
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length < 1) return em;
            return attributes[0].Description;
        }

        /// <summary>
        /// 获取枚举类型转化为int值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="thisStatus"></param>
        /// <returns></returns>
        public static int ToInt<TEnum>(this TEnum thisStatus) where TEnum : struct
        {
            return thisStatus.GetHashCode();
        }

        /// <summary>
        /// 将枚举以字典的形式返回
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static Dictionary<int, string> ToSelectList<TEnum>(this TEnum enumObj) where TEnum : struct
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new KeyValuePair<int, string>(e.GetHashCode(), e.ToString());
            return values.ToDictionary(a => a.Key, b => b.Value);
        }

        /// <summary>
        /// 返回枚举值列表
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static List<TEnum> ToEnumList<TEnum>(this TEnum enumObj) where TEnum : struct
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select e;
            return values.ToList();
        }

        /// <summary>
        /// 转换成为枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strEnum"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string strEnum)
        {
            return (T)Enum.Parse(typeof(T), strEnum);
        }
    }
}