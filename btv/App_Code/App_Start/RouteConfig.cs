using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace App_Start
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //var settings = new FriendlyUrlSettings();
            //settings.AutoRedirectMode = RedirectMode.Permanent;
            //routes.EnableFriendlyUrls(settings);

            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings, new SiteMobileMasterFriendlyUrlResolver());
            
            
            
            //routes.EnableFriendlyUrls();
            //routes.MapPageRoute("", "Default", "~/Default.aspx");
            //routes.MapPageRoute("", "company", "~/company.aspx");
            //routes.MapPageRoute("company",
            //                    "company/{q}",
            //                    "~/company");

        }
    }
}
