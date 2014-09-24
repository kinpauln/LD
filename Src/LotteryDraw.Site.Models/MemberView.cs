using LotteryDraw.Component.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class MemberView
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime AddDate { get; set; }

        public int LoginLogCount { get; set; }

        public IEnumerable<string> RoleNames { get; set; }

        public MemberType MemberType { get; set; }
    }
}
