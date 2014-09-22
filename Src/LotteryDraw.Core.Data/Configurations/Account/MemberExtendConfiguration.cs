using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

using LotteryDraw.Component.Data;
using LotteryDraw.Core.Models;


namespace LotteryDraw.Core.Data.Configurations.Account
{
    partial class MemberExtendConfiguration
    {
        partial void MemberExtendConfigurationAppend()
        {
            HasRequired(m => m.Member).WithOptional(n => n.Extend);
        }
    }
}
