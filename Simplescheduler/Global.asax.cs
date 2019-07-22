using Simplescheduler.Logger;
using Simplescheduler.Models;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Simplescheduler
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static int userId = 1;
        ILogger logger = new TextLogger($"FPI_{userId}");

        protected void Application_Start()
        {
            logger.Log("Application_Start.Time Taken = " + DateTime.UtcNow, LogType.Error);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JobScheduler.Start();
        }
    }
}
