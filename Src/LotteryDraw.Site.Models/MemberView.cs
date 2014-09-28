using LotteryDraw.Component.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class MemberView : ModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}不能为空！")]
        [Display(Name = "登录账号")]
        public string UserName { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "{0}不能为空！")]
        [Display(Name = "登录密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0}不能为空！")]
        public string Email { get; set; }

        public string Tel { get; set; }

        public int LoginLogCount { get; set; }

        public IEnumerable<string> RoleNames { get; set; }

        public MemberType MemberType { get; set; }
    }
}
