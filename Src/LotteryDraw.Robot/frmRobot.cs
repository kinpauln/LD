﻿using LotteryDraw.Site;
using LotteryDraw.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LotteryDraw.Core.Data.Initialize;
using System.IO;
using System.Reflection;
using LotteryDraw.Core.Data.Repositories.Account;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Core.Models.Business;
using LotteryDraw.Component.Tools;
using System.Data.SqlClient;
using System.Threading;
using LotteryDraw.Component.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using LotteryDraw.Core.Data.Repositories.Business;
using Microsoft.Win32;

namespace RevealTest
{
    [Export]
    public partial class frmRobot : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Thread _revealLottery;
        private bool _revealWatchingStopped = true;
        private bool _fromTrayExit = false;             //是否允许退出

        private static CompositionContainer _container;

        [Import]
        public IPrizeOrderContract PrizeOrderContract { get; set; }

        [Import]
        public IPrizeContract PrizeContract { get; set; }

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected IPrizeRepository PrizeRepository { get; set; }

        /// <summary>
        /// 获取或设置 奖品数据访问对象
        /// </summary>
        [Import]
        protected IPrizePhotoRepository PrizePhotoRepository { get; set; }

        [Import]
        public IAccountContract AccountContract { get; set; }

        [Import]
        public IPrizeBettingContract PrizeBettingContract { get; set; }

        public frmRobot()
        {
            log.Info("程序启动");

            InitializeComponent();
            //初始化MEF组合容器
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory()));
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);

            DatabaseInitializer.Initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.btnOpenLottery.Enabled = true;
            this.btnStopReveal.Enabled = false;

            ni.Text = ProductName;

            SetAutoRun();

            btnOpenLottery_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var memberSet = AccountContract.Members.ToList();

            // 构造奖品
            List<Prize> prizelist = new List<Prize>();
            for (int i = 1; i < 30; i++)
            {
                Random rnd = new Random((int)DateTime.Now.Ticks + i);
                var prize = new Prize() { Name = "奖品名称奖品名称" + i.ToString(), Description = "奖品描述奖品描述奖品描述奖品描述奖品描述奖品描述奖品描述奖品描述奖品描述奖品描述" + i.ToString(), AddDate = DateTime.Now };
                var memberArray = memberSet.ToArray();
                var member = memberArray[rnd.Next(0, memberArray.Length)];
                prize.Member = member;
                //prize.Photo = StreamUtil.Base64ToBytes(StaticStrings.demoImageBase64String);
                prizelist.Add(prize);
            }
            PrizeRepository.Insert(prizelist);

            //DbSet<Prize> prizeSet = context.Set<Prize>();
            //prizeSet.AddOrUpdate(prizelist.ToArray());
            //context.SaveChanges();

            // 构造奖品
            List<PrizePhoto> photos = new List<PrizePhoto>();
            var prizeArray = prizelist.ToArray();
            for (int i = 0; i < prizelist.Count; i++)
            {
                Random rnd = new Random((int)DateTime.Now.Ticks + i);
                PrizePhoto pphoto = new PrizePhoto
                {
                    Name = "e26b4610-58fb-4ceb-ac72-a3f700c7c301.jpg",
                    Prize = prizeArray[rnd.Next(0, prizeArray.Length)]
                };

                photos.Add(pphoto);
            }
            PrizePhotoRepository.Insert(photos);
            //DbSet<PrizePhoto> pPhotoSet = context.Set<PrizePhoto>();
            //pPhotoSet.AddOrUpdate(photos.ToArray());
            //context.SaveChanges();

            var prizes = PrizeContract.Prizes.ToList();

            var prizeOrdersCount = PrizeOrderContract.PrizeOrders.Count();
            List<PrizeOrder> prizeOrders = new List<PrizeOrder>();
            bool poAdded = false;
            if (prizeOrdersCount == 0)
            {
                for (int i = 0; i < prizes.Count; i++)
                {
                    Random rnd = new Random((int)DateTime.Now.Ticks + i);
                    //var member = AccountContract.Members.Skip(rnd.Next(0, prizesCount)).Take(1);
                    var prize = prizes[i];
                    var prizeOrder = new PrizeOrder()
                    {
                        Prize = prize,
                        RevealTypeNum = rnd.Next(3) + 1,
                        //RevealStateNum = rnd.Next(4) + 1,
                        RevealStateNum = (int)RevealState.UnDrawn,
                        SortOrder = i + 1,
                        IsDeleted = false,
                        AddDate = DateTime.Now
                    };
                    switch (prizeOrder.RevealTypeNum)
                    {
                        case (int)RevealType.Timing:
                            prizeOrder.Extend = new PrizeOrderExtend()
                            {
                                LaunchTime = DateTime.Now.AddMinutes(10),
                                MinLuckyCount = 1,
                                LuckyCount = 5,
                                //,LuckyPercent = 2
                            };
                            break;
                        case (int)RevealType.Answer:
                            prizeOrder.Extend = new PrizeOrderExtend()
                            {
                                PrizeAsking = new PrizeAsking() { Question = "好声音冠军是谁？", Answer = "梁博" },
                                MinLuckyCount = 1,
                                LuckyCount = 5,
                                AnswerRevealConditionTypeNum = rnd.Next(2) + 1,
                                //,LuckyPercent = 2
                            };
                            switch (prizeOrder.Extend.AnswerRevealConditionTypeNum)
                            {
                                case (int)AnswerRevealConditionType.Timing:
                                    prizeOrder.Extend.LaunchTime = DateTime.Now.AddMinutes(10);
                                    break;
                                case (int)AnswerRevealConditionType.Quota:
                                    prizeOrder.Extend.PoolCount = 50;
                                    break;
                            }
                            break;
                        case (int)RevealType.Quota:
                            prizeOrder.Extend = new PrizeOrderExtend()
                            {
                                PoolCount = 10,
                                MinLuckyCount = 1,
                                LuckyCount = 5
                                //,LuckyPercent = 2
                            };
                            break;
                    }

                    prizeOrders.Add(prizeOrder);
                }
                poAdded = true;
            }
            OperationResult result = new OperationResult(OperationResultType.NoChanged);
            if (poAdded)
            {
                result = PrizeOrderContract.Add(prizeOrders);
            }
            if (result.ResultType == OperationResultType.Success || !poAdded)
            {
                if (PrizeBettingContract.PrizeBettings.Count() == 0)
                {
                    var members = AccountContract.Members.OrderBy(po => po.Id).Skip(2).ToList();
                    var porders = PrizeOrderContract.PrizeOrders.ToList();

                    List<PrizeBetting> prizeBettings = new List<PrizeBetting>();
                    for (int i = 0; i < porders.Count; i++)
                    {
                        var prizeorder = porders[i];
                        for (int j = 0; j < members.Count; j++)
                        {
                            var member = members[j];
                            var prizeBetting = new PrizeBetting()
                            {
                                PrizeOrder = prizeorder,
                                Member = member,
                                Address = "青岛市市南区江西路软件大厦",
                                Phone = "18829876354",
                                IsDeleted = false,
                                AddDate = DateTime.Now
                            };
                            prizeBettings.Add(prizeBetting);
                        }
                    }
                    PrizeBettingContract.Add(prizeBettings);
                }
                MessageBox.Show("构造奖单数据成功");
                btnCreateData.Enabled = false;
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void btnOpenLottery_Click(object sender, EventArgs e)
        {
            btnOpenLottery.Enabled = false;
            btnStopReveal.Enabled = true;
            _revealWatchingStopped = false;
            txtInfo.Text += "开奖监控正在启动..." + Environment.NewLine;
            log.Info("开奖监控正在启动...");

            _revealLottery = new Thread(new ThreadStart(RevealLottery));
            _revealLottery.IsBackground = true;
            _revealLottery.Start();
            txtInfo.Text += "开奖监控启动成功..." + Environment.NewLine + Environment.NewLine;
            log.Info("开奖监控启动成功...");
        }

        private void RevealLottery()
        {
            int interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ReadDBInteval"]);
            string splitLine = "****************************************************************************";
            string subSplitLine = "---------------------------------------------------------------";
            while (!_revealWatchingStopped)
            {
                this.Invoke(new Action(() =>
                {
                    txtInfo.Text += splitLine + Environment.NewLine;
                    txtInfo.Text += string.Format("尝试新一次的开奖...【{0}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + Environment.NewLine;
                    log.Info(string.Format("尝试新一次的开奖...【{0}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    string errorString = string.Empty;
                    OperationResult result = PrizeOrderContract.RevealLottery(interval, out errorString);
                    if (result.ResultType == OperationResultType.Success)
                    {
                        DataSet ds = (DataSet)result.AppendData;
                        if (!string.IsNullOrEmpty(errorString))
                        {
                            txtInfo.Text += errorString + Environment.NewLine;
                        }
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            #region 定时
                            txtInfo.Text += subSplitLine + Environment.NewLine;
                            Notice(ds.Tables[0], "定时开奖");
                            #endregion

                            #region 定员
                            txtInfo.Text += subSplitLine + Environment.NewLine;
                            Notice(ds.Tables[1], "定员开奖");
                            #endregion

                            #region 竞猜
                            txtInfo.Text += subSplitLine + Environment.NewLine;
                            Notice(ds.Tables[2], "竞猜开奖");
                            #endregion

                            #region 现场
                            txtInfo.Text += subSplitLine + Environment.NewLine;
                            Notice(ds.Tables[3], "现场开奖");
                            txtInfo.Text += subSplitLine + Environment.NewLine;
                            #endregion
                        }
                    }
                    else
                    {
                        log.Error("出错了，错误信息：" + result.Message);
                        txtInfo.Text += "出错了，错误信息：" + result.Message + Environment.NewLine;
                    }
                    txtInfo.Text += "本次开奖结束！" + Environment.NewLine;
                    log.Info("本次开奖结束！");
                    txtInfo.Text += splitLine + Environment.NewLine + Environment.NewLine;

                }));
                Thread.Sleep(interval * 1000);
            }
        }

        private void Notice(DataTable dt, string lotteryName)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                int revealCount = int.Parse(row["RevealCount"].ToString());
                if (revealCount > 0)
                {
                    //txtInfo.Text += string.Format("本次【{1}】共{0}次！", revealCount.ToString(), lotteryName) + Environment.NewLine;
                    log.Info(string.Format("本次【{1}】共{0}次！", revealCount.ToString(), lotteryName));
                    string succeededOrders = row["SucceededOrders"].ToString();
                    if (!string.IsNullOrEmpty(succeededOrders))
                    {
                        string[] poids = succeededOrders.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        succeededOrders = succeededOrders.EndsWith(",") ? succeededOrders.Substring(0, succeededOrders.Length - 1) : succeededOrders;
                        //txtInfo.Text += string.Format("本次【{2}】成功{0}次，所涉及的奖单ID为：{1}", poids.Length.ToString(), succeededOrders, lotteryName) + Environment.NewLine;
                        log.Info(string.Format("本次【{2}】成功{0}次，所涉及的奖单ID为：{1}", poids.Length.ToString(), succeededOrders, lotteryName));
                    }
                    else
                    {
                        //txtInfo.Text += string.Format("本次【{0}】无一成功！", lotteryName) + Environment.NewLine;
                        log.Info(string.Format("本次【{0}】无一成功！", lotteryName));
                    }
                    string failedOrders = row["FailedOrders"].ToString();
                    if (!string.IsNullOrEmpty(failedOrders))
                    {
                        string[] poids = failedOrders.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        failedOrders = failedOrders.EndsWith(",") ? failedOrders.Substring(0, failedOrders.Length - 1) : failedOrders;
                        //txtInfo.Text += string.Format("本次【{2}】失败{0}次，所涉及的奖单ID为{1}", poids.Length.ToString(), failedOrders, lotteryName) + Environment.NewLine;
                        log.Info(string.Format("本次【{2}】失败{0}次，所涉及的奖单ID为{1}", poids.Length.ToString(), failedOrders, lotteryName));
                    }
                    else
                    {
                        txtInfo.Text += string.Format("本次【{0}】全部成功！", lotteryName) + Environment.NewLine;
                        log.Info(string.Format("本次【{0}】全部成功！", lotteryName));
                    }
                }
                else
                {
                    txtInfo.Text += string.Format("本次没有需要开奖的【{0}】", lotteryName) + Environment.NewLine;
                    log.Info(string.Format("本次没有需要开奖的【{0}】", lotteryName));
                }
            }
        }

        private void btnStopReveal_Click(object sender, EventArgs e)
        {
            btnStopReveal.Enabled = false;
            _revealWatchingStopped = true;
            _revealLottery.Abort();
            txtInfo.Text += "开奖监控已停止" + Environment.NewLine + Environment.NewLine;

            log.Info("开奖监控已停止");

            btnOpenLottery.Enabled = true;
        }

        //显示托盘提醒
        private void ShowTip()
        {
            //ni.ShowBalloonTip(5000, "提示",
            //    "系统并没有退出只是最小化到托盘，您可以在托盘图标上使用右键菜单来调出主窗口！", ToolTipIcon.Info);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_fromTrayExit)
            {
                //最小化到托盘
                this.Hide();
                ni.Visible = true;
                e.Cancel = true;
                ShowTip();
            }
            //_revealWatchingStopped = true;
            //log.Info("程序正在退出");
        }

        //处理托盘退出
        private void ctmExitSystem_Click(object sender, EventArgs e)
        {
            _fromTrayExit = true;
            ni.Visible = false;
            Application.Exit();
        }

        //处理托盘显示主窗口
        private void ctmShowMain_Click(object sender, EventArgs e)
        {
            this.Show();
            this.BringToFront();
            this.WindowState = FormWindowState.Maximized;
            ni.Visible = false;
        }

        //双击打开主窗口
        private void ni_DoubleClick(object sender, EventArgs e)
        {
            ctmShowMain_Click(null, null);
        }

        private void frmRobot_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.Info("程序已退出");
        }

        //自动运行
        private string AutoRunKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private void SetAutoRun()
        {
            try
            {
                /*获得当前登录的Windows用户标示
                System.Security.Principal.WindowsIdentity identity = 
                    System.Security.Principal.WindowsIdentity.GetCurrent();

                //判断当前登录用户是否为管理员
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal( identity );
                if (!principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    //创建启动对象
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

                    //设置运行文件
                    startInfo.FileName = Application.ExecutablePath;


                    //如果不是管理员，则启动UAC
                    System.Diagnostics.Process.Start(startInfo);

                    //退出
                    System.Windows.Forms.Application.Exit();
                }*/

                string path = Application.ExecutablePath;
                RegistryKey root = Registry.LocalMachine;

                //判断键值是不是存在，如果不在则创建
                RegistryKey run = root.CreateSubKey(AutoRunKey);
                object obj = run.GetValue(Application.ProductName);
                if (obj == null)
                {
                    run.SetValue(Application.ProductName, path);
                }

                root.Close();
            }
            catch (Exception ex)
            {
                log.Info("添加自动运行出错", ex);
            }
        }
    }
}
