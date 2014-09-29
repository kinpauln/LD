using LotteryDraw.Component.Tools;
using LotteryDraw.Core;
using LotteryDraw.Site.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using LotteryDraw.Core.Models.Business;
using System.IO;
using LotteryDraw.Site.Extentions;
using LotteryDraw.Component.Utility;

namespace LotteryDraw.Site.Web.Areas.Website.Controllers
{
    [Export]
    public class VipController : WebsiteControllerBase
    {
        #region 属性
            #region 受保护的属性
        [Import]
        protected IPrizeSiteContract PrizeSiteContract { get; set; }

        [Import]
        protected IPrizeOrderSiteContract PrizeOrderSiteContract { get; set; }

        [Import]
        protected IPrizeContract PrizeContract { get; set; }  
        #endregion
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        #region 发布奖品
        /// <summary>
        ///  发布奖品
        /// </summary>
        public ActionResult PublishPrize()
        {
            PrizeView model = new PrizeView
            {
                MemberId = this.UserId ?? 0
            };
            return View(model);
        }

        /// <summary>
        ///  发布奖品
        /// </summary>
        [HttpPost]
        public ActionResult PublishPrize(PrizeView model)
        {
            ViewBag.IsPostBack = true;

            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                ViewBag.Message = "请选择文件";
                return View(model);
            }
            Stream photoStrem = Request.Files[0].InputStream;
            model.Photo = StreamUtil.StreamToBytes(photoStrem);

            if (model.MemberId == 0)
            {
                ViewBag.Message = "用户Id为0";
                return View(model);
            }
            OperationResult result = PrizeSiteContract.Add(model);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                TempData["Message"] = "奖品发布成功。<br /><a href='/Vip/PublishPrize'>继续发布<a><br /><a href='/Vip/ManagePrizes'>奖品管理<a>";
                return RedirectToAction("InfoPage");
            }
            //ModelState.AddModelError("", msg);
            ViewBag.Message = msg;
            return View(model);
        } 
        #endregion

        #region 发起抽奖
        /// <summary>
        ///  发起抽奖
        /// </summary>
        public ActionResult LaunchPrize(Guid id)
        {
            PrizeOrderView model = new PrizeOrderView()
            {
                PrizeId = id
            };
            ViewBag.RevealTypeList = model.RevealType.ToDescriptionSelectList();  
            return View(model);
        }

        /// <summary>
        ///  发起抽奖
        /// </summary>
        [HttpPost]
        public ActionResult LaunchPrize(PrizeOrderView model)
        {
            OperationResult result = PrizeOrderSiteContract.Add(model);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                TempData["Message"] = "发起抽奖成功。<br /><a href='/Vip/PrizeOrderDetail'>查看<a>奖单";
                return RedirectToAction("InfoPage");
            }
            //ModelState.AddModelError("", msg);
            ViewBag.Message = msg;
            return View(model);
        } 
        #endregion

        #region 编辑奖品
        public ActionResult PrizeEdit(Guid id)
        {
            PrizeView model = PrizeContract.Prizes
                .Where(p => p.Id.Equals(id))
                .Select(p => new PrizeView()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Photo = p.Photo
                }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult PrizeEdit(PrizeView model)
        {
            ViewBag.IsPostBack = true;
            bool savePhoto = false;

            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                Stream photoStrem = Request.Files[0].InputStream;
                byte[] fileBytes = StreamUtil.StreamToBytes(photoStrem);

                savePhoto = true;
                model.Photo = StreamUtil.StreamToBytes(photoStrem);
            }

            OperationResult result = PrizeSiteContract.Update(model, savePhoto);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                TempData["Message"] = "奖品修改成功。<br /><a href='/Vip/ManagePrizes'>返回</a>奖品管理页";
                return RedirectToAction("InfoPage");
            }
            ViewBag.Message = msg;
            return View(model);
        }
        #endregion

        #region override

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ViewBag.LeftTitleContent = "管理面板";
            ViewBag.OptionName = "会员选项";
            ViewBag.MetHitsVisible = false;
            ViewBag.MemberId = this.UserId ?? 0;
        }

        public override int PageSize
        {
            get
            {
                return 10;
            }
        } 
        #endregion

        /// <summary>
        ///  奖品管理
        /// </summary>
        public ActionResult ManagePrizes(int? id)
        {
            int userid = this.UserId ?? 0;

            int pageIndex = id ?? 1;
            int total;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };

            var rlist = PrizeContract.Prizes
                .Where(p => p.Member.Id.Equals(userid))
                .Where<Prize, Guid>(m => true, pageIndex, this.PageSize, out total, sortConditions)
                .OrderByDescending(p => p.AddDate)
                .Select(p => new PrizeView()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                AddDate = p.AddDate,
                UpdateDate = p.UpdateDate,
                Photo = p.Photo
            });

            PagedList<PrizeView> model = new PagedList<PrizeView>(rlist, pageIndex, this.PageSize, total);
            return View(model);
        }

        /// <summary>
        ///  删除奖品
        /// </summary>
        /// <param name="id"></param>
        public ActionResult PrizeDelete(Guid id)
        {
            OperationResult result = PrizeSiteContract.Delete(id);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                TempData["Message"] = "奖品删除成功。<br /><a href='/Vip/ManagePrizes'>返回</a>奖品管理页";
                return RedirectToAction("InfoPage");
            }
            TempData["Message"] = msg;
            return RedirectToAction("ManagePrizes");
        }

        /// <summary>
        ///  奖品详情
        /// </summary>
        public ActionResult PrizeDetail(Guid id)
        {
            PrizeView model = PrizeContract.Prizes
                .Where(p => p.Id.Equals(id))
                .Select(p => new PrizeView()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    AddDate = p.AddDate,
                    UpdateDate = p.UpdateDate,
                    Photo = p.Photo
                }).FirstOrDefault();
            return View(model);
        }

        public FileResult GetImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;
            //return the image to View
            return new FileContentResult(StreamUtil.Base64ToBytes(base64String), "image/jpeg");
        }
    }
}
