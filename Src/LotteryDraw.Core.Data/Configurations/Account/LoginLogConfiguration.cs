using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using LotteryDraw.Component.Data;
using LotteryDraw.Core.Models;


namespace LotteryDraw.Core.Data.Configurations.Account
{
    partial class LoginLogConfiguration
    {
        partial void LoginLogConfigurationAppend()
        {
            HasRequired(m => m.Member).WithMany(n => n.LoginLogs);
        }
    }
}