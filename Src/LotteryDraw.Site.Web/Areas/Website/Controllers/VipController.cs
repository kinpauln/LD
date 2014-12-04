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
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Site.Extentions;

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
        protected IPrizeOrderContract PrizeOrderContract { get; set; }

        [Import]
        protected IPrizeContract PrizeContract { get; set; }

        [Import]
        protected IPrizePhotoSiteContract PrizePhotoSiteContract { get; set; }

        [Import]
        public IAccountContract AccountContract { get; set; }

        [Import]
        protected IPrizeBettingSiteContract PrizeBettingSiteContract { get; set; }

        #endregion
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrizeBetting(Guid poId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                PrizeBettingView model = new PrizeBettingView() { PrizeOrderId = poId };
                int rtvalue = int.Parse(Request.QueryString["RevealType"]);
                ViewBag.RevealType = rtvalue;
                if (RevealType.Answer.ToInt() == rtvalue)
                {
                    InitAnswerOptions(model);
                }
                long userid = this.UserId ?? 0;
                model.UserId = userid;
                OperationResult result = AccountContract.GetMember(userid);
                if (result.ResultType == OperationResultType.Success)
                {
                    Member member = (Member)result.AppendData;
                    if (member.Extend != null)
                    {
                        model.Phone = member.Extend.Tel;
                        model.Address = member.Extend.Address.ToString();
                    }
                }
                return View(model);
            }
        }

        [HttpPost]
        [ValidateMvcCaptcha]
        public ActionResult PrizeBetting(PrizeBettingView model)
        {
            ViewBag.IsPostBack = true;
            int rtype = int.Parse(Request.Form["RevealType"]);
            ViewBag.RevealType = rtype;

            if (ModelState.IsValid)
            {
                //验证码验证通过
            }
            else
            {
                //验证码验证失败
                //ModelState.AddModelError("", e.Message);
                ViewBag.Message = "验证码输入不正确";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Phone.Trim()))
            {
                ViewBag.Message = "领奖电话不能为空";
                return View(model);
            }
            if (!LotteryDraw.Component.Utility.RegExp.IsMobileNo(model.Phone.Trim()))
            {
                ViewBag.Message = "手机号码不合法";
                return View(model);
            }
            if (string.IsNullOrEmpty(model.Address.Trim()))
            {
                ViewBag.Message = "奖品邮寄地址不能为空";
                return View(model);
            }
            // 答案开奖的话需要验证是否选择了答案
            if ((int)RevealType.Answer == rtype)
            {
                if (string.IsNullOrEmpty(model.UserAnswer))
                {
                    ViewBag.Message = "答案必须选择";
                    return View(model);
                }
            }

            OperationResult result = PrizeBettingSiteContract.Add(model);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                TempData["Message"] = "参与抽奖成功。";//<br /><a href='#'>奖池状况<a>";
                return RedirectToAction("InfoPage");
            }
            //ModelState.AddModelError("", msg);
            ViewBag.Message = msg;
            return View(model);
        }

        private void InitAnswerOptions(PrizeBettingView model)
        {
            OperationResult innerresult = PrizeOrderSiteContract.GetPrizeAsking(model.PrizeOrderId.Value);
            if (innerresult != null)
            {
                PrizeOrderView pov = (PrizeOrderView)innerresult.AppendData;
                if (pov != null)
                {
                    //ViewBag.Question = pov.Question;
                    model.Question = pov.Question;
                    model.AnswerOptionsString = pov.AnswerOptions;
                }
            }
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
        [ValidateMvcCaptcha]
        public ActionResult PublishPrize(PrizeView model)
        {
            ViewBag.IsPostBack = true;

            if (ModelState.IsValid)
            {
                //验证码验证通过
            }
            else
            {
                //验证码验证失败
                //ModelState.AddModelError("", e.Message);
                ViewBag.Message = "验证码输入不正确";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Name.Trim()))
            {
                ViewBag.Message = "奖品名称不能为空";
                return View(model);
            }

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

        /// <summary>
        ///  发布奖品
        /// </summary>
        [HttpPost]
        [ValidateMvcCaptcha]
        public JsonResult PublishPrizeAjax(PrizeView model)
        {
            if (ModelState.IsValid)
            {
                //验证码验证通过
            }
            else
            {
                //验证码验证失败
                //ModelState.AddModelError("", e.Message);
                ViewBag.Message = "验证码输入不正确";
                return Json(new { OK = false, Message = "验证码输入不正确" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Name.Trim()))
            {
                ViewBag.Message = "奖品名称不能为空";
                return Json(new { OK = false, Message = "奖品名称不能为空" }, JsonRequestBehavior.AllowGet);
            }

            if (model.MemberId == 0)
            {
                ViewBag.Message = "用户Id为0";
                return Json(new { OK = false, Message = "用户Id为0" }, JsonRequestBehavior.AllowGet);
            }
            OperationResult result = PrizeSiteContract.Add(model);
            if (result.ResultType == OperationResultType.Success)
            {
                Prize rtnmodel = (Prize)result.AppendData;
                string pid = string.Empty;
                if (rtnmodel != null)
                {
                    pid = rtnmodel.Id.ToString();
                }
                return Json(new { OK = true, Message = "奖品发布成功！", PrizeId = pid }, JsonRequestBehavior.AllowGet);
            }

            string msg = result.Message ?? result.ResultType.ToDescription();
            return Json(new { OK = false, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  发布奖品
        /// </summary>
        [HttpPost]
        public JsonResult SavePhotoAjax()
        {
            Response.ContentType = "text/json";
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                return Json(new { OK = false, Message = "请选择文件" }, JsonRequestBehavior.AllowGet);
            }

            long userid = this.UserId ?? 0;
            string pid = Request.Form["PrizeId"] as string;
            if (string.IsNullOrEmpty(pid))
            {
                return Json(new { OK = false, Message = "抱歉，奖品Id为空，不能保存关联图片" }, JsonRequestBehavior.AllowGet);
            }

            //存入文件
            try
            {
                HttpPostedFileBase file = Request.Files["PrizePhoto"];
                string filename = pid + System.IO.Path.GetExtension(file.FileName);
                string filepath = System.IO.Path.Combine(Server.MapPath("~/Files/PrizePhotos"), userid.ToString());
                //如果不存在就创建用户文件夹
                if (Directory.Exists(filepath) == false)
                {
                    Directory.CreateDirectory(filepath);
                }

                string fullname = System.IO.Path.Combine(filepath, filename);

                if (System.IO.File.Exists(fullname))
                {
                    System.IO.File.Delete(fullname);
                }

                file.SaveAs(fullname);

                //存DB
                OperationResult result = PrizePhotoSiteContract.Add(new PrizePhotoView()
                {
                    Name = filename,
                    PhotoTypeNum = PhotoType.Original.ToInt(),
                    PrizeId = new Guid(pid)
                });
                if (result.ResultType == OperationResultType.Success)
                {
                    return Json(new { OK = true, Message = "图片上传成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { OK = true, Message = "物理文件保存成功，但存数据库信息失败【" + result.Message + "】" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { OK = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 奖单
        /// <summary>
        ///  奖单详情
        /// </summary>
        public ActionResult PrizeOrderDetail(Guid id)
        {
            PrizeOrderView model = PrizeOrderContract.PrizeOrders
                .Where(p => p.Id.Equals(id))
                .Select(p => new PrizeOrderView()
                {
                    Id = p.Id,
                    RevealTypeNum = p.RevealTypeNum,
                    RevealStateNum = p.RevealStateNum,
                    LaunchTime = p.Extend.LaunchTime,
                    MinLuckyCount = p.Extend.MinLuckyCount,
                    LuckyCount = p.Extend.LuckyCount,
                    LuckyPercent = p.Extend.LuckyPercent,
                    PoolCount = p.Extend.PoolCount,
                    Remarks = p.Extend.Remarks,
                    AddDate = p.AddDate
                }).FirstOrDefault();
            if (model == null)
                ViewBag.Message = string.Format("不存在Id为{0}的奖单", id);
            return View(model);
        }

        /// <summary>
        ///  奖单列表
        /// </summary>
        public ActionResult PrizeOrderList(Guid? pid, int? id)
        {
            long userid = this.UserId ?? 0;
            int pageIndex = id ?? 1;
            int total;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };

            var query = PrizeOrderContract.PrizeOrders.Where(p => !p.IsDeleted && p.Prize.Member.Id == userid);
            if (pid != null)
            {
                query = query.Where(p => p.Prize.Id == pid);
            }
            var rlist = query.Where<PrizeOrder, Guid>(m => true, pageIndex, this.PageSize, out total, sortConditions)
            .OrderByDescending(p => p.AddDate)
            .Select(p => new PrizeOrderView()
            {
                Id = p.Id,
                RevealTypeNum = p.RevealTypeNum,
                RevealState = p.RevealState,
                LaunchTime = p.Extend.LaunchTime,
                MinLuckyCount = p.Extend.MinLuckyCount,
                LuckyCount = p.Extend.LuckyCount,
                LuckyPercent = p.Extend.LuckyPercent,
                PoolCount = p.Extend.PoolCount,
                Remarks = p.Extend.Remarks,
                AddDate = p.AddDate
            });

            PagedList<PrizeOrderView> model = new PagedList<PrizeOrderView>(rlist, pageIndex, this.PageSize, total);

            if (model == null)
                ViewBag.Message = string.Format("不存在Id为{0}的奖单", id);
            return View(model);
        }

        /// <summary>
        ///  删除奖单
        /// </summary>
        /// <param name="id"></param>
        public ActionResult PrizeOrderDelete(Guid id)
        {
            OperationResult result = PrizeOrderSiteContract.Delete(id);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                TempData["Message"] = "奖单删除成功。<br /><a href='/Vip/PrizeOrderList'>返回</a>奖单管理页";
                return RedirectToAction("InfoPage");
            }
            TempData["Message"] = msg;
            return RedirectToAction("PrizeOrderList");
        }
        #endregion

        public ActionResult MyBettingList(int? id)
        {
            long userid = this.UserId ?? 0;
            int pageIndex = id ?? 1;
            int total;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };

            return View();
        }

        #region 发起抽奖
        /// <summary>
        ///  发起抽奖
        /// </summary>
        public ActionResult LaunchPrize(Guid id)
        {
            PrizeOrderView model = new PrizeOrderView()
            {
                PrizeId = id,
                MinLuckyCount = 1
            };
            ViewBag.RevealTypeList = model.RevealType.ToDescriptionSelectList();
            return View(model);
        }

        /// <summary>
        ///  发起抽奖
        /// </summary>
        [HttpPost]
        [ValidateMvcCaptcha]
        public ActionResult LaunchPrize(PrizeOrderView model)
        {
            ViewBag.IsPostBack = true;
            int rtype = int.Parse(Request.Form["RevealType"]);
            ViewBag.RevealType = rtype;

            if (this.PubishingEnableTimes == 0)
            {
                ViewBag.Message = "您还可发起抽奖0次，请续费后再执行此功能。";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                //验证码验证通过
            }
            else
            {
                //验证码验证失败
                //ModelState.AddModelError("", e.Message);
                ViewBag.Message = "验证码输入不正确";
                return View(model);
            }

            bool shouldMinus = this.PubishingEnableTimes < 1000000 ? true : false;
            OperationResult result = PrizeOrderSiteContract.Add(model, shouldMinus);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                if (shouldMinus) {
                    if(!this.UpdatePubishingEnableTimes()){
                        //记日志（更新可发布奖品次数失败）
                    }
                }
                TempData["Message"] = string.Format("发起抽奖成功。<br /><a href='/Vip/PrizeOrderDetail/{0}'>查看<a>奖单", model.PrizeId);
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
        #endregion

        /// <summary>
        ///  奖品管理
        /// </summary>
        public ActionResult ManagePrizes(int? id)
        {
            long userid = this.UserId ?? 0;
            ViewBag.UserId = userid.ToString();

            int pageIndex = id ?? 1;
            int total;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };

            var plist = PrizeContract.Prizes
                .Where(p => p.Member.Id.Equals(userid))
                .Where<Prize, Guid>(m => true, pageIndex, this.PageSize, out total, sortConditions)
                .OrderByDescending(p => p.AddDate)
                .ToList();
            var rlist = new List<PrizeView>();
            plist.ForEach(p =>
            {
                rlist.Add(new PrizeView()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    AddDate = p.AddDate,
                    UpdateDate = p.UpdateDate,
                    Photos = p.PrizePhotos.ToList().ToSiteViewModels()
                });
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
            ViewBag.UserId = this.UserId ?? 0;
            // 可发起抽奖的次数
            ViewBag.PublishEnableTimes = this.PubishingEnableTimes;
            Prize pmodel = PrizeContract.Prizes
                .Where(p => p.Id.Equals(id))
                .FirstOrDefault();
            PrizeView model = pmodel.ToSiteViewModel();
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
