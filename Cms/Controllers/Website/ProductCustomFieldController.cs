using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class ProductCustomFieldController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult search (int? pageSize = null, string name = null, string syncName = null, string ViewName= null, int? productTypeId = null)
        {
            pageSize = pageSize == null ? 10 : pageSize;
            var MyQuery = PredicateBuilder.True<ProductCustomField>();
            if (syncName != null)
                MyQuery = MyQuery.And(p => p.SyncName == syncName);
            if (productTypeId != null)
                MyQuery = MyQuery.And(p => p.ProductTypeId == productTypeId);
            List<ProductCustomField> list  = _context
                .ProductCustomField
                .Where(MyQuery)
                .OrderBy(p => Guid.NewGuid())
                .Take(pageSize.Value)
                .ToList();

            //List<ProductCustomField> list = _context.ProductCustomField
            //    .Where(p => 
            //        p.SyncName == syncName
            //    )
            //    .OrderBy(p => Guid.NewGuid())
            //    .Take(pageSize.Value)
            //    .ToList();

            return PartialView(BaseController.GetView(this, ViewName), list);
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