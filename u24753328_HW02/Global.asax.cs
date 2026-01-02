using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using u24753328_HW02.Infastructure;

namespace u24753328_HW02
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // --- NEW LINE TO ADD ---
            // Register our custom factory to handle the dependency injection
            ControllerBuilder.Current.SetControllerFactory(new SimpleControllerFactory());
            // -------------------------
        }
    }
}