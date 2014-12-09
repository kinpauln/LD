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
    internal partial class SceneStaffConfiguration
    {
        partial void SceneStaffConfigurationAppend()
        {
            HasRequired(pb => pb.PrizeOrder).WithMany(n => n.SceneStaffs);
        }
    }
}
