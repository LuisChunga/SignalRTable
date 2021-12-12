using SignalRSingleTable.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SignalRSingleTable
{
    public class MvcApplication : System.Web.HttpApplication
    {
        readonly string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //readonly string connString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            // start SQL dependency
            SqlDependency.Start(connString);
            NotificationComponent NC = new NotificationComponent();
            var currentTime = DateTime.Now;
            NC.RegisterNotification(currentTime);

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //NotificationComponent NC = new NotificationComponent();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["LastUpdated"] = currentTime;
            //NC.RegisterNotification(currentTime);
        }

        protected void Application_End()
        {
            //Stop SQL dependency
            SqlDependency.Stop(connString);
        }
    }
}
