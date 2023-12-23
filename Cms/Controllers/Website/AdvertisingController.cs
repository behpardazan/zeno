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
    public class AdvertisingController : Controller
    {
        UnitOfWork _context = new UnitOfWork();


        public ActionResult Index(int prid, string pridname = "")
        {
            string currentUrl = Request.Url.ToString()/*.Replace("http:", "https:")*/.ToLower();
            Product product = _context.Product.GetById(prid);
            if (currentUrl != product.GetLinkAddvertising())
            {

                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", product.GetLinkAddvertising());
            }
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

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, product);
        }

        public ActionResult Search(
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
            Enum_ProductOrder prorder = Enum_ProductOrder.LADDER,
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
            string smartOffer=null
            )
        {
            string currentUrl = Request.Url.ToString().ToLower();
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
                smartOffer:smartOffer,
                isPublish:true,
                isAdvertising:true
                );
            List<Product> results = _context.Product.Search(
             pageSize: prpageSize.Value,
             index: prindex.Value,
            title: prtitle,
            name: prname,
            order: prorder,
            query: searchQuery,
            prstock: prstock);
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
                        search.Type = Enum_SearchType.PRODUCTSUBCATEGORY;
                        search.ProductCategory = _context.ProductCategory.GetById(search.ProductSubCategory.CategoryId);
                        search.ProductType = _context.ProductType.GetById(search.ProductSubCategory.ProductCategory.ProductType.ID);
                    }
                    else if (search.ProductCategory != null)
                    {
                        search.Type = Enum_SearchType.PRODUCTCATEGORY;
                        search.ProductType = _context.ProductType.GetById(search.ProductCategory.ProductType.ID);
                    }
                    else if (search.ProductType != null)
                    {
                        search.Type = Enum_SearchType.PRODUCTTYPE;
                        search.ProductCategory = null;
                        search.ProductSubCategory = null;
                    }

                }
               
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
            return BaseController.GetView(this, prresultType, prviewName, model);
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