using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using static QRCoder.PayloadGenerator;

namespace Cms.Controllers
{
    public class ProductController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult IndexNew(string prfullurl)
        {
            int prid = prfullurl.Split('-')[0].GetInteger();
            Product product = _context.Product.GetById(prid);
            if (product == null || product.Active == false)
                return Redirect("/error/404");

            product.VisitCount = product.VisitCount + 1;
            _context.Product.Update(product);
            _context.Save();
            var account = BaseWebsite.CurrentAccount;
            if (account != null)
            {
                IEnumerable<ProductVisit> pvList = _context.ProductVisit.Where(x => x.AccountId == account.Id && x.ProductId == product.ID);
                foreach (var item in pvList)
                {
                    _context.ProductVisit.Delete(item);
                }
                ProductVisit productVisit = new ProductVisit();
                productVisit.DateVisit = DateTime.Now;
                productVisit.ProductId = product.ID;
                productVisit.AccountId = account.Id;
                _context.ProductVisit.Insert(productVisit);
                _context.Save();
            }
            ViewAccount CurrentAccount = _context.Account.GetCurrentAccount();
            ViewBag.CurrentAccount = CurrentAccount;

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "Index", product);
        }

        public ActionResult Index(int prid, string pridname = "")
        {
            string currentUrl = Request.Url.ToString()/*.Replace("http:", "https:")*/.ToLower();
            Product product = _context.Product.GetById(prid);
            if (currentUrl != product.GetLink())
            {

                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", product.GetLink());
            }
            if (product == null/* || product.Active == false*/)
                return Redirect("/error/404");

            product.VisitCount = product.VisitCount + 1;
            _context.Product.Update(product);
            _context.Save();
            var account = BaseWebsite.CurrentAccount;
            if (account != null)
            {
                IEnumerable<ProductVisit> pvList = _context.ProductVisit.Where(x => x.AccountId == account.Id && x.ProductId == product.ID);
                foreach (var item in pvList)
                {
                    _context.ProductVisit.Delete(item);
                }
                ProductVisit productVisit = new ProductVisit();
                productVisit.DateVisit = DateTime.Now;
                productVisit.ProductId = product.ID;
                productVisit.AccountId = account.Id;
                _context.ProductVisit.Insert(productVisit);
                _context.Save();
            }
            ViewAccount CurrentAccount = _context.Account.GetCurrentAccount();
            ViewBag.CurrentAccount = CurrentAccount;

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, product);
        }
        [OutputCache(Duration = 1800, VaryByParam = "*")]

        public ActionResult IndexPicture(int id, Product model)
        {
            return PartialView(BaseController.GetView(this), model: model);

        }
        [OutputCache(Duration = 1800, VaryByParam = "*")]

        public ActionResult IndexDetail(int id, Product model)
        {
            return PartialView(BaseController.GetView(this), model: model);

        }
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Search(
            string prIds = null,
            string prtypeLabel = null,
            string prcategoryLabel = null,
            int? prnotId = null,
            string groupIds = null,
            string prtypeId = null,
            string prcategoryId = null,
            string prsubCategoryId = null,
            string prshopId = null,
            int? prindex = null,
            int? prpageSize = null,
            int? prpricefrom = null,
            int? prpriceto = null,
            string prbrandId = null,
            string prstoreId = null,
            string prcompetitiveId = null,
            string prcolorId = null,
            string prsizeId = null,
            string prname = null,
            string prtitle = null,
            string prdiscount = null,
            string prviewName = null,
            string prcustom = null,
            string proptions = null,
            string practive = "true",
            bool? IsSpecialProduct = null,
            bool? prshowhome = null,
            string Summary = null,
            bool? prIsService = null,
            string Value = null,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            Enum_ProductOrder prorder = Enum_ProductOrder.MORE_VISIT,
            Enum_ProductOutput proutput = Enum_ProductOutput.ENTITY,
            Enum_ResultType prresultType = Enum_ResultType.RESULT_TYPE_VIEWNAME,
            string prstock = "all",
            string prcustomLabel = "all",
            bool activeLocation = false,
            string prstateId = null,
            string prcityId = null,
            string prFromDate = null,
            string prToDate = null,
            bool? haveDate = null,
            string smartOffer = null,
            bool? isPublish = null
            )
        {
            string currentUrl = Request.Url.ToString().ToLower();
            if (currentUrl.Contains("/pt/"))
            {
                string url = _context.ProductType.GetById(int.Parse(prtypeId)).GetUrlNew();
                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", url);
            }
            if (currentUrl.Contains("/pc/"))
            {
                string url = _context.ProductCategory.GetById(int.Parse(prcategoryId)).GetUrlNew();
                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", url);
            }
            if (currentUrl.Contains("/ps/"))
            {
                string url = _context.ProductSubCategory.GetById(int.Parse(prsubCategoryId)).GetUrlNew();
                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", url);
            }
            var model = SearchContent(
                prIds, prnotId, groupIds, prtypeId,
                prcategoryId, prsubCategoryId, prshopId,
                prindex, prpageSize, prpricefrom, prpriceto,
                prbrandId, prstoreId, prcompetitiveId,
                prcolorId, prsizeId, prname, prtitle,
                prdiscount, prviewName, prcustom, proptions,
                practive, prshowhome, Summary, prIsService, Value,
                SyncNameProductCustomField, valueProductCustom,
                prorder, proutput, prresultType, prstock, prcustomLabel,
                activeLocation, prstateId, prcityId,
                prFromDate, prToDate, haveDate, smartOffer, isPublish, IsSpecialProduct
                );
            return BaseController.GetView(this, prresultType, prviewName, model);

        }
        [OutputCache(Duration = 1800, VaryByParam = "*")]
        public ActionResult SearchPartial(
            string prIds = null,
            int? prnotId = null,
            string groupIds = null,
            string prtypeId = null,
            string prcategoryId = null,
            string prsubCategoryId = null,
            string prshopId = null,
            int? prindex = null,
            int? prpageSize = null,
            int? prpricefrom = null,
            int? prpriceto = null,
            string prbrandId = null,
            string prstoreId = null,
            string prcompetitiveId = null,
            string prcolorId = null,
            string prsizeId = null,
            string prname = null,
            string prtitle = null,
            string prdiscount = null,
            string prviewName = null,
            string prcustom = null,
            string proptions = null,
            string practive = "true",
            bool? prshowhome = null,
            string Summary = null,
            bool? prIsService = null,
            string Value = null,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            Enum_ProductOrder prorder = Enum_ProductOrder.NEW,
            Enum_ProductOutput proutput = Enum_ProductOutput.ENTITY,
            Enum_ResultType prresultType = Enum_ResultType.RESULT_TYPE_VIEWNAME,
            string prstock = "all",
            string prcustomLabel = "all",
            bool activeLocation = false,
            string prstateId = null,
            string prcityId = null,
            string prFromDate = null,
            string prToDate = null,
            bool? haveDate = null,
            string smartOffer = null,
            bool? isPublish = null,
            bool? hasdiscount = null
            )
        {

            var model = SearchContent(
                prIds, prnotId, groupIds, prtypeId,

                prcategoryId, prsubCategoryId, prshopId,
                prindex, prpageSize, prpricefrom, prpriceto,
                prbrandId, prstoreId, prcompetitiveId,
                prcolorId, prsizeId, prname, prtitle,
                prdiscount, prviewName, prcustom, proptions,
                practive, prshowhome, Summary, prIsService, Value,
                SyncNameProductCustomField, valueProductCustom,
                prorder, proutput, prresultType, prstock, prcustomLabel,
                activeLocation, prstateId, prcityId,
                prFromDate, prToDate, haveDate, smartOffer, isPublish, hasdiscount
                );
            return BaseController.GetView(this, prresultType, prviewName, model);

        }
        private object SearchContent(
            string prIds = null,
            int? prnotId = null,
            string groupIds = null,
            string prtypeId = null,
            string prcategoryId = null,
            string prsubCategoryId = null,
            string prshopId = null,
            int? prindex = null,
            int? prpageSize = null,
            int? prpricefrom = null,
            int? prpriceto = null,
            string prbrandId = null,
            string prstoreId = null,
            string prcompetitiveId = null,
            string prcolorId = null,
            string prsizeId = null,
            string prname = null,
            string prtitle = null,
            string prdiscount = null,
            string prviewName = null,
            string prcustom = null,
            string proptions = null,
            string practive = "true",
            bool? prshowhome = null,
            string Summary = null,
            bool? prIsService = null,
            string Value = null,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            Enum_ProductOrder prorder = Enum_ProductOrder.MORE_VISIT,
            Enum_ProductOutput proutput = Enum_ProductOutput.ENTITY,
            Enum_ResultType prresultType = Enum_ResultType.RESULT_TYPE_VIEWNAME,
            string prstock = "all",
            string prcustomLabel = "all",
            bool activeLocation = false,
            string prstateId = null,
            string prcityId = null,
            string prFromDate = null,
            string prToDate = null,
            bool? haveDate = null,
            string smartOffer = null,
            bool? isPublish = null,
            bool? hasdiscount = null,
             bool? IsSpecialProduct = null
            
            )
        {

            DateTime fromDate;
            DateTime toDate;
            try
            {
                fromDate = string.IsNullOrEmpty(prFromDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(prFromDate))).GetValueOrDefault();
            }
            catch
            {
                fromDate = default(DateTime);
            }
            try
            {
                toDate = string.IsNullOrEmpty(prToDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(prToDate))).GetValueOrDefault();
            }
            catch
            {
                toDate = default(DateTime);
            }
            if (!IsNullOrEmpty(smartOffer))
            {
                int? accountId = null;
                ViewAccount tempAccount = _context.Account.GetCurrentAccount();
                if (tempAccount != null)
                {
                    accountId = tempAccount.Id;
                }
                _context.SmartOffer.IncreaseView(accountId);
            }
            prpageSize = prpageSize == null ? 48 : prpageSize;
            prindex = prindex == null ? 1 : prindex;
            prviewName = prviewName == null ? "Search" : prviewName;
            prname = prname == null ? null : prname.GetEnglish();
            //prname = prname == null ? null : prname.ToPersianChar();
            var searchQuery = _context.Product.GetSearchQuery(
                Id: prIds,
                notId: prnotId,
                typeId: prtypeId,
                categoryId: prcategoryId,
                subCategoryId: prsubCategoryId,
                shopId: prshopId,
                brandId: prbrandId,
                storeId: prstoreId,
                competitiveId: prcompetitiveId,
                colorId: prcolorId,
                sizeId: prsizeId,
                discount: prdiscount,
                pricefrom: prpricefrom,
                isSpecialProduct: IsSpecialProduct,
                priceto: prpriceto,
                custom: prcustom,
                option: proptions,
                active: practive,
                name: prname,
                showHome: prshowhome,
                Summary: Summary,
                isService: prIsService,
                SyncNameProductCustomField: SyncNameProductCustomField,
                valueProductCustom: valueProductCustom,
                prstock: prstock,
                prcustomLabel: prcustomLabel,
                activeLocation: activeLocation,
                cityId: prcityId,
                stateId: prstateId,
                fromDate: fromDate,
                toDate: toDate,
                haveDate: haveDate,
                smartOffer: smartOffer,
                isPublish: isPublish,
                hasdiscount: hasdiscount
                );
            List<Product> results = _context.Product.Search(
             pageSize: prpageSize.Value,
             index: prindex.Value,
            title: prtitle,
            name: prname,
            order: prorder,
            query: searchQuery,
            prstock: prstock, smartOffer: smartOffer,
            isSpecialProduct: IsSpecialProduct);
            object model = null;
            foreach (var item in results)
            {
                item.Url = item.GetLinkWithUrl();
            }
            if (proutput == Enum_ProductOutput.SEARCH)
            {
                ViewBag.CurrentAccount = _context.Account.GetCurrentAccount();
                ViewSearchProduct search = new ViewSearchProduct()
                {
                    TotalCount = _context.Product.SearchDetail(query: searchQuery, activeLocation: activeLocation).Count,
                    PageIndex = prindex.Value,
                    PageSize = prpageSize.Value,
                    Results = results
                };
                if (IsNullOrEmpty(prname) == false)
                {
                    search.Type = Enum_SearchType.NAME;
                    search.Name = prname;
                }
                //if (!string.IsNullOrEmpty(groupIds))
                //{
                //    var typeList = groupIds.Split('-').ToList();
                //    if (typeList.Count() == 3)
                //    {
                //        prtypeId = typeList[0] != "0" ? typeList[0] : null;
                //        prcategoryId = typeList[1] != "0" ? typeList[1] : null;
                //        prsubCategoryId = typeList[2] != "0" ? typeList[2] : null;

                //    }
                //}
                if (IsNullOrEmpty(prbrandId) == false)
                {
                    search.Type = Enum_SearchType.PRODUCTBRAND;
                    if (prbrandId.Contains("-") == false)
                    {
                        search.ProductBrand = _context.ProductBrand.GetById(prbrandId.GetInteger());
                        if (search.ProductBrand == null)
                            return Redirect("/error/404");
                    }
                }
                if (IsNullOrEmpty(prstoreId) == false)
                {
                    search.Type = Enum_SearchType.PRODUCTBRAND;
                    if (prstoreId.Contains("-") == false)
                    {
                        search.Store = _context.Store.GetById(prstoreId.GetInteger());
                        if (search.Store == null)
                            return Redirect("/error/404");
                    }
                }
                if (IsNullOrEmpty(prtypeId) == false)
                {
                    search.Type = Enum_SearchType.PRODUCTTYPE;
                    if (prtypeId.Contains("-") == false)
                    {
                        search.ProductType = _context.ProductType.GetById(prtypeId.GetInteger());
                        if (search.ProductType == null)
                            return Redirect("/error/404");
                    }
                }
                if (IsNullOrEmpty(prcategoryId) == false)
                {
                    search.Type = Enum_SearchType.PRODUCTCATEGORY;
                    if (prcategoryId.Contains("-") == false)
                    {
                        search.ProductCategory = _context.ProductCategory.GetById(prcategoryId.GetInteger());
                        if (search.ProductCategory == null)
                            return Redirect("/error/404");
                    }
                }
                if (IsNullOrEmpty(prsubCategoryId) == false)
                {
                    search.Type = Enum_SearchType.PRODUCTSUBCATEGORY;
                    if (prsubCategoryId.Contains("-") == false)
                    {
                        search.ProductSubCategory = _context.ProductSubCategory.GetById(prsubCategoryId.GetInteger());
                        if (search.ProductSubCategory == null)
                            return Redirect("/error/404");
                    }
                }
                if (!string.IsNullOrEmpty(prcustomLabel) && prcustomLabel != "all" && IsNullOrEmpty(prsubCategoryId) == true && IsNullOrEmpty(prcategoryId) == true && IsNullOrEmpty(prtypeId) == true)
                {
                    search.ProductSubCategory = _context.ProductSubCategory.Where(s => s.CustomLabel == prcustomLabel).FirstOrDefault();
                    search.ProductCategory = _context.ProductCategory.Where(s => s.CustomLabel == prcustomLabel).FirstOrDefault();
                    search.ProductType = _context.ProductType.Where(s => s.CustomLabel == prcustomLabel).FirstOrDefault();
                    if (search.ProductSubCategory != null)
                    {
                        if (search.ProductSubCategory.Deleted == true)
                        {
                            HttpContext.Response.Status = "410 Gone";
                            HttpContext.Response.AddHeader("Location", Request.Url.ToString());
                            HttpContext.Response.Redirect(BaseWebsite.ShopUrl+"/error/410");
                        }
                        else
                        {
                            search.Type = Enum_SearchType.PRODUCTSUBCATEGORY;
                            search.ProductCategory = _context.ProductCategory.GetById(search.ProductSubCategory.CategoryId);
                            search.ProductType = _context.ProductType.GetById(search.ProductSubCategory.ProductCategory.ProductType.ID);

                        }
                    }
                    else if (search.ProductCategory != null)
                    {
                        if (search.ProductCategory.Deleted == true)
                        {
                            HttpContext.Response.Status = "410 Gone";
                            HttpContext.Response.AddHeader("Location", Request.Url.ToString());
                            HttpContext.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        }
                        else
                        {
                            search.Type = Enum_SearchType.PRODUCTCATEGORY;
                            search.ProductType = _context.ProductType.GetById(search.ProductCategory.ProductType.ID);

                        }
                    }
                    else if (search.ProductType != null)
                    {
                        if (search.ProductType.Deleted == true)
                        {
                            HttpContext.Response.Status = "410 Gone";
                            HttpContext.Response.AddHeader("Location", Request.Url.ToString());
                            HttpContext.Response.Redirect(BaseWebsite.ShopUrl + "/error/410");
                        }
                        else
                        {
                            search.Type = Enum_SearchType.PRODUCTTYPE;
                            search.ProductCategory = null;
                            search.ProductSubCategory = null;
                        }

                    }

                }
                //else
                //{
                //    if (search.ProductSubCategory != null)
                //    {
                //        groupIds = string.Format("{0}-{1}-{2}", search.ProductSubCategory.ProductCategory.TypeId, search.ProductSubCategory.CategoryId, search.ProductSubCategory.ID);
                //    }
                //    else if (search.ProductCategory != null)
                //    {
                //        groupIds = string.Format("{0}-{1}-{2}", search.ProductCategory.TypeId, search.ProductCategory.ID, 0);
                //    }
                //    else if (search.ProductType != null)
                //    {
                //        groupIds = string.Format("{0}-{1}-{2}", search.ProductType.ID, 0, 0);
                //    }
                //}
                search.GroupIds = groupIds;
                search.MaxPrice = search.Results.Max(s => s.Price);
                model = search;
            }
            else if (proutput == Enum_ProductOutput.ENTITY)
            {
                model = results;
            }
            else if (proutput == Enum_ProductOutput.VIEWMODEL)
            {
                model = results.ToView();
            }
            else if (proutput == Enum_ProductOutput.JSON)
            {
                model = results.ToView();
                return new JsonResult()
                {
                    Data = model,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return model;
        }
        [Route("product/getqr")]
        public ActionResult GetQR(int productId)
        {
            Product product = _context.Product.GetById(productId);
            var generator = new Url($"{/*"https://" + DataLayer.Base.BaseWebsite.WebsiteLabel + ".com" + */product.GetLinkWithUrl()}");
            string payload = generator.ToString();
            var qrGenerator = new QRCoder.QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(payload, QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(6);
            var memoryStream = new System.IO.MemoryStream();
            qrCodeAsBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Position = 0;
            return File(memoryStream, "image/Png");
        }

        private bool IsNullOrEmpty(string value)
        {
            bool result = false;
            if (value == null)
                result = true;
            else if (value == "0")
                result = true;
            else if (value == "")
                result = true;
            else if (value == "null")
                result = true;
            return result;
        }

        public ActionResult Like(int ProductId, string Function)
        {
            Product product = _context.Product.GetById(ProductId);
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            if (CurrentUser != null)
            {
                if (Function == "ADD")
                {
                    ProductLike like = new ProductLike()
                    {
                        AccountId = CurrentUser.Id,
                        ProductId = ProductId,
                        Datetime = DateTime.Now
                    };
                    _context.ProductLike.Insert(like);
                }
                else
                {
                    ProductLike like = _context.ProductLike.GetByProductIdAndAccountId(ProductId, CurrentUser.Id);
                    _context.ProductLike.Delete(like);
                }
                _context.Save();

                return RedirectToAction("index", new { prid = ProductId, prname = BaseUrl.StandardUrl(product.Name) });
            }
            else
            {
                return RedirectToAction("login", "Account", new { back = product.GetLink() });
            }
        }

        public ActionResult Compare(int[] ProductId)
        {
            List<Product> list = new List<Product>();
            if (ProductId != null && ProductId.Any())
            {
                foreach (var item in ProductId)
                {
                    list.Add(_context.Product.GetById(item));
                }
            }
            return View(BaseController.GetView(this), list);
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