﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FrontEnd
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "CerrarModulo",
               url: "Account/CloseModule",
               defaults: new { controller = "Account", action = "CloseModule", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "CerrarSesion",
                url: "Account/CloseSession",
                defaults: new { controller = "Account", action = "CloseSession", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
