using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Stardust.Particles;

namespace Stardust.WebTest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            try
            {
                // Web API configuration and services
                //   config.SuppressDefaultHostAuthentication();
                // Web API routes
                config.MapHttpAttributeRoutes();
                //config.Services.Replace(typeof(IHttpControllerSelector),new CustomControllerSelector(config.Services.GetService(typeof(IHttpControllerSelector))));
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
            }
            catch (Exception ex)
            {
                ex.Log();
                throw;
            }
        }
    }
}
