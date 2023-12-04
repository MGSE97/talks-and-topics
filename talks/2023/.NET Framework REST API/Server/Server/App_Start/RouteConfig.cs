using System.Web.Mvc;
using System.Web.Routing;

namespace Server
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { action = "^(?![Ww]eb[Ff]orm$).*" }
            );

            routes.MapPageRoute(
                routeName: "WebForm",
                routeUrl: "Demo/WebForm",
                physicalFile: "~/Views/Demo/WebForm.aspx"
            );
        }
    }
}
