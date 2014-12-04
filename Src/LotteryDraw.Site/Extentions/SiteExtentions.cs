using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LotteryDraw.Component.Tools;
using LotteryDraw.Site.Models;
using System.Data;
using LotteryDraw.Core.Models.Business;
using LotteryDraw.Core.Models.Account;

namespace LotteryDraw.Site.Extentions
{
    public static partial class SiteExtentions
    {

        /// <summary>
        /// 将枚举转换到Select的扩展方法
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="enumObj">枚举对象</param>
        /// <returns>SelectList</returns>
        public static SelectList ToNameSelectList<TEnum>(this TEnum enumObj) where TEnum : struct
        {

            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = Convert.ToInt32(e), Name = e.ToString() };

            return new SelectList(values, "Id", "Name", Convert.ToInt32(enumObj));
        }

        public static IEnumerable<PrizePhotoView> ToSiteViewModels(this IEnumerable<PrizePhoto> photos)
        {
            List<PrizePhotoView> rlist = new List<PrizePhotoView>();
            foreach (PrizePhoto item in photos)
            {
                rlist.Add(item.ToSiteViewModel());
            }
            return rlist;
        }

        public static PrizePhotoView ToSiteViewModel(this PrizePhoto photo)
        {
            if (photo == null)
                return null;
            return new PrizePhotoView()
                {
                    Name = photo.Name,
                    PhotoTypeNum = photo.PhotoTypeNum,
                    PrizeId = photo.Prize.Id,
                    AddDate = photo.AddDate,
                    IsDeleted = photo.IsDeleted
                };
        }

        public static PrizeView ToSiteViewModel(this Prize p)
        {
            if (p == null)
                return null;
            return new PrizeView()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                AddDate = p.AddDate,
                UpdateDate = p.UpdateDate,
                Photo = p.Photo,
                Photos = p.PrizePhotos.ToList().ToSiteViewModels()
            };
        }

        public static MemberView ToSiteViewModel(this Member m)
        {
            if (m == null)
                return null;
            return new MemberView()
            {
                Id = m.Id,
                UserName = m.UserName,
                Password = m.Password,
                Email = m.Email,
                Name = m.Name,
                AddDate = m.AddDate,
                Tel = m.Extend.Tel,
                AdvertisingUrl = m.Extend.AdvertisingUrl,
                Province = m.Extend.Address.Province,
                City = m.Extend.Address.City,
                Town = m.Extend.Address.Town,
                AddrSuffix = m.Extend.Address.Suffix,
                MemberType = m.MemberType,
                LoginLogCount = m.LoginLogs.Count,
                RoleNames = m.Roles.AsEnumerable().Select(r => r.Name)
            };
        }

        /// <summary>
        /// 将枚举转换到Select的扩展方法
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="enumObj">枚举对象</param>
        /// <returns>SelectList</returns>
        public static SelectList ToDescriptionSelectList<TEnum>(this TEnum enumObj) where TEnum : struct
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = Convert.ToInt32(e), Name = e.GetDescription() };

            return new SelectList(values, "Id", "Name", Convert.ToInt32(enumObj));
        }

        public static IEnumerable<PrizeOrderDetailView> ToPrizeOrderDetailList(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<PrizeOrderDetailView> rlist = new List<PrizeOrderDetailView>();
            foreach (DataRow row in dt.Rows)
            {
                rlist.Add(new PrizeOrderDetailView()
                {
                    PrizeOrderView = new PrizeOrderView()
                    {
                        Id = new Guid(row["PrizeOrderId"].ToString()),
                        PrizeId = new Guid(row["PrizeId"].ToString()),
                        RevealTypeNum = int.Parse(row["RevealType"].ToString()),
                        RevealStateNum = int.Parse(row["RevealState"].ToString()),
                        SortOrder = int.Parse(row["SortOrder"].ToString()),
                        AddDate = Convert.ToDateTime(row["RaiseTime"])
                    },
                    PrizeView = new PrizeView()
                    {
                        Id = new Guid(row["PrizeId"].ToString()),
                        Name = row["PrizeName"].ToString(),
                        Description = row["PrizeDescription"].ToString(),
                        OriginalPhoto = new PrizePhotoView() { Name = row["OriginalPhotoName"].ToString() }
                    },
                    MemberView = new MemberView()
                    {
                        UserName = row["UserName"].ToString(),
                        Name = row["UserNickName"].ToString()
                    }
                });
            }

            return rlist;
        }


        public static IEnumerable<MemberView> ToMemberViewList(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<MemberView> rlist = new List<MemberView>();
            foreach (DataRow row in dt.Rows)
            {
                rlist.Add(new MemberView()
                {
                    Id = int.Parse(row["MemberId"].ToString()),
                    Name = row["Name"].ToString(),
                    UserName = row["UserName"].ToString(),
                    Email = row["Email"].ToString(),
                    Tel = row["Tel"].ToString(),
                    AddDate = Convert.ToDateTime(row["AddDate"]),
                    //LoginLogCount = int.Parse(row["PrizeOrderId"].ToString()),
                    //MemberType = row["PrizeOrderId"].ToString()                     
                });
            }

            return rlist;
        }
    }
}
