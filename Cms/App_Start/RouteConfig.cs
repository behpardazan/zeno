using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataLayer.Enumarables;
using System.Xml;
using System.IO;

namespace Cms
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //            routes.MapRoute(
            //    "Store_default",
            //    "Store/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional },
            //    new[] { "AppName.Areas.Store.Controllers" }
            //);
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Product_ABAK2", "search", new { controller = "Product", action = "Search", proutput = "search" });
            routes.MapRoute("Add_ABAK", "Advertising/{prcustomLabel}", new { controller = "Advertising", action = "Search", proutput = "search" });
            routes.MapRoute("Add_ABAK2", "Advertising", new { controller = "Advertising", action = "Search", proutput = "search" });
            routes.MapRoute("Add_Page", "ad/{prid}/{pridname}", new { controller = "Advertising", action = "Index" });
            routes.MapRoute("Product_ABAKZENO1", "search/{prtypeLabel}/{prcustomLabel}", new { controller = "Product", action = "Search", proutput = "search" });
            routes.MapRoute("Product_ABAKZENO2", "search/{prtypeLabel}/{prcategoryLabel}/{prcustomLabel}", new { controller = "Product", action = "Search", proutput = "search" });

            routes.MapRoute("Product_ABAK", "search/{prcustomLabel}", new { controller = "Product", action = "Search", proutput = "search" });
            routes.MapRoute("Post_ABAK", "post/{poviewName}/{url}", new { controller = "Post", action = "Search" });


            routes.MapRoute("SeoRobots", "robots.txt", new { controller = "Seo", action = "Robots" });
            routes.MapRoute("SeoSitemapInfo", "sitemap-{sitemap}-{pageindex}.xml", new { controller = "Seo", action = "SitemapInfo" });
            routes.MapRoute("SeoSitemapListXml", "sitemap.xml", new { controller = "Seo", action = "SitemapList" });
            routes.MapRoute("SeoSitemapList", "sitemap", new { controller = "Seo", action = "SitemapList" });
            //routes.MapRoute("GoogleWebmasters", "google{webmasterId}.html", new { controller = "Seo", action = "GoogleWebmaster" });
            routes.MapRoute("City_Page", "ci/{ciid}/{ciidname}", new { controller = "City", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Product_Page", "pr/{prid}/{pridname}", new { controller = "Product", action = "Index" });


            // بچه های سئو 
            //routes.MapRoute("Product_PageNew", "product/{prfullurl}", new { controller = "Product", action = "IndexNew", apiId = UrlParameter.Optional });
            routes.MapRoute("Product_PageNew", "product/{prid}/{prName}", new { controller = "Product", action = "Index", apiId = UrlParameter.Optional });
            //routes.MapRoute("Product_Page2", "product/{pridname}", new { controller = "Product", action = "Index2", apiId = UrlParameter.Optional });
            //routes.MapRoute("Product_Page3", "service/{pridname}", new { controller = "Product", action = "Index2", apiId = UrlParameter.Optional });
            routes.MapRoute("Product_SubCategory", "products/{prsubCategoryId}/{prSubCategoryTitle}", new { controller = "Product", action = "Search", prsubCategoryId = UrlParameter.Optional });
            routes.MapRoute("Product_category2", "ptcategory/{prcategoryId}/{prCategoryTitle}", new { controller = "Product", action = "Search", prcategoryId = UrlParameter.Optional, proutput = Enum_ProductOutput.SEARCH });
            routes.MapRoute("Reseller_Page", "rs/{rsid}/{rsidname}", new { controller = "ShopReseller", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("ProductBrand_Page", "pb/{pbid}/{pbidname}", new { controller = "ProductBrand", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Product_Search_Name", "pn/{prindex}/{prname}", new { controller = "Product", action = "Search", apiId = UrlParameter.Optional, proutput = Enum_ProductOutput.SEARCH });
            routes.MapRoute("Logout", "logout", new { controller = "Account", action = "Logout" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Basket", "basket", new { controller = "Basket", action = "Index" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Login", "login", new { controller = "Account", action = "Login" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Services", "Services", new { controller = "Product", action = "Services" }, new[] { "Cms.Controllers" });
            routes.MapRoute("category", "category", new { controller = "ProductCategory", action = "Search" }, new[] { "Cms.Controllers" });
            routes.MapRoute("ProductCategory", "ProductCategory/Index/{id}", new { controller = "ProductCategory", action = "Index", id = UrlParameter.Optional }, new[] { "Cms.Controllers" });
            routes.MapRoute("Productcategorycategory", "Productcategory/{pctypeId}/{name}", new { controller = "ProductCategory", action = "Search", pctypeId = UrlParameter.Optional }, new[] { "Cms.Controllers" });
            routes.MapRoute("ProductSubCategory", "productsubcategory/{pscategoryId}/{name}", new { controller = "ProductSubCategory", action = "Search", pscategoryId = UrlParameter.Optional }, new[] { "Cms.Controllers" });
            //routes.MapRoute("RegisterReagent", "register/{reid}", new { controller = "Account", action = "Register", apiId = UrlParameter.Optional });
            routes.MapRoute("ApplicationPage", "app/{appid}", new { controller = "App", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Register", "register", new { controller = "Account", action = "Register" });
            routes.MapRoute("Forget", "forget", new { controller = "Account", action = "Forget" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Recover", "recover", new { controller = "Account", action = "Recover" }, new[] { "Cms.Controllers" });
            routes.MapRoute("MessageShop", "profile/messageshop/{id}", new { controller = "Account", action = "MessageShop", apiId = UrlParameter.Optional });
            routes.MapRoute("Message", "profile/message", new { controller = "Account", action = "Message" });
            routes.MapRoute("Follow", "profile/follow", new { controller = "Account", action = "Follow" });
            routes.MapRoute("Favorite", "favorite", new { controller = "Account", action = "Favorite" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Profile", "profile", new { controller = "Account", action = "Profile" }, new[] { "Cms.Controllers" });
            routes.MapRoute("History", "History", new { controller = "Account", action = "History" }, new[] { "Cms.Controllers" });
            //routes.MapRoute("Preview", "{username}", new { controller = "Account", action = "Preview" }, new[] { "Cms.Controllers" });
            routes.MapRoute("Tracking", "tracking", new { controller = "Account", action = "Tracking" });
            routes.MapRoute("Post_Category", "po/{poidlabel}", new { controller = "Post", action = "SearchCategory" });
            routes.MapRoute("Post_CategoryName", "poCategory/{poidlabel}/{pocategoryId}", new { controller = "Post", action = "Search", pocategoryId = UrlParameter.Optional }, new[] { "Cms.Controllers" });
            routes.MapRoute("Post_Index", "po/{poid}/{poidname}", new { controller = "Post", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Mall_Index", "ma/{maid}/{maidname}", new { controller = "Mall", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Banner_Index", "ba/{baid}/{baidname}", new { controller = "Banner", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Gallery_Index", "ga/{gaid}/{gaidname}", new { controller = "Gallery", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Shop_Index", "shop/{shid}/{shidname}", new { controller = "Shop", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Shop_Profile", "shop/profile/{shid}/{shidname}", new { controller = "Shop", action = "Profile", apiId = UrlParameter.Optional });
            routes.MapRoute("Shop_Contact", "shop/contact/{shid}/{shidname}", new { controller = "Shop", action = "Contact", apiId = UrlParameter.Optional });
            routes.MapRoute("Error_Index", "error/{id}", new { controller = "ErrorPage", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("StaticPage_Index", "st/{id}", new { controller = "StaticPage", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Category_Index", "cat/{name}", new { controller = "Category", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Shop_Search", "ss/{index}/{name}", new { controller = "Shop", action = "SearchWithProduct", apiId = UrlParameter.Optional });
            routes.MapRoute("Form_Create", "form/{frlabel}/{frname}", new { controller = "WebsiteForm", action = "Index", apiId = UrlParameter.Optional });
            /* routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }); */
            routes.MapRoute("Product_Search_SubCategory", "ps/{prsubcategoryId}/{prindex}/{prtitle}", new { controller = "Product", action = "Search", apiId = UrlParameter.Optional, proutput = Enum_ProductOutput.SEARCH });
            routes.MapRoute("Product_Search_Category", "pc/{prcategoryId}/{prindex}/{prtitle}", new { controller = "Product", action = "Search", apiId = UrlParameter.Optional, proutput = Enum_ProductOutput.SEARCH }, new[] { "Cms.Controllers" });
            //routes.MapRoute("Product_Search_CategoryNew", "category/{prcategoryId}-{prindex}-{prtitle}", new { controller = "Product", action = "Search",  apiId = UrlParameter.Optional, proutput = Enum_ProductOutput.SEARCH }, new[] { "Cms.Controllers" });
            routes.MapRoute("product_search-c", "category/{prcategoryId}/{prindex}/{prtitle}", new { controller = "Product", action = "Search", apiId = UrlParameter.Optional, proutput = Enum_ProductOutput.SEARCH });
            routes.MapRoute("product_search-d", "subcategory/{prsubCategoryId}/{prindex}/{prtitle}", new { controller = "Product", action = "Search", apiId = UrlParameter.Optional, proutput = Enum_ProductOutput.SEARCH, prsubCategoryId = UrlParameter.Optional});
            routes.MapRoute("Product_Search_Type", "pt/{prtypeId}/{prindex}/{prtitle}", new { controller = "Product", action = "Search", apiId = UrlParameter.Optional, proutput = Enum_ProductOutput.SEARCH });
            routes.MapRoute("Store_Index", "seller/{prstoreId}/{stname}", new { controller = "Store", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Brand_Index", "brand/{brbrandId}/{brname}", new { controller = "Brand", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Company_Index", "company/{cpcompanyId}/{cpname}", new { controller = "Company", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Brand_Old", "br/{brbrandId}/{brname}", new { controller = "Brand", action = "Index", apiId = UrlParameter.Optional });
            routes.MapRoute("Brand_List", "Brands", new { controller = "ProductBrand", action = "Search", apiId = UrlParameter.Optional });

            //routes.MapRoute("ProductCategory-Kohan", "pcategorylist/{pccustomLabel}", new { controller = "ProductCategory", action = "Search" });
            //routes.MapRoute("ProductSubCategory-Kohan", "psubCategorylist/{pscustomLabel}", new { controller = "ProductSubCategory", action = "Search" });

            //routes.MapRoute("Product_Compare", "pr/compare/", new { controller = "Product", action = "Compare", apiId = UrlParameter.Optional });

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Cms.Controllers" }
            );
        }
    }
}
