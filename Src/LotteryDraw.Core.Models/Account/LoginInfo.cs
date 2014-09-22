// 源文件头信息：
// <copyright file="LoginInfo.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Core.Models
// 最后修改：王金鹏
// 最后修改：2014/08/06 23:47
// </copyright>

namespace LotteryDraw.Core.Models.Account
{
    /// <summary>
    ///     登录信息类
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        ///     获取或设置 登录账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///     获取或设置 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     获取或设置 IP地址
        /// </summary>
        public string IpAddress { get; set; }
    }
}