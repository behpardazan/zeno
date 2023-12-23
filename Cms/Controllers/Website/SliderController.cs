using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class SliderController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(int? slid, string slname = "")
        {
            Slider slider = _context.Slider.GetById(slid);
            _context.Slider.Update(slider);
            _context.Save();
            ViewBag.CurrentAccount = _context.Account.GetCurrentAccount();
            return PartialView(BaseController.GetView(this), slider);
        }
        [OutputCache(Duration = 3600)]
        public ActionResult Search(string slviewName = null, int? slWebsiteId = null)
        {
            List<Slider> list = _context.Slider.Search(websiteId: slWebsiteId);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, slviewName, list);
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