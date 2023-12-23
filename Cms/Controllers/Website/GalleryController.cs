using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class GalleryController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? gaid, string gaidname = "")
        {
            Gallery gallery = _context.Gallery.GetById(gaid);
            return PartialView(BaseController.GetView(this), gallery);
        }

        public ActionResult Search(
            int? ganotId = null,
            int? gawebsiteId = null,
            int? gaIndex = null,
            int? gaSize = null,
            string ganame = null,
            string gaviewName = null,
            string gacategory = null)
        {
            List<Gallery> list = _context.Gallery.Search(
                websiteId: gawebsiteId,
                index: gaIndex,
                pageSize: gaSize,
                name: ganame,
                notId: ganotId,
                categoryLabel: gacategory);
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, gaviewName, list);
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