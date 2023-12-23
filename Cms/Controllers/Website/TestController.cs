using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class TestController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index()
        {
            //_context.AccountOrder.updateorder();
            var searchQuery = _context.Product.GetSearchQuery(
               SyncNameProductCustomField: "SpecialProduct",
               valueProductCustom: "True",
               prstock: "AVAILABLE"
               );
            List<Product> results = _context.Product.Search(
             pageSize: 100000,
             index: 1,
            query: searchQuery);
            foreach(var item in results)
            {
                var product = _context.Product.GetById(item.ID);
                product.SpecialProduct = DateTime.Now;
                _context.Product.Update(product);
                _context.Save();
            }
            return PartialView(BaseController.GetView(this));
        }

        public ActionResult MasoudTemp(string appId, string appName)
        {
            return PartialView(BaseController.GetView(this, appId + "-" + appName));
        }
        
    }
}