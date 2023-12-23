using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace CDN.Controllers
{
    public partial class ConfigsController : Controller
    {
        public ActionResult Index()
        {
            return Content(Server.MapPath("~/App_Data"));
        }
    }
}