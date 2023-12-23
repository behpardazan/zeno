using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class ColorController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ActionResult Search(
            int? clnotId = null,
            int? cltypeId = null,
            int? clindex = null,
            int? clpageSize = null,
            string clname = null,
            string clviewName = null)
        {
            List<Color> results = new List<Color>();
            if(cltypeId!=null)
                results = _context.Color.Search(
                typeId: cltypeId,
                index: clindex,
                name: clname,
                notId: clnotId,
                pageSize: clpageSize);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, clviewName, results);
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