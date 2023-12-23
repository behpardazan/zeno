using DataLayer.Base;
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
    public class BrandController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string brbrandId = "", string brname = "", int? prpageSize = null, int? prindex = null, Enum_ProductOrder prorder = Enum_ProductOrder.MORE_SELL)
        {
            string currentUrl = Request.Url.ToString()/*.Replace("http:", "https:")*/.ToLower();
           
            if (currentUrl.Contains("/br/"))
            {
                string url = _context.ProductBrand.GetById(int.Parse(brbrandId)).GetLinkNew();
                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", url);
            }


            ProductBrand ProductBrand = _context.ProductBrand.GetById(int.Parse(brbrandId));
            if (currentUrl != ProductBrand.GetLinkNew())
            {

                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", ProductBrand.GetLinkNew());
            }
            ViewSearchBrand viewmodel = new ViewSearchBrand();
            prpageSize = prpageSize == null ? 48 : prpageSize;
            prindex = prindex == null ? 1 : prindex;
            var searchQuery = _context.Product.GetSearchQuery(brandId: brbrandId, isAdvertising: null,
            isPublish: null);
            List<Product> results = _context.Product.Search(
             pageSize: prpageSize.Value,
             index: prindex.Value,
            order: prorder,
            brandId: brbrandId,
            isAdvertising:null,
            isPublish:null);
            ViewSearchProduct search = new ViewSearchProduct()
            {
                TotalCount = _context.Product.SearchDetail(query: searchQuery).Count,
                PageIndex = prindex.Value,
                PageSize = prpageSize.Value,
                Results = results
            };
            viewmodel.SearchProduct = search;
            var brand = _context.ProductBrand.GetById(int.Parse(brbrandId));
            viewmodel.Brand = brand;
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