using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public abstract class ModelBase
    {
        public bool IsDeleted { get; set; }

        public DateTime AddDate { get; set; }
    }
}
