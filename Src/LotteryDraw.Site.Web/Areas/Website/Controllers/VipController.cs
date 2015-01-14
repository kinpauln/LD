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
using System.Text;

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

        [Import]
        protected ILotteryResultContract LotteryResultContract { get; set; }

        [Import]
        protected ILotteryResultSiteContract LotteryResultSiteContract { get; set; }

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
                //int rtvalue = int.Parse(Request.QueryString["RevealType"]);
                //ViewBag.RevealType = rtvalue;
                ViewBag.CurrentUserId = this.UserId ?? 0;
                OperationResult result = PrizeOrderSiteContract.GetPrizeOrderDetail(poId);
                if (result.ResultType == OperationResultType.Success)
                {
                    PrizeBettingView model = new PrizeBettingView()
                    {
                        PrizeOrderDetailView = (PrizeOrderDetailView)result.AppendData,
                        UserId = this.UserId ?? 0
                    };

                    ViewBag.RevealType = model.PrizeOrderDetailView != null ? model.PrizeOrderDetailView.PrizeOrderView.RevealType.ToInt().ToString() : string.Empty;

                    //if (RevealType.Answer.ToInt() == rtvalue)
                    //{
                    //    //InitAnswerOptions(model);
                    //}
                    return View(model);
                }
                else
                {
                    return View(new PrizeBettingView());
                }
            }
        }

        [HttpPost]
        [ValidateMvcCaptcha]
        public ActionResult PrizeBetting(PrizeBettingView model)
        {
            ViewBag.IsPostBack = true;
            int rtype = model.PrizeOrderDetailView.PrizeOrderView.RevealType.ToInt();
            ViewBag.RevealType = rtype.ToString();
            ViewBag.CurrentUserId = this.UserId ?? 0;

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

            if ((this.UserId ?? 0) == 0)
            {
                //验证码验证失败
                //ModelState.AddModelError("", e.Message);
                ViewBag.Message = "当前登录的用户Id莫名的为空，无法发布奖品，请尝试退出并重新登录。";
                return View(model);
            }
            else
            {
                model.UserId = this.UserId;
            }

            if (model.UserId.Value == model.PrizeOrderDetailView.MemberView.Id)
            {
                //验证码验证失败
                //ModelState.AddModelError("", e.Message);
                ViewBag.Message = "该奖品是您发起的抽奖，您不能参与自己发起的抽奖。";
                return View(model);
            }

            //if (string.IsNullOrEmpty(model.PrizeOrderDetailView.MemberView.Tel))
            //{
            //    ViewBag.Message = "领奖电话不能为空";
            //    return View(model);
            //}
            //if (!LotteryDraw.Component.Utility.RegExp.IsMobileNo(model.PrizeOrderDetailView.MemberView.Te))
            //{
            //    ViewBag.Message = "手机号码不合法";
            //    return View(model);
            //}
            //if (string.IsNullOrEmpty(model.PrizeOrderDetailView.MemberView.Address))
            //{
            //    ViewBag.Message = "奖品邮寄地址不能为空";
            //    return View(model);
            //}
            // 竞猜开奖的话需要验证是否选择了答案
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
                ViewBag.PostBackIsOK = true;
                ViewBag.SuccessString = "参与抽奖成功";
                return View(model);
                //TempData["Message"] = "参与抽奖成功。";//<br /><a href='#'>奖池状况<a>";
                //return RedirectToAction("InfoPage");
            }
            //ModelState.AddModelError("", msg);
            ViewBag.Message = msg;
            return View(model);
        }

        //private void InitAnswerOptions(PrizeBettingView model)
        //{
        //    OperationResult innerresult = PrizeOrderSiteContract.GetPrizeAsking(model.PrizeOrderDetailView.PrizeOrderView.Id);
        //    if (innerresult != null)
        //    {
        //        PrizeOrderView pov = (PrizeOrderView)innerresult.AppendData;
        //        if (pov != null)
        //        {
        //            //ViewBag.Question = pov.Question;
        //            model.Question = pov.Question;
        //            model.AnswerOptionsString = pov.AnswerOptions;
        //        }
        //    }
        //}

        #region 发布抽奖
        public ActionResult LaunchLottery()
        {
            PrizeOrderDetailView model = new PrizeOrderDetailView();

            return View(model);
        }

        [HttpPost]
        [ValidateMvcCaptcha]
        public ActionResult LaunchLottery(PrizeOrderDetailView model)
        {
            ViewBag.IsPostBack = true;

            ViewBag.RevealType = model.PrizeOrderView.RevealType.ToInt().ToString();
            ViewBag.ScopeType = model.PrizeOrderView.ScopeType.ToInt().ToString();
            ViewBag.ScopeProvince = model.PrizeOrderView.ScopeProvince;
            ViewBag.ScopeAreaCity = model.PrizeOrderView.ScopeAreaCity;

            //if (this.PubishingEnableTimes == 0)
            //{
            //    //验证码验证通过
            //    ViewBag.Message = "抱歉，您可发起抽奖的次数为0，请续费后再继续使用该功能。";
            //    return View(model);
            //}

            // 竞猜开奖并且开奖方式为“手动”，则取消对“开奖条件”的验证
            if (model.PrizeOrderView.RevealType == RevealType.Answer
                && model.PrizeOrderView.RevealTypeOfAnswer == RevealTypeOfAnswer.Manual)
            {
                ModelState.Remove("PrizeOrderView.AnswerRevealConditionType");
            }

            // 非竞猜开奖，则取消对“揭晓答案（先知、后知）方式”的验证
            if (model.PrizeOrderView.RevealType != RevealType.Answer)
            {
                ModelState.Remove("PrizeOrderView.RevealTypeOfAnswer");
                ModelState.Remove("PrizeOrderView.AnswerRevealConditionType");
            }

            // 现场开奖，则取消对“是否需要兑奖码”的验证
            if (model.PrizeOrderView.RevealType == RevealType.Scene)
            {
                ModelState.Remove("PrizeOrderView.IsNeedExchangeCode");
                ModelState.Remove("PrizeOrderView.RevealTypeOfAnswer");
                ModelState.Remove("PrizeOrderView.AnswerRevealConditionType");
            }

            // 非现场开奖，则取消对“人员录入方式”的验证
            if (model.PrizeOrderView.RevealType != RevealType.Scene)
            {
                ModelState.Remove("PrizeOrderView.InputTypeOfStaff");
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
            if ((this.UserId ?? 0) == 0)
            {
                //验证码验证失败
                //ModelState.AddModelError("", e.Message);
                ViewBag.Message = "当前登录的用户Id莫名的为空，无法发布奖品，请尝试退出并重新登录。";
                return View(model);
            }
            else
            {
                model.MemberView.Id = this.UserId.Value;
            }

            if (!ValidatePrizeOrderDetailViewModel(model))
            {
                return View(model);
            }

            // 先上传图片
            string upmessage = string.Empty;
            string photoname = string.Empty;
            bool uploadResult = UploadPhoto(out upmessage, out photoname);
            if (!uploadResult)
            {
                ViewBag.Message = upmessage;
                return View(model);
            }

            // 保存数据到数据库
            model.PrizeView.OriginalPhoto = new PrizePhotoView() { Name = photoname, PhotoTypeNum = PhotoType.Original.ToInt() };

            bool shouldMinus = this.PubishingEnableTimes < 1000000 ? true : false;

            OperationResult result = PrizeOrderSiteContract.BatchAdd(model, shouldMinus);
            if (result.ResultType == OperationResultType.Success)
            {
                if (shouldMinus)
                {
                    if (!this.UpdatePubishingEnableTimes())
                    {
                        //记日志（更新可发布奖品次数失败）
                    }
                }
                PrizeOrder porder = (PrizeOrder)result.AppendData;
                //ViewBag.OkMessage = "发布抽奖成功";
                //TempData["Message"] = string.Format("发起抽奖成功。<br /><a href='/Vip/PrizeOrderDetail/{0}'>查看<a>奖单", porder.Id);
                //return RedirectToAction("InfoPage");
                ViewBag.PostBackIsOK = true;
                ViewBag.SuccessString = "发布抽奖成功，请等待审核。";
                return View(model);
            }
            else
            {
                // 删除图片
                bool delresult = DeletePhoto(photoname);
                ViewBag.Message = "发布抽奖失败" + (delresult ? string.Empty : ",已经上传的图片在删除过程中也失败，可能已产生图片冗余");
            }
            return View(model);
        }

        private bool ValidatePrizeOrderDetailViewModel(PrizeOrderDetailView model)
        {
            switch (model.PrizeOrderView.RevealType)
            {
                case RevealType.Timing:
                    if (model.PrizeOrderView.ScopeType == ScopeType.AreaCity && string.IsNullOrEmpty(model.PrizeOrderView.ScopeAreaCity))
                    {
                        ViewBag.Message = "抽奖城市必须指定";
                        return false;
                    }
                    if (!model.PrizeOrderView.LaunchTime.HasValue)
                    {
                        ViewBag.Message = "开奖时间必须指定";
                        return false;
                    }
                    break;
                case RevealType.Quota:
                    if (model.PrizeOrderView.ScopeType == ScopeType.AreaCity && string.IsNullOrEmpty(model.PrizeOrderView.ScopeAreaCity))
                    {
                        ViewBag.Message = "抽奖城市必须指定";
                        return false;
                    }
                    if (!model.PrizeOrderView.PoolCount.HasValue || model.PrizeOrderView.PoolCount.Value == 0)
                    {
                        ViewBag.Message = "总人数必须指定且大于0";
                        return false;
                    }
                    break;
                case RevealType.Answer:
                    if (model.PrizeOrderView.ScopeType == ScopeType.AreaCity && string.IsNullOrEmpty(model.PrizeOrderView.ScopeAreaCity))
                    {
                        ViewBag.Message = "抽奖城市必须指定";
                        return false;
                    }
                    if (string.IsNullOrEmpty(model.PrizeOrderView.Question))
                    {
                        ViewBag.Message = "必须命题";
                        return false;
                    }
                    if (string.IsNullOrEmpty(model.PrizeOrderView.AnswerOptions))
                    {
                        ViewBag.Message = "命题答案选项必须设置";
                        return false;
                    }

                    //if (model.PrizeOrderView.RevealTypeOfAnswer == RevealTypeOfAnswer.Auto)
                    //{
                    //    ViewBag.Message = "竞猜开奖的开奖方式必须选择";
                    //    return false;
                    //}
                    
                    // 竞猜开奖的开奖方式为“自动”
                    if (model.PrizeOrderView.RevealTypeOfAnswer == RevealTypeOfAnswer.Auto)
                    {
                        switch (model.PrizeOrderView.AnswerRevealConditionType)
                        {
                            case AnswerRevealConditionType.Timing:
                                if (!model.PrizeOrderView.LaunchTime.HasValue)
                                {
                                    ViewBag.Message = "开奖时间必须指定";
                                    return false;
                                }
                                break;
                            case AnswerRevealConditionType.Quota:
                                if (!model.PrizeOrderView.PoolCount.HasValue || model.PrizeOrderView.PoolCount.Value == 0)
                                {
                                    ViewBag.Message = "总人数必须指定且大于0";
                                    return false;
                                }
                                break;
                        }
                    }
                    break;
                case RevealType.Scene:
                    if (model.PrizeOrderView.ScopeType == ScopeType.AreaCity && string.IsNullOrEmpty(model.PrizeOrderView.ScopeAreaCity))
                    {
                        ViewBag.Message = "抽奖城市必须指定";
                        return false;
                    }
                    if (!model.PrizeOrderView.LaunchTime.HasValue)
                    {
                        ViewBag.Message = "开奖时间必须指定";
                        return false;
                    }

                    if (model.PrizeOrderView.InputTypeOfStaff == InputTypeOfStaff.Manual)
                    {
                        if (string.IsNullOrEmpty(model.PrizeOrderView.StaffsOfScenceString))
                        {
                            ViewBag.Message = "抽奖人员至少指定一位";
                            return false;
                        }

                        if (model.PrizeOrderView.StaffsOfScenceString.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries).Length == 0)
                        {
                            ViewBag.Message = "抽奖人员至少指定一位";
                            return false;
                        }
                    }

                    if (model.PrizeOrderView.InputTypeOfStaff == InputTypeOfStaff.File)
                    {
                        if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
                        {
                            ViewBag.Message = "请选择参与抽奖的人员文件";
                            return false;
                        }

                        HttpPostedFileBase file = Request.Files["StaffFile"];
                        if (file != null)
                        {
                            try
                            {
                                string staffString = string.Empty;
                                int counter = 0;
                                byte[] bytes = new byte[file.ContentLength];
                                //利用InputStream 属性直接从HttpPostedFile对象读取文本内容  
                                System.IO.Stream fileStream = file.InputStream;
                                using (StreamReader sreader = new StreamReader(fileStream, Encoding.UTF8))
                                {
                                    while (!sreader.EndOfStream)
                                    {
                                        counter++;
                                        staffString += sreader.ReadLine() + "|||";
                                    }
                                }
                                if (counter < model.PrizeOrderView.LuckyCount)
                                {
                                    ViewBag.Message = "抽奖人员的名单人数不能少于中奖人数";
                                    return false;
                                }

                                //fileStream.Read(bytes, 0, file.ContentLength);
                                model.PrizeOrderView.StaffsOfScenceString = staffString;
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Message = "读取参与抽奖人员的名单文件时出错了，请确认文件是否为【文本文件】，且内容合法。";
                                return false;
                            }
                        }
                        else
                        {
                            ViewBag.Message = "参与抽奖人员的名单文件未选择。";
                            return false;
                        }
                    }
                    break;
            }
            if (!model.PrizeOrderView.LuckyCount.HasValue || model.PrizeOrderView.LuckyCount.Value == 0)
            {
                ViewBag.Message = "中奖人数必须指定且大于0";
                return false;
            }
            return true;
        }

        #endregion

        #region 中奖通知

        public ActionResult LuckyNotice(int? id)
        {
            long userid = this.UserId ?? 0;
            ViewBag.UserId = userid.ToString();

            int pageIndex = id ?? 1;
            int total;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };

            var plist = LotteryResultContract.LotteryResults
                .Where(p => p.Member.Id.Equals(userid) && !p.IsDeleted)
                .Where<LotteryResult, Guid>(m => true, pageIndex, this.PageSize, out total, sortConditions)
                .OrderByDescending(p => p.AddDate)
                .ToList();
            var rlist = new List<LotteryResultView>();
            plist.ForEach(p =>
            {
                rlist.Add(new LotteryResultView()
                {
                    Id = p.Id,
                    LotteryResultState = p.LotteryResultState,
                    PrizeOrderView = p.PrizeOrder.ToSiteViewModel(),
                    MemberView = p.Member.ToSiteViewModel(),
                    AddDate = p.AddDate
                });
            });

            PagedList<LotteryResultView> model = new PagedList<LotteryResultView>(rlist, pageIndex, this.PageSize, total);
            return View(model);
        }

        #endregion

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

            if (string.IsNullOrEmpty(model.Name))
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

            if (string.IsNullOrEmpty(model.Name))
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
                RevealType = p.RevealType,
                RevealState = p.RevealState,
                LaunchTime = p.Extend.LaunchTime,
                MinLuckyCount = p.Extend.MinLuckyCount,
                LuckyCount = p.Extend.LuckyCount,
                LuckyPercent = p.Extend.LuckyPercent,
                PoolCount = p.Extend.PoolCount,
                Remarks = p.Extend.Remarks,
                AddDate = p.AddDate,
                Question = p.Extend.PrizeAsking.Question,
                AnswerOptions = p.Extend.PrizeAsking.AnswerOptions,
                RevealTypeOfAnswer = p.Extend.RevealTypeOfAnswer
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
                if (shouldMinus)
                {
                    if (!this.UpdatePubishingEnableTimes())
                    {
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
            ViewBag.LeftTitleContent = "会员中心";
            ViewBag.OptionName = "会员中心";
            ViewBag.MetHitsVisible = false;
            ViewBag.MemberId = this.UserId ?? 0;
        }
        #endregion

        #region 中奖者
        public ActionResult LuckyResults(Guid? poid, int? id)
        {
            long userid = this.UserId ?? 0;
            ViewBag.UserId = userid.ToString();

            int pageIndex = id ?? 1;
            int total;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };

            var plist = LotteryResultContract.LotteryResults
                .Where(p => p.PrizeOrder.Id.Equals(poid.Value) && !p.IsDeleted)
                .Where<LotteryResult, Guid>(m => true, pageIndex, this.PageSize, out total, sortConditions)
                .OrderBy(p => p.Member.UserName)
                .ToList();
            var rlist = new List<LotteryResultView>();
            plist.ForEach(p =>
            {
                rlist.Add(new LotteryResultView()
                {
                    Id = p.Id,
                    LotteryResultState = p.LotteryResultState,
                    PrizeOrderView = p.PrizeOrder.ToSiteViewModel(),
                    MemberView = p.Member.ToSiteViewModel(),
                    AddDate = p.AddDate
                });
            });

            if (plist.FirstOrDefault() != null)
            {
                ViewBag.PrizeOrderEntity = plist.FirstOrDefault().PrizeOrder.ToSiteViewModel();
            }
            PagedList<LotteryResultView> model = new PagedList<LotteryResultView>(rlist, pageIndex, this.PageSize, total);
            return View(model);
        } 
        #endregion

        [HttpPost]
        public JsonResult UpdateLotteryResult(Guid id, int state)
        {
            OperationResult result = LotteryResultSiteContract.UpdateLotteryResult(id, state);
            if (result.ResultType == OperationResultType.Success)
            {
                LotteryResult rtnmodel = (LotteryResult)result.AppendData;
                return Json(new { OK = true, Message = "更新状态成功！" }, JsonRequestBehavior.AllowGet);
            }

            string msg = result.Message ?? result.ResultType.ToDescription();
            return Json(new { OK = false, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RevealManualAnswerLottery(Guid poid, string answer)
        {
            OperationResult result = PrizeOrderSiteContract.RevealManualAnswerLottery(poid, answer);
            if (result.ResultType == OperationResultType.Success)
            {
                return Json(new { OK = true, Message = "开奖成功！" }, JsonRequestBehavior.AllowGet);
            }

            string msg = result.Message ?? result.ResultType.ToDescription();
            return Json(new { OK = false, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyBettingList(int? id)
        {
            long userid = this.UserId ?? 0;
            int pageIndex = id ?? 1;
            int total;
            PropertySortCondition[] sortConditions = new[] { new PropertySortCondition("Id") };

            return View();
        }

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
        ///  奖品详情
        /// </summary>
        public ActionResult PrizeDetail(Guid id)
        {
            ViewBag.UserId = this.UserId ?? 0;

            Prize pmodel = PrizeContract.Prizes
                .Where(p => p.Id.Equals(id))
                .FirstOrDefault();
            PrizeView model = pmodel.ToSiteViewModel();
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

        public FileResult GetImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;
            //return the image to View
            return new FileContentResult(StreamUtil.Base64ToBytes(base64String), "image/jpeg");
        }

        private bool UploadPhoto(out string message, out string filename)
        {
            message = string.Empty;
            filename = string.Empty;
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                message = "请选择文件";
                return false;
            }

            //存入文件
            try
            {
                HttpPostedFileBase file = Request.Files["PrizePhoto"];
                string randomString = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next(1, 9999).ToString();
                filename = randomString + System.IO.Path.GetExtension(file.FileName);
                string filepath = System.IO.Path.Combine(Server.MapPath("~/Files/PrizePhotos"), this.UserId.Value.ToString());
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

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        private bool DeletePhoto(string filename)
        {
            try
            {
                string filepath = System.IO.Path.Combine(Server.MapPath("~/Files/PrizePhotos"), this.UserId.Value.ToString());

                string fullname = System.IO.Path.Combine(filepath, filename);

                if (System.IO.File.Exists(fullname))
                {
                    System.IO.File.Delete(fullname);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
