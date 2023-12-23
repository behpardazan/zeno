using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;

namespace Cms.Controllers
{
    public class StaticPageController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        [OutputCache(Duration = 3600, VaryByParam = "*")]

        public ActionResult Index(string id, string parameter = null)
        {
            ViewBag.parameter = parameter;
            string currentUrl = Request.Url.ToString().ToLower();

            if (id == "coffeerecipes")
            {
                string url = "https://khoshkala.com/st/coffee-recipes";
                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", url);
                return Redirect(url);
            }
            return PartialView(BaseController.GetView(this, id));
        }
        public ActionResult Static(string id)
        {
            var model = _context.StaticPage.GetByCategory(id);
            if (model == null)
                return Redirect("/");
            return PartialView(BaseController.GetView(this), model);
        }
    }
}