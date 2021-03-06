﻿using LotteryDraw.Component.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class MemberView : ModelBase
    {
        public long Id { get; set; }

        //[Required(ErrorMessage = "{0}不能为空！")]
        [Display(Name = "登录账号")]
        public string UserName { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///  昵称
        /// </summary>
        public string NickName { get; set; }

        //[Required(ErrorMessage = "{0}不能为空！")]
        [Display(Name = "登录密码")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "{0}不能为空！")]
        public string Email { get; set; }

        public string Tel { get; set; }

        public string AdvertisingUrl { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Town { get; set; }

        public string AddrSuffix { get; set; }

        public string Address
        {
            get
            {
                return (string.IsNullOrEmpty(Province) ? "" : Province) + (string.IsNullOrEmpty(City) ? string.Empty : City) + (string.IsNullOrEmpty(Town) ? "" : Town) + (string.IsNullOrEmpty(AddrSuffix) ? "" : AddrSuffix);
            }
        }

        public int LoginLogCount { get; set; }

        public IEnumerable<string> RoleNames { get; set; }

        public MemberType MemberType { get; set; }

        public int PubishingEnableTimes { get; set; }

        /// <summary>
        ///  新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        ///  确认新密码
        /// </summary>
        public string ConfirmNewPassword { get; set; }
    }
}
