using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LotteryDraw.Component.Tools;
using LotteryDraw.Site.Models;
using System.Data;

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

        public static IEnumerable<PrizeOrderDetailView> ToPrizeOrderDetailList(this DataTable dt) {
            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<PrizeOrderDetailView> rlist = new List<PrizeOrderDetailView>();
            foreach (DataRow row in dt.Rows) {
                rlist.Add(new PrizeOrderDetailView()
                {
                    PrizeOrderView = new PrizeOrderView() {
                        Id = new Guid(row["PrizeOrderId"].ToString()),
                        PrizeId = new Guid(row["PrizeId"].ToString()),
                        RevealTypeNum = int.Parse(row["RevealType"].ToString()),
                        RevealStateNum = int.Parse(row["RevealState"].ToString()),
                        SortOrder = int.Parse(row["SortOrder"].ToString()),
                        AddDate = Convert.ToDateTime(row["RaiseTime"])
                    },
                    PrizeView = new PrizeView() {
                        Id = new Guid(row["PrizeId"].ToString()),
                        Name = row["PrizeName"].ToString(),
                        Description = row["PrizeDescription"].ToString()
                    },
                    MemberView = new MemberView() {
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
