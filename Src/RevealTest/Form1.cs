using LotteryDraw.Site;
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

namespace RevealTest
{
    [Export]
    public partial class Form1 : Form
    {
        private static CompositionContainer _container;

        [Import]
        public IPrizeOrderContract PrizeOrderContract { get; set; }

        [Import]
        public IPrizeContract PrizeContract { get; set; }

        [Import]
        public IAccountContract AccountContract { get; set; }

        [Import]
        public IPrizeBettingContract PrizeBettingContract { get; set; }

        public Form1()
        {
            InitializeComponent();
            //初始化MEF组合容器
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory()));
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);

            DatabaseInitializer.Initialize();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                        RevealStateNum = rnd.Next(4) + 1,
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
                                LuckyPercent = 2
                            };
                            break;
                        case (int)RevealType.Answer:
                            prizeOrder.Extend = new PrizeOrderExtend()
                            {
                                PrizeAsking = new PrizeAsking() { Question = "好声音冠军是谁？", Answer = "梁博" },
                                MinLuckyCount = 1,
                                LuckyPercent = 2
                            };
                            break;
                        case (int)RevealType.Quota:
                            prizeOrder.Extend = new PrizeOrderExtend()
                            {
                                PoolCount = 10,
                                LuckyPercent = 1
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
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }
    }
}
