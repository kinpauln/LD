﻿// 源文件头信息：
// <copyright file="StringHelper.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Component.Tools
// 最后修改：王金鹏
// 最后修改：2014/08/06 23:04
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace LotteryDraw.Component.Tools
{
    /// <summary>
    ///     字符串辅助操作类
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        ///     把对象转换成Json字符串表示形式
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static string BuildJsonString(object jsonObject)
        {
            PublicHelper.CheckArgument(jsonObject, "jsonObject");
            return JsonConvert.SerializeObject(jsonObject);
        }

        /// <summary>
        ///     判断指定字符串是否对象（Object）类型的Json字符串格式
        /// </summary>
        /// <param name="input">要判断的Json字符串</param>
        /// <returns></returns>
        public static bool IsJsonObjectString(string input)
        {
            return input != null && input.StartsWith("{") && input.EndsWith("}");
        }

        /// <summary>
        ///     判断指定字符串是否集合类型的Json字符串格式
        /// </summary>
        /// <param name="input">要判断的Json字符串</param>
        /// <returns></returns>
        public static bool IsJsonArrayString(string input)
        {
            return input != null && input.StartsWith("[") && input.EndsWith("]");
        }
    }
}