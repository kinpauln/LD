using LotteryDraw.Component.Tools;
using LotteryDraw.Component.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class PrizeView
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0}不能为空！")]
        [Display(Name = "奖品名称")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}不能为空！")]
        [Display(Name = "图片")]
        public byte[] Photo { get; set; }

        public string PhotoBase64
        {
            get
            {
                return StreamUtil.BytesToBase64(this.Photo);
            }
            set { }
        }

        [Display(Name = "描述")]
        public string Description { get; set; }

        public int MemberId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
