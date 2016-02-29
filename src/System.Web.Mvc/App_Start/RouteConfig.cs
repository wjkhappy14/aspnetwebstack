using MvcXmlRouting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { culture="zh", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //配置文化区域Url 模板
            routes.MapRoute(
                name: "culture",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { culture="zh", controller= "ThreadCulture", action="Index",id=UrlParameter.Optional });
            ///通过XML 配置Url 模板
            MvcRouteConfigurationSection section =
          (MvcRouteConfigurationSection)ConfigurationManager.GetSection("RouteConfiguration");
            routes.MapRoute(section);
        }
    }
}

