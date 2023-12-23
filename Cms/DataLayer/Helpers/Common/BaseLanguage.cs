using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Base
{
    public static class BaseLanguage
    {
        public static Enum_Lang GetCurrentLanguage()
        {
            string mylang = null;
            //if (HttpContext.Current.Session != null)
            //{
            //    mylang = HttpContext.Current.Session["LANGUAGE"] != null ?
            //        HttpContext.Current.Session["LANGUAGE"].ToString() : null;
            //}
            //if (HttpContext.Current.Session != null)
            //{
            //    mylang = HttpContext.Current.Session["LANGUAGE"] != null ?
            //        HttpContext.Current.Session["LANGUAGE"].ToString() : null;
            //}
            //try {

            //    mylang = HttpContext.Current.Request.Cookies["LANGUAGE"].Value;
            //}
            //catch { mylang = null; }
            if (mylang == null)
            {
                if (HttpContext.Current.Request.Cookies["LANGUAGE"] != null)
                {
                    mylang = HttpContext.Current.Request.Cookies["LANGUAGE"].Value;
                }
            }
            if (mylang == null)
            {
                UnitOfWork _context = new UnitOfWork();
                WebsiteDomain domain = _context.WebsiteDomain.GetByUrl(HttpContext.Current.Request.Url);
                if (domain != null && domain.LanguageId.HasValue)
                {
                    mylang = domain.Language.Culture;
                }
                _context.Dispose();
            }
            if (mylang == null)
            {
                mylang = Enum_Lang.FA.ToString();
            }

            string value = mylang.ToUpper();
            if (value == Enum_Lang.FA.ToString())
                return Enum_Lang.FA;
            else if (value == Enum_Lang.EN.ToString())
                return Enum_Lang.EN;
            else if (value == Enum_Lang.RU.ToString())
                return Enum_Lang.RU;
            else if (value == Enum_Lang.AR.ToString())
                return Enum_Lang.AR;
            else if (value == Enum_Lang.TR.ToString())
                return Enum_Lang.TR;
            else if (value == Enum_Lang.GR.ToString())
                return Enum_Lang.GR;
            else if (value == Enum_Lang.ZH.ToString())
                return Enum_Lang.ZH;
            else
                return Enum_Lang.FA;

        }
        public static int GetCurrentLanguageId(UnitOfWork context)
        {
            var current = GetCurrentLanguage().ToString();
            if (context == null)
            {
                context = new UnitOfWork();
            }
            var language = context.Language.GetByLabel(current);
            return language.ID;
        }
        public static string GetCurrentLanguageString()
        {
            var current = GetCurrentLanguage().ToString();

            return current;
        }
        public static string GetName(this Menu menu, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return menu.Name;
            else
            {
                MenuLanguage temp = menu.MenuLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : menu.Name;
            }
        }

        public static string GetName(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Name;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : product.Name;
            }
        }
        public static string GetName(this City city, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return city.Name;
            else
            {
                CityLanguage temp = city.CityLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : city.Name;
            }
        }
        public static string GetName(this State state, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return state.Name;
            else
            {
                StateLanguage temp = state.StateLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : state.Name;
            }
        }
        public static string GetH1(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.H1;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.H1 : product.H1;
            }
        }
        public static string GetDescription(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Description;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Description : product.Description;
            }
        }
        public static string GetSummery(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Summary;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Summary : product.Summary;
            }
        }
        public static string GetTitle(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Title;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Title : product.Title;
            }
        }

        public static string GetUrl(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);

            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.Url;
            else
            {
                return temp != null ? temp.Url : productSubCategory.Url;
            }
        }
        public static string GetUrlNew(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                string customLabel = "";
                if (productSubCategory.CustomLabel != null && productSubCategory.CustomLabel != "") customLabel = productSubCategory.CustomLabel;
                else customLabel = "product-subcategory";
                return BaseWebsite.ShopUrl+"search/" + customLabel ;
            }
               
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                string customLabel = "";
                if (temp != null)
                {
                    if (temp.ProductSubCategory.CustomLabel != null && temp.ProductSubCategory.CustomLabel != "") customLabel = temp.ProductSubCategory.CustomLabel;
                    else customLabel = "product-subcategory";
                    return "/search/" + customLabel;
                }
                else return "/search/" + customLabel;

            }
        }
        public static string GetUrlNewZeno(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                string customLabel = "";
                if (productSubCategory.CustomLabel != null && productSubCategory.CustomLabel != "") customLabel = productSubCategory.CustomLabel;
                else customLabel = "product-subcategory";
                return BaseWebsite.ShopUrl + "search/" + productSubCategory.ProductCategory.ProductType.CustomLabel + "/" + productSubCategory.ProductCategory.CustomLabel + "/" + customLabel;
            }

            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                string customLabel = "";
                if (temp != null)
                {
                    if (temp.ProductSubCategory.CustomLabel != null && temp.ProductSubCategory.CustomLabel != "") customLabel = temp.ProductSubCategory.CustomLabel;
                    else customLabel = "product-subcategory";
                    return BaseWebsite.ShopUrl + "search/" + productSubCategory.ProductCategory.ProductType.CustomLabel + "/" + productSubCategory.ProductCategory.CustomLabel + "/" + customLabel;


                }
                else return BaseWebsite.ShopUrl + "search/" + productSubCategory.ProductCategory.ProductType.CustomLabel + "/" + productSubCategory.ProductCategory.CustomLabel + "/" + customLabel;


            }
        }
        public static string GetUrlAdvertising(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                string customLabel = "";
                if (productSubCategory.CustomLabel != null && productSubCategory.CustomLabel != "") customLabel = productSubCategory.CustomLabel;
                else customLabel = "product-subcategory";
                return BaseWebsite.ShopUrl + "Advertising/" + customLabel;
            }

            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                string customLabel = "";
                if (temp != null)
                {
                    if (temp.ProductSubCategory.CustomLabel != null && temp.ProductSubCategory.CustomLabel != "") customLabel = temp.ProductSubCategory.CustomLabel;
                    else customLabel = "product-subcategory";
                    return "/Advertising/" + customLabel;
                }
                else return "/Advertising/" + customLabel;

            }
        }
        public static string GetBrandUrlNew(this Product product,string customLabel, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            
                int? brandId = product.BrandId;
            if (brandId != null)
            {
                return "/search/" + customLabel+ "?prbrandId="+ brandId+ "&proutput=search";
            }
            else
            {
                return "/search/" + customLabel;
            }
                
        }
        public static string GetUrl(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.URL;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.URL : post.URL;
            }
        }


        public static string GetUrl(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.Url;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Url : productType.Url;
            }
        }
        public static string GetUrlNew(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                string customLabel = "";
                if (productType.CustomLabel != null && productType.CustomLabel != "") customLabel = productType.CustomLabel;
                else customLabel = "product-type";
                return BaseWebsite.ShopUrl + "search/" + customLabel;
            }
              
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                string customLabel = "";
                if (temp != null)
                {
                    if (temp.ProductType.CustomLabel != null && temp.ProductType.CustomLabel != "") customLabel = temp.ProductType.CustomLabel;
                    else customLabel = "product-type";

                }
                else customLabel = "product-type";

                return "/search/" + customLabel;

            }
        }

        public static string GetUrlAdvertising(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                string customLabel = "";
                if (productType.CustomLabel != null && productType.CustomLabel != "") customLabel = productType.CustomLabel;
                else customLabel = "product-type";
                return BaseWebsite.ShopUrl + "Advertising/" + customLabel;
            }

            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                string customLabel = "";
                if (temp != null)
                {
                    if (temp.ProductType.CustomLabel != null && temp.ProductType.CustomLabel != "") customLabel = temp.ProductType.CustomLabel;
                    else customLabel = "product-type";

                }
                else customLabel = "product-type";

                return "/Advertising/" + customLabel;

            }
        }
        //public static string GetUrl(this ProductType productType,int prcategoryId,int prtypeId, Enum_Lang lang = Enum_Lang.NONE)
        //{
        //    string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();


        //    if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
        //    {
        //        string customLabel = "";
        //        if (productType.CustomLabel != null && productType.CustomLabel != "") customLabel = productType.CustomLabel;
        //        else customLabel = "product-category";
        //        return "/search/" + customLabel + "?proutput=" + Enum_ProductOutput.SEARCH.ToString().ToLower() + "&prcategoryId=" + prcategoryId + "&prtypeId=" + prtypeId;

        //    }

        //    else
        //    {
        //        ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
        //        string customLabel = "";
        //        if (temp.ProductType.CustomLabel != null && temp.ProductType.CustomLabel != "") customLabel = temp.ProductType.CustomLabel;
        //        else customLabel = "product-category";
        //        return "/search/"+ customLabel+ "?proutput="+ Enum_ProductOutput.SEARCH.ToString().ToLower()+ "&prcategoryId="+ prcategoryId+ "&prtypeId=" + prtypeId;
        //    }
        //}

        public static string GetUrl(this Product product, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return product.Url;
            else
            {
                ProductLanguage temp = product.ProductLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Url : product.Url;
            }
        }

        public static string GetName(this State state, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return state.Name;
            else
            {
                StateLanguage temp = state.StateLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : state.Name;
            }
        }
        public static string GetUrl(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.Url;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Url : productCategory.Url;
            }
        }
        public static string GetUrlNew(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();


            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                string customLabel = "";
                if (productCategory.CustomLabel != null && productCategory.CustomLabel != "") customLabel = productCategory.CustomLabel;
                else customLabel = "product-category";
                return BaseWebsite.ShopUrl+"search/" + customLabel;

            }

            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                string customLabel = "";
                if (temp != null)
                {
                    if (temp.ProductCategory.CustomLabel != null && temp.ProductCategory.CustomLabel != "") customLabel = temp.ProductCategory.CustomLabel;
                    else customLabel = "product-category";
                    return BaseWebsite.ShopUrl + "search/" + customLabel;
                }
                else return BaseWebsite.ShopUrl + "search/" + customLabel;

            }
        }
        public static string GetUrlNewZeno(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();


            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                string customLabel = "";
                if (productCategory.CustomLabel != null && productCategory.CustomLabel != "") customLabel = productCategory.CustomLabel;
                else customLabel = "product-category";
                return BaseWebsite.ShopUrl + "search/" + productCategory.ProductType.CustomLabel + "/" + customLabel;

            }

            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                string customLabel = "";
                if (temp != null)
                {
                    if (temp.ProductCategory.CustomLabel != null && temp.ProductCategory.CustomLabel != "") customLabel = temp.ProductCategory.CustomLabel;
                    else customLabel = "product-category";
                    return BaseWebsite.ShopUrl + "search/" + productCategory.ProductType.CustomLabel + "/" + customLabel;
                }
                else return BaseWebsite.ShopUrl + "search/" + productCategory.ProductType.CustomLabel + "/" + customLabel;

            }
        }
        public static string GetUrlAdvertising(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();


            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
            {
                string customLabel = "";
                if (productCategory.CustomLabel != null && productCategory.CustomLabel != "") customLabel = productCategory.CustomLabel;
                else customLabel = "product-category";
                return BaseWebsite.ShopUrl + "Advertising/" + customLabel;

            }

            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                string customLabel = "";
                if (temp != null)
                {
                    if (temp.ProductCategory.CustomLabel != null && temp.ProductCategory.CustomLabel != "") customLabel = temp.ProductCategory.CustomLabel;
                    else customLabel = "product-category";
                    return BaseWebsite.ShopUrl + "Advertising/" + customLabel;
                }
                else return BaseWebsite.ShopUrl + "Advertising/" + customLabel;

            }
        }
        public static string GetUrlDefaultLanguage(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
                string customLabel = "";
                if (productCategory.CustomLabel != null && productCategory.CustomLabel != "") customLabel = productCategory.CustomLabel;
                else customLabel = "product-category";
                return BaseWebsite.ShopUrl + "search/" + customLabel;
        }

        public static string GetH1(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.H1;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : productCategory.H1;
            }
        }


        public static string GetH1(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.H1;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : productType.H1;
            }
        }



        public static string GetH1(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.H1;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : productSubCategory.H1;
            }
        }

        public static string GetName(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.Name;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                if (temp == null || temp.Name == null)
                {
                    return post.Name;
                }
                else
                {
                    return temp.Name;
                }
                //return temp != null ? temp.Name : post.Name;
            }
        }
        public static string GetH1(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.H1;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                if (temp == null || temp.H1 == null)
                {
                    return post.H1;
                }
                else
                {
                    return temp.H1;
                }
                //return temp != null ? temp.Name : post.Name;
            }
        }
        public static string GetName(this Gallery gallery, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return gallery.Name;
            else
            {
                GalleryLanguage temp = gallery.GalleryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                if (temp == null || temp.Name == null)
                {
                    return gallery.Name;
                }
                else
                {
                    return temp.Name;
                }
            }
        }


        public static string GetBody(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.Body;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Body : post.Body;
            }
        }


        public static string GetDescription(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.Description;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Description : productType.Description;
            }
        }



        public static string GetDescription(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.Description;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.description : productSubCategory.Description;
            }
        }
        public static string GetMetaDescriptionLang(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.DescriptionMeta;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.DescriptionMeta : productCategory.DescriptionMeta;
            }
        }

        public static string GetDescriptionLang(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.Description;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Description : productCategory.Description;
            }
        }

        public static string GetSummery(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.Summary;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Summary : post.Summary;
            }
        }

        public static string GetKeywords(this Post post, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return post.Keywords;
            else
            {
                PostLanguage temp = post.PostLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Keywords : post.Keywords;
            }
        }
        public static string GetName(this Size size, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return size.Name;
            else
            {
                SizeLanguage temp = size.SizeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : size.Name;
            }
        }

        public static string GetName(this Color color, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return color.Name;
            else
            {
                ColorLanguage temp = color.ColorLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : "";
            }
        }

        public static string GetName(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.Name;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : productType.Name;
            }
        }

        public static string GetName(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.Name;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : productCategory.Name;
            }
        }

        public static string GetName(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.Name;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : productSubCategory.Name;
            }
        }


        public static string GetTitle(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.title;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.title : productType.title;
            }
        }

        public static string GetTitlePage(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.TitlePage;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.TitlePage : productCategory.TitlePage;
            }
        }




        public static string GetTitle(this ProductSubCategory productSubcategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubcategory.Title;
            else
            {
                ProductSubCategoryLanguage temp = productSubcategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Title : productSubcategory.Title;
            }
        }
        public static string GetDescription(this ProductCategory productCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCategory.Description;
            else
            {
                ProductCategoryLanguage temp = productCategory.ProductCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Description : productCategory.Description;
            }
        }

        public static string GetBody(this ProductSubCategory productSubCategory, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productSubCategory.Body;
            else
            {
                ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.body : productSubCategory.Body;
            }
        }


        public static string GetBody(this ProductType productType, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productType.body;
            else
            {
                ProductTypeLanguage temp = productType.ProductTypeLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.body : productType.body;
            }
        }


        public static string GetName(this Category category, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return category.Name;
            else
            {
                CategoryLanguage temp = category.CategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : category.Name;
            }
        }
        public static string GetBody(this Category category, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return category.Body;
            else
            {
                CategoryLanguage temp = category.CategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Body : category.Body;
            }
        }
        public static string GetName(this ProductCustomField productCustomField, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCustomField.Name;
            else
            {
                ProductCustomFieldLanguage temp = productCustomField.ProductCustomFieldLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : productCustomField.Name;
            }
        }

        public static string GetValue(this ProductCustomItem productCustomItem, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return productCustomItem.Value;
            else
            {
                ProductCustomItemLanguage temp = productCustomItem.ProductCustomItemLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Value : productCustomItem.Value;
            }
        }

        public static string GetName(this Slider slider, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return slider.Name;
            else
            {
                SliderLanguage temp = slider.SliderLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : slider.Name;
            }
        }
        public static string GetSecondName(this Slider slider, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return slider.SecondName;
            else
            {
                SliderLanguage temp = slider.SliderLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.SecondName : slider.SecondName;
            }
        }    
        public static string GetName(this Banner banner, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return banner.Name;
            else
            {
                BannerLanguage temp = banner.BannerLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : banner.Name;
            }
        }
        public static string GetName(this ProductBrand brand, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return brand.Name;
            else
            {
                ProductBrandLanguage temp = brand.ProductBrandLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : brand.Name;
            }
        }
        public static string GetText(this QuestionSmartOffer question, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return question.Text;
            else
            {
                QuestionSmartOfferLanguage temp = question.QuestionSmartOfferLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Text : question.Text;
            }
        }
        public static string GetText(this AnswerSmartOffer answer, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return answer.Text;
            else
            {
                AnswerSmartOfferLanguage temp = answer.AnswerSmartOfferLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Text : answer.Text;
            }
        }
        public static string GetTitle(this ProductBrand brand, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return brand.Title;
            else
            {
                ProductBrandLanguage temp = brand.ProductBrandLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Title : brand.Title;
            }
        }
        public static string GetDescription(this ProductBrand brand, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return brand.Description;
            else
            {
                ProductBrandLanguage temp = brand.ProductBrandLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Description : brand.Description;
            }
        }
        public static string GetSummery(this Banner banner, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return banner.Summary;
            else
            {
                BannerLanguage temp = banner.BannerLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Summary : banner.Summary;
            }
        }
        public static string GetName(this Survey survey, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return survey.Name;
            else
            {
                SurveyLanguage temp = survey.SurveyLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : survey.Name;
            }
        }

        public static string GetQuestion(this SurveyQuestion question, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return question.Question;
            else
            {
                SurveyQuestionLanguage temp = question.SurveyQuestionLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Question : question.Question;
            }
        }
        public static string GetName(this SurveyQuestionItem questionItem, Enum_Lang lang = Enum_Lang.NONE)
        {
            string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
            if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
                return questionItem.Name;
            else
            {
                SurveyQuestionItemLanguage temp = questionItem.SurveyQuestionItemLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
                return temp != null ? temp.Name : questionItem.Name;
            }
        }


        public static List<WebsiteLanguage> GetWebsiteLanguages()
        {
            UnitOfWork _context = new UnitOfWork();
            Uri url = HttpContext.Current.Request.Url;
            string panel_type = Enum_Code.SYSTEM_TYPE_PANEL.ToString();
            string label = url.Host == "localhost" ? url.Host + ":" + url.Port : url.Host;
            WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p =>
                p.Website.Code.Label == panel_type &&
                (
                    p.Domain == "http://" + label + "/" ||
                    p.Domain == "http://" + label ||
                    p.Domain == "https://" + label + "/" ||
                    p.Domain == "https://" + label)
                );

            if (domain != null)
                return domain.Website.WebsiteLanguage.ToList();
            else
                return new List<WebsiteLanguage>();
        }

        public static bool HasLanguage(Enum_Lang lang)
        {
            UnitOfWork _context = new UnitOfWork();
            Uri url = HttpContext.Current.Request.Url;
            string panel_type = Enum_Code.SYSTEM_TYPE_PANEL.ToString();
            string lang_string = lang.ToString();
            string label = url.Host == "localhost" ? url.Host + ":" + url.Port : url.Host;
            WebsiteDomain domain = _context.WebsiteDomain.FirstOrDefault(p =>
                p.Website.Code.Label == panel_type &&
                p.Website.WebsiteLanguage.Any(s =>
                    s.Language.Label == lang_string ||
                    s.Language.Culture == lang_string
                ) &&
                (
                    p.Domain == "http://" + label + "/" ||
                    p.Domain == "http://" + label ||
                    p.Domain == "https://" + label + "/" ||
                    p.Domain == "https://" + label)
                );
            return domain != null ? true : false;
        }
        //public static string GetUrlProductCategory(this ProductCategory productCategory)
        //{
        //    string customLabel = "";
        //    if (productCategory.CustomLabel != null && productCategory.CustomLabel != "")
        //        customLabel = productCategory.CustomLabel;
        //    else customLabel = "product-category";
        //    return "/pcategorylist/" + customLabel;
        //}
        //public static string GetUrlProductSubCategory(this ProductSubCategory productSubCategory)
        //{
        //    string customLabel = "";
        //    if (productSubCategory.CustomLabel != null && productSubCategory.CustomLabel != "")
        //        customLabel = productSubCategory.CustomLabel;
        //    else customLabel = "product-category";
        //    return "/psubCategorylist/" + customLabel;
        //}

    }
}
