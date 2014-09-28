using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

using LotteryDraw.Component.Data;
using LotteryDraw.Core.Models.Business;


namespace LotteryDraw.Core.Data.Configurations
{
    public class PrizeAskingConfiguration : ComplexTypeConfiguration<PrizeAsking>, IEntityMapper
    {
        public PrizeAskingConfiguration()
        {
            Property(m => m.Question).HasColumnName("Question");
            Property(m => m.Answer).HasColumnName("Answer");
        }

        public void RegistTo(ConfigurationRegistrar configurations)
        {
            configurations.Add(this);
        }
    }
}
