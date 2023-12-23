using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class ProductCompanyController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? pbId = null, string pbidname = null)
        {
            ProductBrand productBrand = _context.ProductBrand.GetById(pbId);
            if (productBrand == null || productBrand.Active == false)
                return RedirectPermanent("/error/404");

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, productBrand);
        }

        public ActionResult Search(
            string pbviewName = null
           )
        {
         
            List<ProductCompany> list = _context.ProductCompany.Where(s=>s.Active).ToList();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, pbviewName, list);
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