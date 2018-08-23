using System.Web.Mvc;
using System.Web.Routing;

namespace BasementRenting
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //home page
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            ////Ashishbhai
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Company", action = "GetCompanies", id = UrlParameter.Optional }
            //);

            //login
            routes.MapRoute(
                name: "login/",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Customer", action = "Login", id = UrlParameter.Optional }
            );

            //passwordrecovery
            routes.MapRoute(
                name: "PasswordRecovery",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Customer", action = "PasswordRecovery", id = UrlParameter.Optional }
            );

            //register
            routes.MapRoute(
                name: "register/",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Customer", action = "Register", id = UrlParameter.Optional }
            );

            //Province
            routes.MapRoute(
                name: "Province/",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Country", action = "ProvinceList", id = UrlParameter.Optional }
            );

        }
    }
}
