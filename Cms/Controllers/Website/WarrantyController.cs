using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class WarrantyController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string serial = "")
        {
          
            List<Warranty> list = new List<Warranty>();
            if (serial == "")
            {
                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, list);
            }
            else
            {
                var listWarranty = _context.Warranty.Search(serial:serial);
                
                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, listWarranty);

            }
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