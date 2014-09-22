using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace LotteryDraw.Site.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeLotteryDrawleMembershipAttribute : ActionFilterAttribute
    {
        private static LotteryDrawleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET LotteryDrawle Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class LotteryDrawleMembershipInitializer
        {
            public LotteryDrawleMembershipInitializer()
            {
                //Database.SetInitializer<UsersContext>(null);

                //try
                //{
                //    using (var context = new UsersContext())
                //    {
                //        if (!context.Database.Exists())
                //        {
                //            // Create the LotteryDrawleMembership database without Entity Framework migration schema
                //            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                //        }
                //    }

                //    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                //}
                //catch (Exception ex)
                //{
                //    throw new InvalidOperationException("The ASP.NET LotteryDrawle Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                //}
            }
        }
    }
}
