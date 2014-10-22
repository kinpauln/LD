using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public abstract class ModelBase
    {
        public bool IsDeleted { get; set; }

        public DateTime AddDate { get; set; }

        public object GetDefaultValue(string attributeName){
            AttributeCollection attrColl = TypeDescriptor.GetProperties(this)[attributeName].Attributes;
            DefaultValueAttribute attr = attrColl[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
            return attr.Value;  
        }
    }
}
