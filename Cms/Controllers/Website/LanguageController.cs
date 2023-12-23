using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class LanguageController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string langId)
        {
            string BackUrl = Request.QueryString["BackUrl"];
            _context.WebsiteLanguage.SetCurrentLanguage(langId);
            return Redirect( BackUrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}