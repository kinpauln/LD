using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using LotteryDraw.Component.Data;
using LotteryDraw.Core.Models.Business;


namespace LotteryDraw.Core.Data.Configurations.Business
{
    /// <summary>
    /// 实体类-数据表映射——奖品信息
    /// </summary>    
	internal partial class PrizeBettingConfiguration
    {
        partial void PrizeBettingConfigurationAppend()
        {
            HasRequired(pb => pb.Member).WithMany(n => n.PrizeBettings);
            HasRequired(pb => pb.PrizeOrder).WithMany(n => n.PrizeBettings);
        }
    }
}
