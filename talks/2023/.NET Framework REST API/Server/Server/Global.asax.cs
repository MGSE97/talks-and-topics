using Server.Domain;
using Server.Migrations;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Run DB seed and migrations
            // We should avoid this in production!
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WeatherDbContext, Configuration>());
            using (var ctx = new WeatherDbContext())
                ctx.Database.Initialize(false);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
