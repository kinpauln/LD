using LotteryDraw.Component.Tools;
using LotteryDraw.Component.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class PrizePhotoView : ModelBase
    {
        public Guid Id { get; set; }

        [Display(Name = "图片名称")]
        public string Name { get; set; }

        /// <summary>
        /// 开奖类型
        /// </summary>
        [Display(Name = "图片类型")]
        public PhotoType RevealType
        {
            get { return (PhotoType)PhotoTypeNum; }
            set { PhotoTypeNum = (int)value; }
        }

        public int PhotoTypeNum { get; set; }

        public Guid PrizeId { get; set; }
    }
}
