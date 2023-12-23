using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class ProductSubCategoryController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Search(
            int? pscategoryId = null, 
            int? psnotId = null, 
            int? psindex = null, 
            int? pspageSize = null, 
            string psname = null, 
            string psviewName = null,
            string pscustomLabel = null
            )
        {
            List<ProductSubCategory> results = new List<ProductSubCategory>();
            var MyQuery = PredicateBuilder.True<ProductSubCategory>();
            pspageSize = pspageSize == null ? 10 : pspageSize;
            psindex = psindex == null ? 1 : psindex;
            int skipValue = pspageSize.Value * (psindex.Value - 1);
            int pageValue = pspageSize.Value;
            MyQuery.And(p => p.Deleted != true);
            if (pscategoryId != null)
                MyQuery = MyQuery.And(p => p.CategoryId == pscategoryId);
            if (psnotId != null)
                MyQuery = MyQuery.And(p => p.ID != psnotId);
            if (psname != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(psname));
            if (pscustomLabel != null)
                MyQuery = MyQuery.And(p => p.CustomLabel == pscustomLabel);
            results = _context
                .ProductSubCategory
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, psviewName, results);
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