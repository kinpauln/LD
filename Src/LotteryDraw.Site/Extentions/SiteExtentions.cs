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
                MemberView = p.Member.ToSiteViewModel(),
                MemberId = p.Member.Id,
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
            MemberView mv = new MemberView()
            {
                Id = m.Id,
                UserName = m.UserName,
                Password = m.Password,
                Email = m.Email,
                Name = m.Name,
                AddDate = m.AddDate,
                MemberType = m.MemberType,
                LoginLogCount = m.LoginLogs.Count,
                RoleNames = m.Roles.AsEnumerable().Select(r => r.Name)
            };
            if (m.Extend != null)
            {
                mv.Tel = m.Extend.Tel;
                mv.AdvertisingUrl = m.Extend.AdvertisingUrl;
                mv.Province = m.Extend.Address.Province;
                mv.City = m.Extend.Address.City;
                mv.Town = m.Extend.Address.Town;
                mv.AddrSuffix = m.Extend.Address.Suffix;
            }
            return mv;
        }

        public static PrizeOrderView ToSiteViewModel(this PrizeOrder po)
        {
            if (po == null)
                return null;
            var rentity = new PrizeOrderView()
            {
                Id = po.Id,
                RevealType = po.RevealType,
                RevealTypeNum = po.RevealTypeNum,
                RevealState = po.RevealState,
                RevealStateNum = po.RevealStateNum,
                ScopeType = po.Extend.ScopeType,
                ScopeTypeNum = po.Extend.ScopeTypeNum,
                //ScopeProvince = po.Extend.ScopeCity,
                ScopeAreaCity = po.Extend.ScopeCity,
                AnswerRevealConditionType = po.Extend.AnswerRevealConditionType,
                AnswerRevealConditionTypeNum = po.Extend.AnswerRevealConditionTypeNum,
                //Remarks = po.Extend,
                PrizeId = po.Prize.Id,
                PrizeView = po.Prize.ToSiteViewModel(),
                SortOrder = po.SortOrder ?? 0,
                LaunchTime = po.Extend.LaunchTime,
                RevealDate = po.RevealDate,
                MinLuckyCount = po.Extend.MinLuckyCount,
                LuckyPercent = po.Extend.LuckyPercent,
                PoolCount = po.Extend.PoolCount,
                LuckyCount = po.Extend.LuckyCount,
                Question = po.Extend.PrizeAsking.Question,
                AnswerOptions = po.Extend.PrizeAsking.AnswerOptions,
                Answer = po.Extend.PrizeAsking.Answer
                //LuckyStaffsOfScenceString = po.Extend.
                //StaffsOfScenceString = po.SceneStaffs.AsEnumerable().ToArray().,
                //Is2Top = po.Extend,
                //UpdateDate = po.,
            };

            if (po.RevealType == RevealType.Scene)
            {
                // 参与者数目
                if (po.SceneStaffs != null)
                {
                    rentity.BettingCount = po.SceneStaffs.Count;
                }
                // 已开奖的，取出中奖者
                if (po.RevealState == RevealState.Drawn)
                {
                    if (po.SceneStaffs != null && po.SceneStaffs.Count > 0)
                    {
                        rentity.LuckyStaffsOfScenceString = string.Join("|", po.SceneStaffs.Where(ss => ss.IsLucky).Select(ss => ss.Value).ToArray());
                    }
                }
            }
            else
            {
                // 投注者数目
                if (po.PrizeBettings != null)
                {
                    rentity.BettingCount = po.PrizeBettings.Where(pb => !pb.IsDeleted).Count();
                }
                // 白名单数目
                if (po.WhiteLists != null)
                {
                    rentity.BettingCount = rentity.BettingCount + po.WhiteLists.Where(wl => !wl.IsDeleted).Count();
                }
            }

            return rentity;
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
            DataRow[] rows = new DataRow[dt.Rows.Count];
            dt.Rows.CopyTo(rows, 0);
            return rows.ToPrizeOrderDetailList();
        }

        public static IEnumerable<PrizeOrderDetailView> ToPrizeOrderDetailList(this DataRow[] rows)
        {
            List<PrizeOrderDetailView> rlist = new List<PrizeOrderDetailView>();
            foreach (DataRow row in rows)
            {
                var detail = new PrizeOrderDetailView()
                {
                    PrizeOrderView = new PrizeOrderView()
                    {
                        Id = new Guid(row["PrizeOrderId"].ToString()),
                        PrizeId = new Guid(row["PrizeId"].ToString()),
                        RevealTypeNum = int.Parse(row["RevealType"].ToString()),
                        RevealStateNum = int.Parse(row["RevealState"].ToString()),
                        SortOrder = int.Parse(row["SortOrder"].ToString()),
                        LuckyCount = int.Parse(row["LuckyCount"].ToString()),
                        AddDate = Convert.ToDateTime(row["RaiseTime"]),
                        ScopeAreaCity = row["ScopeCity"].ToString(),
                        ScopeTown = row["ScopeTown"].ToString(),
                        ScopeTypeNum = int.Parse(row["ScopeType"].ToString()),
                        AnswerRevealConditionTypeNum = int.Parse(row["AnswerRevealConditionTypeNum"].ToString()),
                        Freight = Convert.ToDecimal(row["Freight"]),
                        PresalePrice = Convert.ToDecimal(row["PresalePrice"]),
                        ForgedParticipantCount = Convert.ToInt32(row["ForgedParticipantCount"])
                        //Is2Top = containIs2Top ? Convert.ToBoolean(row["Is2Top"]) : false
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
                        Id = long.Parse(row["MemberId"].ToString()),
                        UserName = row["UserName"].ToString(),
                        Name = row["UserNickName"].ToString(),
                        AdvertisingUrl = row["AdvertisingUrl"].ToString()
                    }
                };
                if (!(row["LaunchTime"] is System.DBNull))
                {
                    detail.PrizeOrderView.LaunchTime = Convert.ToDateTime(row["LaunchTime"]);
                }

                if (!(row["PoolCount"] is System.DBNull))
                {
                    detail.PrizeOrderView.PoolCount = Convert.ToInt32(row["PoolCount"]);
                }

                if (row.Table.Columns.Contains("RevealTypeOfAnswerNum"))
                {
                    detail.PrizeOrderView.RevealTypeOfAnswerNum = int.Parse(row["RevealTypeOfAnswerNum"].ToString());
                }

                if (row.Table.Columns.Contains("Is2Top"))
                {
                    detail.PrizeOrderView.Is2Top = Convert.ToBoolean(row["Is2Top"]);
                }
                else
                {
                    detail.PrizeOrderView.Is2Top = false;
                }

                if (row.Table.Columns.Contains("StaffTotalCount"))
                {
                    detail.PrizeOrderView.StaffTotalCount = Convert.ToInt32(row["StaffTotalCount"]);
                }


                if (row.Table.Columns.Contains("LuckyStaffs"))
                {
                    detail.PrizeOrderView.LuckyStaffsOfScenceString = row["LuckyStaffs"].ToString();
                }

                if (row.Table.Columns.Contains("BettingCount"))
                {
                    detail.PrizeOrderView.BettingCount = Convert.ToInt32(row["BettingCount"]);
                }

                if (row.Table.Columns.Contains("WhiteListCount"))
                {
                    detail.PrizeOrderView.WhiteListCount = Convert.ToInt32(row["WhiteListCount"]);
                }

                rlist.Add(detail);
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
