using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class CompanyController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string cpcompanyId = "", string cpname = "", int? prpageSize = null, int? prindex = null, Enum_ProductOrder prorder = Enum_ProductOrder.MORE_SELL)
        {
            ViewSearchCompany viewmodel = new ViewSearchCompany();
            prpageSize = prpageSize == null ? 48 : prpageSize;
            prindex = prindex == null ? 1 : prindex;
            var searchQuery = _context.Product.GetSearchQuery(name: cpname);
            List<Product> results = _context.Product.Search(
             pageSize: prpageSize.Value,
             index: prindex.Value,
            order: prorder,
            companyId: cpcompanyId);
            ViewSearchProduct search = new ViewSearchProduct()
            {
                TotalCount = _context.Product.SearchDetail(query: searchQuery).Count,
                PageIndex = prindex.Value,
                PageSize = prpageSize.Value,
                Results = results
            };
            viewmodel.SearchProduct = search;
            var company = _context.ProductCompany.GetById(int.Parse(cpcompanyId));
            viewmodel.Company = company;
            return PartialView(BaseController.GetView(this), viewmodel);
          
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