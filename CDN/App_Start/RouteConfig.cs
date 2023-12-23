using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CDN
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region File

            // Thumb
            routes.MapRoute("CDNFileThumb", "file/thumb/{fileId}w{width}h{height}q{quality}.jpg",
                new { controller = "File", action = "Thumb" },
                namespaces: new[] { "Website.Controllers" });

            // Thumb
            routes.MapRoute("CDNFileThumbSquare", "file/thumb/{fileId}s{size}q{quality}.jpg",
                new { controller = "File", action = "ThumbSquare" },
                namespaces: new[] { "Website.Controllers" });

            // ThumbWidth
            routes.MapRoute("CDNFileThumbWidth", "file/thumb/{fileId}w{width}q{quality}.jpg",
                new { controller = "File", action = "ThumbWidth" },
                namespaces: new[] { "Website.Controllers" });
            //
            routes.MapRoute("CDNFileThumbVideo", "file/thumb/{fileId}.mp4",
                new { controller = "File", action = "ThumbVideo" },
                namespaces: new[] { "Website.Controllers" });
            // Download
            routes.MapRoute("CDNFileDownload", "file/dl/{fileId}",
                new { controller = "File", action = "Download" },
                namespaces: new[] { "Website.Controllers" });
            routes.MapRoute("CDNFileDownload2", "file/dl2/{fileId}",
               new { controller = "File", action = "Download2" },
               namespaces: new[] { "Website.Controllers" });

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
