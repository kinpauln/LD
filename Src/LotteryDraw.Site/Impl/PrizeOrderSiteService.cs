// 源文件头信息：
// <copyright file="AccountSiteService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR版本：4.0.30319.239
// 开发组织：王金鹏@中国
// 公司网站：http://www.wuliubang.net/
// 所属工程：LotteryDraw.Site
// 最后修改：王金鹏
// 最后修改：2013/05/20 13:06
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Impl;
using LotteryDraw.Core.Models;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Site.Models;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Models.Business;
using LotteryDraw.Core;
using System.Data;
using LotteryDraw.Site.Extentions;


namespace LotteryDraw.Site.Impl
{
    /// <summary>
    ///     账户模块站点业务实现
    /// </summary>
    [Export(typeof(IPrizeOrderSiteContract))]
    internal class PrizeOrderSiteService : IPrizeOrderSiteContract
    {
        [Import]
        protected IPrizeOrderContract PrizeOrderContract { get; set; }

        [Import]
        protected IPrizeContract PrizeContract { get; set; }

        /// <summary>
        ///     添加奖品
        /// </summary>
        /// <param name="prizebetting">奖品信息</param>
        /// <param name="shouldMinus">是否该对用户的可发起抽奖次数减</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Add(PrizeOrderView povmodel, bool shouldMinus = false)
        {
            PublicHelper.CheckArgument(povmodel, "povmodel");
            int? sortorder = PrizeOrderContract.PrizeOrders.Max(po => po.SortOrder);

            PrizeOrder pmodel = new PrizeOrder
            {
                RevealTypeNum = povmodel.RevealTypeNum,
                RevealState = Component.Tools.RevealState.UnDrawn,
                Prize = PrizeContract.Prizes.SingleOrDefault(m => m.Id.Equals(povmodel.PrizeId)),
                Extend = new PrizeOrderExtend() { Id = povmodel.Id },
                SortOrder = (sortorder ?? 0) + 1
            };

            switch (povmodel.RevealType)
            {
                case RevealType.Timing:
                    pmodel.Extend.LaunchTime = povmodel.LaunchTime;
                    pmodel.Extend.MinLuckyCount = povmodel.MinLuckyCount;
                    pmodel.Extend.LuckyCount = povmodel.LuckyCount;
                    //pmodel.Extend.LuckyPercent = povmodel.LuckyPercent;
                    break;
                case RevealType.Quota:
                    pmodel.Extend.PoolCount = povmodel.PoolCount;
                    pmodel.Extend.MinLuckyCount = povmodel.MinLuckyCount;
                    pmodel.Extend.LuckyCount = povmodel.LuckyCount;
                    //pmodel.Extend.LuckyPercent = povmodel.LuckyPercent;
                    break;
                case RevealType.Answer:
                    pmodel.Extend.PrizeAsking.Question = povmodel.Question;
                    pmodel.Extend.PrizeAsking.AnswerOptions = povmodel.AnswerOptions;
                    pmodel.Extend.PrizeAsking.Answer = povmodel.Answer;
                    pmodel.Extend.MinLuckyCount = povmodel.MinLuckyCount;
                    pmodel.Extend.LuckyCount = povmodel.LuckyCount;

                    pmodel.Extend.AnswerRevealConditionTypeNum = (int)povmodel.AnswerRevealConditionType;
                    if (povmodel.AnswerRevealConditionType == AnswerRevealConditionType.Timing)
                    {
                        pmodel.Extend.LaunchTime = povmodel.LaunchTime;
                    }
                    else if (povmodel.AnswerRevealConditionType == AnswerRevealConditionType.Quota)
                    {
                        pmodel.Extend.PoolCount = povmodel.PoolCount;
                    }
                    //pmodel.Extend.LuckyPercent = povmodel.LuckyPercent;
                    break;
                case RevealType.Scene:
                    break;
            }
            try
            {
                return PrizeOrderContract.Add(pmodel, shouldMinus);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///     更新奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Update(PrizeOrderView pvmodel)
        {
            PublicHelper.CheckArgument(pvmodel, "pvmodel");
            try
            {
                PrizeOrder dbmodel = PrizeOrderContract.PrizeOrders.SingleOrDefault(m => m.Id.Equals(pvmodel.Id));
                if (dbmodel == null)
                {
                    return new OperationResult(OperationResultType.Error, string.Format("不存在要更新的Id为{0}的奖单", pvmodel.Id));
                }

                //dbmodel.Name = pvmodel.Name;
                //dbmodel.Description = pvmodel.Description;
                //if (savePhoto) {
                //    dbmodel.Photo = pvmodel.Photo;
                //}
                //dbmodel.UpdateDate = DateTime.Now;
                return PrizeOrderContract.Update(dbmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        ///     删除奖品
        /// </summary>
        /// <param name="member">奖品信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(Guid guid)
        {
            try
            {
                PrizeOrder pmodel = PrizeOrderContract.PrizeOrders.SingleOrDefault(m => m.Id.Equals(guid));
                if (pmodel == null)
                {
                    return new OperationResult(OperationResultType.Error, string.Format("不存在Id为{0}的奖单", guid));
                }

                return PrizeOrderContract.Delete(pmodel);
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }


        /// <summary>
        ///     获取Top奖单
        /// </summary>
        /// <returns>奖单信息结果集</returns>
        public OperationResult GetTopPrizeOrders(int topCount, int? rtype)
        {
            OperationResult result = PrizeOrderContract.GetTopPrizeOrders(topCount, rtype);
            return result;
        }


        /// <summary>
        ///  取奖单
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <param name="revealtype">开奖类型</param>
        /// <param name="revealstate">奖单状态</param>
        /// <returns></returns>
        public OperationResult GetLotteries(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, int revealtype = 0, int revealstate = 0)
        {
            OperationResult result = PrizeOrderContract.GetLotteries(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount, revealtype, revealstate);
            return result;
        }

        /// <summary>
        ///  取奖单
        /// </summary>
        /// <param name="pageSize">每页输出的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="whereString">条件字符串</param>
        /// <param name="orderbyString">排序字符串</param>
        /// <param name="totalCount">返回总记录</param>
        /// <param name="totalPageCount">返回总页数</param>
        /// <param name="revealtype">开奖类型</param>
        /// <param name="revealstate">奖单状态</param>
        /// <returns></returns>
        public OperationResult GetRevealedSceneLotteries(int pageSize, int pageIndex, string whereString, string orderbyString, out int totalCount, out int totalPageCount, int revealstate = 0) {
            OperationResult result = PrizeOrderContract.GetRevealedSceneLotteries(pageSize, pageIndex, whereString, orderbyString, out totalCount, out totalPageCount, revealstate);
            return result;
        }
        
        /// <summary>
        ///  置顶
        /// </summary>
        /// <param name="poid">奖单ID</param>
        /// <param name="moneyvalue">用户缴费金额</param>
        /// <param name="datelong">置顶时长</param>
        /// <param name="operatorid">操作者Id</param>
        public OperationResult Set2Top(Guid poid, decimal moneyvalue, int datelong, long operatorid)
        {
            OperationResult result = PrizeOrderContract.Set2Top(poid, moneyvalue, datelong, operatorid);
            return result;
        }

        public OperationResult GetPrizeAsking(Guid poid)
        {
            OperationResult result = PrizeOrderContract.GetPrizeOrderById(poid);
            if (result.ResultType == OperationResultType.Success)
            {
                PrizeOrder pomodel = (PrizeOrder)result.AppendData;
                result.AppendData = new PrizeOrderView()
                {
                    Question = pomodel.Extend.PrizeAsking.Question,
                    AnswerOptions = pomodel.Extend.PrizeAsking.AnswerOptions
                };
            }
            return result;
        }

        public OperationResult GetPrizeOrderDetail(Guid poid)
        {
            OperationResult result = PrizeOrderContract.GetPrizeOrderDetail(poid);
            if (result.ResultType == OperationResultType.Success)
            {
                PrizeOrder pomodel = (PrizeOrder)result.AppendData;
                result.AppendData = new PrizeOrderDetailView()
                {
                    PrizeView = new PrizeView()
                    {
                        Id = pomodel.Prize.Id,
                        Name = pomodel.Prize.Name,
                        Description = pomodel.Prize.Description,
                        OriginalPhoto = pomodel.Prize.PrizePhotos.FirstOrDefault().ToSiteViewModel()
                    },
                    PrizeOrderView = new PrizeOrderView()
                    {
                        PrizeId = pomodel.Prize.Id,
                        Id = pomodel.Id,
                        RevealType = pomodel.RevealType,
                        RevealState = pomodel.RevealState,
                        LaunchTime = pomodel.Extend.LaunchTime,
                        PoolCount = pomodel.Extend.PoolCount,
                        Question = pomodel.Extend.PrizeAsking.Question,
                        Answer = pomodel.Extend.PrizeAsking.Answer,
                        AnswerOptions = pomodel.Extend.PrizeAsking.AnswerOptions,
                        AnswerRevealConditionType = pomodel.Extend.AnswerRevealConditionType,
                        ScopeType = pomodel.Extend.ScopeType,
                        ScopeAreaCity = pomodel.Extend.ScopeCity,
                        AddDate = pomodel.AddDate
                    },
                    MemberView = new MemberView()
                    {
                        Id = pomodel.Prize.Member.Id,
                        Name = pomodel.Prize.Member.Name,
                        UserName = pomodel.Prize.Member.UserName,
                        Tel = pomodel.Prize.Member.Extend.Tel,
                        AdvertisingUrl = pomodel.Prize.Member.Extend.AdvertisingUrl,
                        Province = pomodel.Prize.Member.Extend.Address.Province,
                        City = pomodel.Prize.Member.Extend.Address.City,
                        Town = pomodel.Prize.Member.Extend.Address.Town,
                        AddrSuffix = pomodel.Prize.Member.Extend.Address.Suffix
                    }
                };
            }
            return result;
        }

        /// <summary>
        ///  同时发布奖品、发起抽奖
        /// </summary>
        /// <param name="shouldMinus">是否该对用户的可发起抽奖次数减</param>
        public OperationResult BatchAdd(PrizeOrderDetailView porderdetail, bool shouldMinus = false)
        {
            int? sortorder = PrizeOrderContract.PrizeOrders.Max(po => po.SortOrder);
            PrizeOrder porder = new PrizeOrder()
            {
                Prize = new Prize()
                {
                    Name = porderdetail.PrizeView.Name,
                    Description = porderdetail.PrizeView.Description,
                    Member = new Member() { Id = porderdetail.MemberView.Id },
                    PrizePhotos = new PrizePhoto[] { 
                        new PrizePhoto() { 
                            Name = porderdetail.PrizeView.OriginalPhoto.Name, 
                            PhotoTypeNum = porderdetail.PrizeView.OriginalPhoto.PhotoTypeNum 
                        } 
                    }
                },
                RevealType = porderdetail.PrizeOrderView.RevealType,
                RevealState = Component.Tools.RevealState.UnDrawn,
                SortOrder = (sortorder ?? 0) + 1,
                Extend = new PrizeOrderExtend()
                {
                    LuckyCount = porderdetail.PrizeOrderView.LuckyCount, //中奖人数
                    MinLuckyCount = 1, //最低中奖人数默认设置为1
                }
            };

            // 定员、定员、答案三种模式需要设置抽奖城市
            if (RevealType.Scene != porderdetail.PrizeOrderView.RevealType)
            {
                porder.Extend.ScopeType = porderdetail.PrizeOrderView.ScopeType;
                if (porderdetail.PrizeOrderView.ScopeType == ScopeType.AreaCity)
                {
                    porder.Extend.ScopeCity = porderdetail.PrizeOrderView.ScopeAreaCity;
                }

                porder.Extend.Freight = porderdetail.PrizeOrderView.Freight;
            }
            switch (porderdetail.PrizeOrderView.RevealType)
            {
                case RevealType.Timing:
                    porder.Extend.LaunchTime = porderdetail.PrizeOrderView.LaunchTime; //开奖时间
                    break;
                case RevealType.Quota:
                    porder.Extend.PoolCount = porderdetail.PrizeOrderView.PoolCount; //总人数
                    break;
                case RevealType.Answer:
                    porder.Extend.PrizeAsking.Question = porderdetail.PrizeOrderView.Question;
                    porder.Extend.PrizeAsking.Answer = porderdetail.PrizeOrderView.Answer;
                    porder.Extend.PrizeAsking.AnswerOptions = porderdetail.PrizeOrderView.AnswerOptions;
                    // 开奖条件
                    porder.Extend.AnswerRevealConditionType = porderdetail.PrizeOrderView.AnswerRevealConditionType;
                    switch (porder.Extend.AnswerRevealConditionType)
                    {
                        case AnswerRevealConditionType.Quota:
                            porder.Extend.PoolCount = porderdetail.PrizeOrderView.PoolCount; //总人数
                            break;
                        case AnswerRevealConditionType.Timing:
                            porder.Extend.LaunchTime = porderdetail.PrizeOrderView.LaunchTime; //开奖时间
                            break;
                    }
                    break;
                case RevealType.Scene:
                    porder.Extend.LaunchTime = porderdetail.PrizeOrderView.LaunchTime; //开奖时间
                    break;
            }

            // 现场抽奖
            if (RevealType.Scene == porderdetail.PrizeOrderView.RevealType)
            {
                string staffsString = porderdetail.PrizeOrderView.StaffsOfScenceString;
                List<SceneStaff> staffs = new List<SceneStaff>();
                if (!string.IsNullOrEmpty(staffsString))
                {
                    string[] staffarray = staffsString.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in staffarray)
                    {
                        staffs.Add(new SceneStaff()
                        {
                            Value = item,
                            IsLucky = false,
                            LuckySceneStaffStateNum = LuckySceneStaffState.Default.ToInt()
                        });
                    }
                }

                porder.SceneStaffs = staffs;
            }

            return PrizeOrderContract.BatchAdd(porder, shouldMinus);
        }
    }
}