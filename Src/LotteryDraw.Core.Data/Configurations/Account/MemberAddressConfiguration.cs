using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

using LotteryDraw.Component.Data;
using LotteryDraw.Core.Models.Account;


namespace LotteryDraw.Core.Data.Configurations
{
    public class MemberAddressConfiguration : ComplexTypeConfiguration<MemberAddress>, IEntityMapper
    {
        public MemberAddressConfiguration()
        {
            Property(m => m.Province).HasColumnName("Province");
            Property(m => m.City).HasColumnName("City");
            Property(m => m.Town).HasColumnName("Town");
            Property(m => m.Suffix).HasColumnName("Suffix");
        }

        public void RegistTo(ConfigurationRegistrar configurations)
        {
            configurations.Add(this);
        }
    }
}
