using LotteryDraw.Component.Tools;
using LotteryDraw.Component.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LotteryDraw.Site.Models
{
    public class PrizeView : ModelBase
    {
        public Guid Id { get; set; }

        //[Required(ErrorMessage = "{0}不能为空！")]
        [Display(Name = "奖品名称")]
        public string Name { get; set; }

        public string PhotoBase64
        {
            get
            {
                return StreamUtil.BytesToBase64(this.Photo);
            }
            set { }
        }

        public byte[] Photo
        {
            get;
            set;
        }

        public IEnumerable<PrizePhotoView> Photos { get; set; }

        private PrizePhotoView _originalPhoto;
        public PrizePhotoView OriginalPhoto
        {
            get
            {
                if (_originalPhoto != null) return _originalPhoto;
                if (Photos == null)
                    return null;
                var photo = Photos.Where(p => p.PhotoTypeNum == PhotoType.Original.ToInt()).FirstOrDefault();
                return photo;
            }
            set
            {
                _originalPhoto = value;
            }
        }

        [Display(Name = "描述")]
        public string Description { get; set; }

        public long MemberId { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
