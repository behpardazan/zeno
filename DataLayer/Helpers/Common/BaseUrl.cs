using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using HashidsNet;

namespace DataLayer.Base
{
    public static class BaseUrl
    {
        public static string CacheVersion { get { return WebConfigurationManager.AppSettings["CACHE_VERSION"]; } }

        public static string CurrentUrl
        {
            get
            {
                Uri url = HttpContext.Current.Request.Url;
                if (url.Host == "localhost")
                    return url.Scheme + "://" + url.Host + ":" + url.Port + "/";
                return url.Scheme + "://" + url.Host + "/";
            }
        }

        public static string GetUrl(this ViewPicture picture, string domain = "")
        {
            string type_panel_string = Enum_Code.SYSTEM_TYPE_PANEL.ToString();
            string type_shop_string = Enum_Code.SYSTEM_TYPE_SHOP.ToString();

            if (String.IsNullOrEmpty(domain))
            {
                if (picture != null)
                {
                    string url = picture.Url;
                    url = url.Replace(type_panel_string, BaseWebsite.PanelUrl);
                    url = url.Replace(type_shop_string, BaseWebsite.ShopUrl);
                    return url;
                }
                else
                    return "";
            }
            else
            {
                if (String.IsNullOrEmpty(picture.Url))
                    return "";
                else
                    return picture.Url.Replace(type_panel_string, domain);
            }
        }
        public static string GetUrl(this Picture picture)
        {
            if (picture != null)
            {
                string url = picture.Url;
                url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString(), BaseWebsite.SyncUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_CMS.ToString(), BaseWebsite.WildCardDomain);
                return url;
            }
            else
                return "";
        }
        public static string GetUrl(this WebsiteDocument picture)
        {
            if (picture != null)
            {
                string url = picture.Url;
                url = url.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SHOP.ToString(), BaseWebsite.ShopUrl);
                url = url.Replace(Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString(), BaseWebsite.SyncUrl);
                return url;
            }
            else
                return "";
        }

        public static string GetUrl(this ProductPicture picture)
        {
            if (picture != null)
                return picture.Picture.GetUrl();
            else
                return "";
        }

        public static string GetUrl(this WebsiteDocument document, string domain = "")
        {
            string type_panel_string = Enum_Code.SYSTEM_TYPE_PANEL.ToString();
            string type_shop_string = Enum_Code.SYSTEM_TYPE_SHOP.ToString();

            if (String.IsNullOrEmpty(domain))
            {
                if (document != null)
                {
                    string url = document.Url;
                    url = url.Replace(type_panel_string, BaseWebsite.PanelUrl);
                    url = url.Replace(type_shop_string, BaseWebsite.ShopUrl);
                    return url;
                }
                else
                    return "";
            }
            else
            {
                if (String.IsNullOrEmpty(document.Url))
                    return "";
                else
                    return document.Url.Replace(type_panel_string, domain);
            }
        }

        public static string GetLink(this Post post)
        {
            return BaseWebsite.ShopUrl + "po/" + post.ID + "/" + post.URL.StandardUrl();
        }
        public static string GetLinkWithName(this Post post)
        {
            return BaseWebsite.ShopUrl + "po/" + post.ID + "/" + post.GetName();
        }

        public static string GetLinkWithUrl(this Post post)
        {
            if (!string.IsNullOrEmpty(post.URL))
            {

                return BaseWebsite.ShopUrl + "po/" + post.ID + "/" + post.GetUrl().StandardUrl();

            }
            return BaseWebsite.ShopUrl + "po/" + post.ID + "/" + post.GetName().StandardUrl();
        }
        public static string GetLinkWithUrl(this Gallery gallery)
        {
            return "/ga/" + gallery.ID + "/" + gallery.Name.StandardUrl();
        }

        public static string GetLink(this Banner banner)
        {
            return BaseWebsite.ShopUrl + "/ba/" + banner.ID + "/" + banner.Name.StandardUrl();
        }

        public static string GetLink(this ShopReseller reseller)
        {
            return BaseWebsite.ShopUrl + "rs/" + reseller.ID + "/" + reseller.Name.StandardUrl();
        }

        public static string GetLink(this Product product)
        {
            return BaseWebsite.ShopUrl+"pr/" + product.ID + "/" + product.GetName().StandardUrl();
        }

        public static string GetLinkAddvertising(this Product product)
        {
            return BaseWebsite.ShopUrl + "ad/" + product.ID + "/" + product.GetName().StandardUrl();
        }


        public static string GetLinkSeo(this Product product)
        {
            string name = product.GetUrl();
            if (string.IsNullOrEmpty(name))
            {
                name = product.GetName();
            }
            return BaseWebsite.ShopUrl + "product/" + product.ID + "/" + name.StandardUrl();
        }

        public static string GetSeoLink(this Product product)
        {
            return BaseWebsite.ShopUrl + "product/" + product.ID + "/" + product.GetUrl().StandardUrl();
        }

        public static string GetLinkWithUrl(this Product product)
        {
            if (!string.IsNullOrEmpty(product.GetUrl()))
            {
                return BaseWebsite.ShopUrl + "pr/" + product.ID + "/" + product.GetUrl().StandardUrl();
            }
            return BaseWebsite.ShopUrl + "pr/" + product.ID + "/" + product.GetName().StandardUrl();
        }

        public static string GetLinkWithCodeValue(this Product product)
        {
            return BaseWebsite.ShopUrl + "product/" + product.ID + "/" + product.CodeValue;
        }



        public static string GetLinkAbsolute(this Product product)
        {
            return BaseWebsite.ShopUrl + GetLink(product);
        }

        public static string GetLink(this Category category)
        {
            return BaseWebsite.ShopUrl + "po/" + category.Label;
        }
        public static string GetIndexLink(this Category category)
        {
            return BaseWebsite.ShopUrl + "cat/" + category.Label;
        }


        public static string GetLink(this Gallery gallery)
        {
            return BaseWebsite.ShopUrl + "ga/" + gallery.ID + "/" + gallery.Name.StandardUrl();
        }

        public static string GetLink(this Menu menu)
        {
            string link = "";
            if (menu.Code.Label == Enum_Code.MENU_TYPE_POST.ToString())
                link = menu.Post.GetLink();
            else if (menu.Code.Label == Enum_Code.MENU_TYPE_CATEGORY.ToString())
                link = menu.Category.GetLink();
            else if (menu.Code.Label == Enum_Code.MENU_TYPE_GALLERY.ToString())
                link = menu.Gallery.GetLink();
            else if (menu.Code.Label == Enum_Code.MENU_TYPE_LINK.ToString())
                link = menu.Link;
            return link;
        }

        public static string GetLink(this ProductBrand brand)
        {
            return BaseWebsite.ShopUrl + "br/" + brand.ID + "/" + brand.Name.StandardUrl();
        }
        public static string GetLinkNew(this ProductBrand brand)
        {
            if (brand != null)
            {
                if (brand.Label != "" && brand.Label != null)
                {
                    return BaseWebsite.ShopUrl + "brand/" + brand.ID + "/" + brand.Label.StandardUrl();
                }
                else
                {
                    return BaseWebsite.ShopUrl + "brand/" + brand.ID + "/" + brand.Name.StandardUrl();
                }
            }
            else
            {
                return "";
            }


        }
        public static string GetLinkNew(this ProductCompany company)
        {
            if (company != null)
            {
                if (company.Label != "" && company.Label != null)
                {
                    return "/company/" + company.ID + "/" + company.Label.StandardUrl();
                }
                else
                {
                    return "/company/" + company.ID + "/" + company.Name.StandardUrl();
                }
            }
            else
            {
                return "";
            }


        }
        public static string GetLink(this ProductType entity, int index = 1)
        {
            return "/pt/" + entity.ID + "/" + index + "/" + entity.GetName().StandardUrl();
        }

        public static string GetLinkwithURl(this ProductType entity, int index = 1)
        {

            string Name = entity.GetUrl();

            if (Name == null)
            {
                Name = entity.GetName();

            }
            return "/pt/" + entity.ID + "/" + index + "/" + Name.StandardUrl();

        }


        public static string GetLinkwithTitle(this ProductType entity, int index = 1)
        {

            string Name = entity.GetTitle();

            if (Name == null)
            {
                Name = entity.GetName();

            }
            return "/pt/" + entity.ID + "/" + index + "/" + Name.StandardUrl();

        }


        public static string GetLink(this ProductCategory entity, int index = 1)
        {
            return "/pc/" + entity.ID + "/" + index + "/" + entity.GetName().StandardUrl();
        }


        public static string GetLinkwithURl(this ProductCategory entity, int index = 1)
        {
            if (!string.IsNullOrEmpty(entity.GetUrl()))
            {

                return "/category/" + entity.ID + "/" + index + "/" + entity.GetUrl().StandardUrl();

            }
            return "/category/" + entity.ID + "/" + index + "/" + entity.GetName().StandardUrl();
        }

        public static string GetLinkPtcategorywithURl(this ProductCategory entity, int index = 1)
        {
            if (!string.IsNullOrEmpty(entity.GetUrl()))
            {

                return "/ptcategory/" + entity.ID + "/" + entity.GetUrl().StandardUrl();

            }
            return "/ptcategory/" + entity.ID + "/" + entity.GetName().StandardUrl();
        }

        public static string GetLinkCategory(this ProductCategory entity, int index = 1)
        {
            string Name = entity.GetUrl();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return "/category/" + entity.ID + "/" + index + "/" + Name.StandardUrl();
        }


        public static string GetLinkSubCategory(this ProductSubCategory entity, int index = 1)
        {
            string Name = entity.GetUrl();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return "/subcategory/" + entity.ID + "/" + index + "/" + Name.StandardUrl();
        }

        public static string GetLinkCategoryShowSub(this ProductCategory entity)
        {
            string Name = entity.GetUrl();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return "/productsubcategory/" + entity.ID + "/" + Name.StandardUrl();
        }

        public static string GetLinkCategoryShowproduct(this ProductCategory entity)
        {
            string Name = entity.GetUrl();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return "/ptcategory/" + entity.ID + "/" + Name.StandardUrl();
        }

        public static string GetPageTitle(this ProductCategory entity)
        {
            string Name = entity.GetTitlePage();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return Name;
        }

        public static string GetPageTitle(this ProductType entity)
        {
            string Name = entity.GetTitle();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return Name;
        }



        public static string GetPageTitle(this ProductSubCategory entity)
        {
            string Name = entity.GetTitle();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return Name;
        }

        public static string GetMetaDescription(this ProductCategory entity)
        {
            string Name = entity.GetMetaDescriptionLang();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return Name;
        }
        //public static string GetDescription(this ProductCategory entity)
        //{
        //    string currentLang = lang == Enum_Lang.NONE ? GetCurrentLanguage().ToString() : lang.ToString();
        //    if (currentLang.ToUpper() == Enum_Lang.FA.ToString())
        //        return productSubCategory.Body;
        //    else
        //    {
        //        ProductSubCategoryLanguage temp = productSubCategory.ProductSubCategoryLanguage.FirstOrDefault(p => p.Language.Label == currentLang);
        //        return temp != null ? temp.body : productSubCategory.Body;
        //    }
        //}


        public static string GetMetaDescription(this ProductSubCategory entity)
        {
            string Name = entity.GetDescription();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return Name;
        }
        public static string GetMetaDescription(this ProductType entity)
        {
            string Name = entity.GetDescription();
            if (Name == null)
            {
                Name = entity.GetName();
            }
            return Name;
        }


        public static string GetLink(this ProductSubCategory entity, int index = 1)
        {
            return "/ps/" + entity.ID + "/" + index + "/" + entity.GetName().StandardUrl();
        }


        public static string GetLink(this Enum_Lang lang)
        {
            return "/language/index/" + lang.ToString().ToLower();
        }

        private static string GetSearchLink(ProductType type, ProductCategory category, ProductSubCategory subCategory, int index, string name)
        {
            string link = "/search";
            link += "/" + (type == null ? "0" : type.ID.ToString());
            link += "/" + (category == null ? "0" : category.ID.ToString());
            link += "/" + (subCategory == null ? "0" : subCategory.ID.ToString());
            link += "/" + index + "/";
            link += name.StandardUrl();
            return link;
        }

        public static string StandardUrl(this string url)
        {
            if (string.IsNullOrEmpty(url) == true)
                return url;
            url = url.Replace(" ", "-");
            url = url.Replace("%", "-");
            url = url.Replace(".", "-");
            url = url.Replace("&", "-");
            url = url.Replace("+", "-");
            url = url.Replace("?", "-");
            url = url.Replace("*", "-");
            url = url.Replace("/", "-");
            url = url.Replace("'", "-");
            url = url.Replace("(", "-");
            url = url.Replace(")", "-");
            return url.ToLower();
        }

        public static string GetShareUrl(this Enum_Social social, string url = null, string text = null)
        {
            url = url == null ? HttpContext.Current.Request.Url.ToString() : url;
            text = text == null ? "" : text;
            switch (social)
            {
                case Enum_Social.TELEGRAM:
                    url = "https://telegram.me/share/url?url=" + url + "&text=" + text;
                    break;
                case Enum_Social.FACEBOOK:
                    url = "https://www.facebook.com/sharer/sharer.php?u=" + url;
                    break;
                case Enum_Social.INSTAGRAM:
                    url = "" + url;
                    break;
                case Enum_Social.TWITTER:
                    url = "https://twitter.com/home?status=" + url;
                    break;
                case Enum_Social.WHATSAPP:
                    url = "https://api.whatsapp.com/send?text=" + url;
                    break;
                case Enum_Social.LINKEDIN:
                    url = "https://www.linkedin.com/shareArticle?mini=true&url=" + url;
                    break;
                case Enum_Social.PINTEREST:
                    url = "" + url;
                    break;
                case Enum_Social.EMAIL:
                    url = "" + url;
                    break;
                case Enum_Social.GOOGLE_PLUS:
                    url = "https://plus.google.com/share?url=" + url;
                    break;
                default:
                    break;
            }
            return url;
        }

        public static string GetShortLink(string link)
        {
            Hashids hashIds = new Hashids();
            string link1 = hashIds.Encode(1);
            string link2 = hashIds.EncodeHex(link);
            string link3 = hashIds.EncodeLong(1);

            int[] decodeValue = hashIds.Decode(link1);
            return link1;
        }


    }
}
