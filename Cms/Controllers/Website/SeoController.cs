using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;

namespace Cms.Controllers
{
    public class SeoController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        private int baseDivision = 1500;

        public ActionResult Robots()
        {
            string baseUrl = GetBaseUrl() + "/";
            StringBuilder text = new StringBuilder();
            text.AppendLine("User-agent: *");
            text.AppendLine("Disallow: /profile/");
            text.AppendLine("Disallow: /login");
            text.AppendLine("Disallow: /register");
            text.AppendLine("Disallow: /compare?*");
            text.AppendLine("Disallow: /compare/");
            text.AppendLine("Disallow: /profile*");
            text.AppendLine("Disallow: /login?*");
            text.AppendLine("Disallow: /basket/*");
            text.AppendLine("Disallow: /register*");
            text.AppendLine("Disallow: /register/");
            text.AppendLine("Disallow: /seller/");

            text.AppendLine("Sitemap: "+baseUrl+"sitemap.xml");

            return Content(text.ToString(), "text/plain");
        }

        public ActionResult SitemapList()
        {
            string baseUrl = GetBaseUrl() + "/";

            StringBuilder xmlString = new StringBuilder();
            xmlString.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            xmlString.AppendLine("<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9' xmlns:image='http://www.google.com/schemas/sitemap-image/1.1'>");


            AppendUrlIndex(xmlString, baseUrl, BaseWebsite.ShopUrl, "1",BaseContent.WebsiteDetail.DateMap);
            List<ProductType> listProductType = _context.ProductType.Search(index: 1, pageSize: baseDivision);
            foreach (ProductType item in listProductType)
            {
                AppendUrlIndex(xmlString, baseUrl, item.GetUrlNew(), "1", item.UpdateDatetime.GetDateStringByTime());
            }

            List<ProductCategory> listProductCategory = _context.ProductCategory.Search(index: 1, pageSize: baseDivision);
            foreach (ProductCategory item in listProductCategory)
            {
                AppendUrlIndex(xmlString, baseUrl, item.GetUrlNew(), "1", item.UpdateDatetime.GetDateStringByTime());
            }

            List<ProductSubCategory> listProductSubCategory = _context.ProductSubCategory.Search(index: 1, pageSize: baseDivision);
            foreach (ProductSubCategory item in listProductSubCategory)
            {
                AppendUrlIndex(xmlString, baseUrl, item.GetUrlNew(), "1", item.UpdateDatetime.GetDateStringByTime());
            }

            List<ProductBrand> listProductBrand = _context.ProductBrand.Search(index: 1, pageSize: baseDivision);
            foreach (ProductBrand item in listProductBrand)
            {
                AppendUrlIndex(xmlString, baseUrl, item.GetLinkNew(), "1", item.UpdateDatetime.GetDateStringByTime());
            }

            List<Product> listProduct = _context.Product.Search(index: 1, pageSize: baseDivision,isAdvertising:null,isPublish:null);
            foreach (Product item in listProduct)
            {
                AppendUrlIndex(xmlString, baseUrl, item.GetLink(), "0.8",item.UpdateDatetime.GetDateStringByTime());
            }

            List<Post> listPost = _context.Post.Search(index: 1, pageSize: baseDivision);
            foreach (Post item in listPost)
            {
                AppendUrlIndex(xmlString, baseUrl, item.GetLinkWithUrl(), "0.9", item.UpdateDateTime.GetDateStringByTime());
            }


            xmlString.AppendLine("</urlset>");

            return Content(xmlString.ToString(), "text/xml");
          
        }
        private void AppendUrlIndex(StringBuilder xmlString, string baseUrl, string url, string priority = "0.9",string date="")
        {
            xmlString.AppendLine("<url>");
            xmlString.AppendLine("<loc>" + /*baseUrl*/  url + "</loc>");
            xmlString.AppendLine("<lastmod>"+ date + "</lastmod>");
            xmlString.AppendLine("<changefreq>daily</changefreq>");
            xmlString.AppendLine("<priority>" + priority + "</priority>");
            xmlString.AppendLine("</url>");
        }
        public ActionResult SitemapInfo(string sitemap = null, string pageindex = null)
        {
            string baseUrl = GetBaseUrl();
            sitemap = sitemap == null ? null : sitemap.ToUpper();
            int pageIndex = pageindex == null ? 1 : pageindex.GetInteger();

            StringBuilder xmlString = new StringBuilder();
            xmlString.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            xmlString.AppendLine("<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9' xmlns:image='http://www.google.com/schemas/sitemap-image/1.1'>");

            if (sitemap == Enum_Sitemap.PRODUCTTYPE.ToString())
            {
                List<ProductType> list = _context.ProductType.Search(index: pageIndex, pageSize: baseDivision);
                foreach (ProductType item in list)
                {
                    AppendUrlIndex(xmlString, "", item.GetUrlNew());
                }
            }
            else if (sitemap == Enum_Sitemap.PRODUCTCATEGORY.ToString())
            {
                List<ProductCategory> list = _context.ProductCategory.Search(index: pageIndex, pageSize: baseDivision);
                foreach (ProductCategory item in list)
                {
                    AppendUrlIndex(xmlString, "", item.GetUrlNew());
                }
            }
            else if (sitemap == Enum_Sitemap.PRODUCTSUBCATEGORY.ToString())
            {
                List<ProductSubCategory> list = _context.ProductSubCategory.Search(index: pageIndex, pageSize: baseDivision);
                foreach (ProductSubCategory item in list)
                {
                    AppendUrlIndex(xmlString, "", item.GetUrlNew());
                }
            }
            else if (sitemap == Enum_Sitemap.PRODUCTBRAND.ToString())
            {
                List<ProductBrand> list = _context.ProductBrand.Search(index: pageIndex, pageSize: baseDivision);
                foreach (ProductBrand item in list)
                {
                    AppendUrlIndex(xmlString, "", item.GetLinkNew());
                }
            }
            else if (sitemap == Enum_Sitemap.PRODUCT.ToString())
            {
                List<Product> list = _context.Product.Search(index: pageIndex, pageSize: baseDivision);
                foreach (Product item in list)
                {
                    AppendUrlIndex(xmlString, "", item.GetLink());
                }
            }
            else if (sitemap == Enum_Sitemap.POST.ToString())
            {
                List<Post> list = _context.Post.Search(index: pageIndex, pageSize: baseDivision);
                foreach (Post item in list)
                {
                    AppendUrlIndex(xmlString, "", item.GetLink());
                }
            }

            xmlString.AppendLine("</urlset>");

            return Content(xmlString.ToString(), "text/xml");
        }

        public ActionResult GoogleWebmaster(string webmasterId)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine("google-site-verification: google" + webmasterId + ".html");
            return Content(text.ToString(), "text/plain");
        }

        private void AppendUrlIndex(StringBuilder xmlString, string baseUrl, string url)
        {
            xmlString.AppendLine("<url>");
            xmlString.AppendLine("<loc>" + baseUrl + url + "</loc>");
            xmlString.AppendLine("<lastmod>2022-07-12</lastmod>");
            xmlString.AppendLine("<changefreq>weekly</changefreq>");
            xmlString.AppendLine("<priority>0.9</priority>");
            xmlString.AppendLine("</url>");
        }

        private void AppendSiteMapIndex(StringBuilder xmlString, string baseUrl, int pageIndex, Enum_Sitemap sitemap)
        {
            xmlString.AppendLine("<sitemap>");
            xmlString.AppendLine("<loc>"+baseUrl + "sitemap-" + sitemap.ToString().ToLower() + "-" + pageIndex + ".xml"+ "</loc>");
            xmlString.AppendLine("</sitemap>");
        }

        private string GetBaseUrl()
        {
            string baseUrl = HttpContext.Request.Url.Host;
            baseUrl = baseUrl == "localhost" ? baseUrl + ":" + HttpContext.Request.Url.Port : baseUrl;
            baseUrl = "https://" + baseUrl;
            return baseUrl;
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