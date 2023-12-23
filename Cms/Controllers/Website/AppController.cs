using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class AppController : Controller
    {
        public ActionResult Index(string appid)
        {
            return PartialView(BaseController.GetView(this, appid));
        }

        public ActionResult MasoudTemp(string appId, string appName)
        {
            return PartialView(BaseController.GetView(this, appId + "-" + appName));
        }
    }
}