using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using Cms.Areas.Store.Security;
using OfficeOpenXml;

namespace Cms.Areas.Store.Controllers
{
    public class ProductsController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        // GET: Store/Products
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Index(int index = 1, int pageSize = 10, string name = null)
        {

            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var storeProduct = _context.StoreProduct.Search(statusId: null, index: index, pageSize: pageSize, name: name, accountId: currentAccount.Id);
            // ViewBag.TotalCount = _context.StoreProduct.Search(statusId: null, index: null, pageSize: null, name: name, accountId: currentAccount.Id).Count();
            ViewBag.TotalCount = _context.StoreProduct.SearchCount(statusId: null, name: name, accountId: currentAccount.Id);
            ViewBag.pageSize = pageSize.ToString();
            return View(storeProduct.ToView());
        }
        [HttpPost]
        public ActionResult UpdatePrice(bool percent, bool increase, double changeAmount, int groupPricingType, int? productTypeId = null, int? productCategoryId = null, int? productSubCategoryId = null, int? productId = null, int? quantityId = null)
        {
            if (changeAmount <= 0 || groupPricingType < 0 || groupPricingType > 5)
                return RedirectToAction("Index");

            double GetFinalPrice(StoreProductQuantity spq)
            {
                var changePrice = percent ? spq.Price * changeAmount / 100 : changeAmount;
                var price = increase ? spq.Price + changePrice : spq.Price - changePrice;
                return price > 0 ? Math.Round(price, MidpointRounding.AwayFromZero) : 0;
            }

            var storeProductQuantities = GetStoreProductQuantities();
            var targetList = new List<StoreProductQuantity>();

            switch (groupPricingType)
            {
                case 0:// all
                    targetList = storeProductQuantities.Where(x => x.StoreProduct.Product.IsAdvertising == true).ToList();
                    break;

                
            }

            if (targetList.Any())
            {
                foreach (var spq in targetList)
                {
                    spq.Price = GetFinalPrice(spq);
                    _context.StoreProductQuantity.Update(spq);
                }
                _context.Save();
                var products = targetList.Select(s => s.ProductQuantity.Product).Distinct().ToList();
                BaseStore.UpdateProductQuantity(products: products, unitOfWork: _context);
            }

            return RedirectToAction("Index");
        }
        public List<StoreProductQuantity> GetStoreProductQuantities()
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            return _context.StoreProductQuantity.GetForStore(currentAccount.StoreId);
        }

        public void DownloadExcel()
        {
            var dataResult = new List<StoreProductQuantity>();
            var currentAccount = BaseWebsite.CurrentAccount;
            var storeProducts = _context.StoreProduct.Search(statusId: null, index: 1, pageSize: int.MaxValue, name: null, accountId: currentAccount.Id);
            foreach (var storeProductQuantities in storeProducts
                .Select(storeProduct => storeProduct.StoreProductQuantity)
                .Where(storeProductQuantities => storeProductQuantities.Any()))
                dataResult.AddRange(storeProductQuantities);

            if (!dataResult.Any()) return;

            var excelPackage = new ExcelPackage();
            var sheet = excelPackage.Workbook.Worksheets.Add(Resource.Lang.StoreProducts);

            var row = 1;

            sheet.Cells[$"A{row}"].Value = Resource.Lang.ID;
            sheet.Cells[$"B{row}"].Value = Resource.Lang.ProductName;
            sheet.Cells[$"C{row}"].Value = Resource.Lang.Color;
            sheet.Cells[$"D{row}"].Value = Resource.Lang.Size;
            sheet.Cells[$"E{row}"].Value = Resource.Lang.ProductOption;
            sheet.Cells[$"F{row}"].Value = $"{Resource.Lang.Price} ({Resource.Lang.Toman})";
            sheet.Cells[$"G{row}"].Value = Resource.Lang.Count;

            foreach (var spq in dataResult)
            {
                row++;
                sheet.Cells[$"A{row}"].Value = spq.ID;
                sheet.Cells[$"B{row}"].Value = spq.ProductQuantity.Product.GetName();
                sheet.Cells[$"C{row}"].Value = spq.ProductQuantity.Color?.GetName();
                sheet.Cells[$"D{row}"].Value = spq.ProductQuantity.Size?.GetName();
                sheet.Cells[$"E{row}"].Value = spq.ProductQuantity.OptionItemId.HasValue
                    ? $"{spq.ProductQuantity.ProductOptionItem.ProductOption.Name} : {spq.ProductQuantity.ProductOptionItem.Value}"
                    : null;
                sheet.Cells[$"F{row}"].Value = spq.Price;
                sheet.Cells[$"G{row}"].Value = spq.Count;
            }

            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=StoreProducts.xlsx");
            Response.BinaryWrite(excelPackage.GetAsByteArray());
            Response.End();
        }


        // GET: Store/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!BaseStore.StoreProductId_IsValid(id.Value, _context))
                return View("Invalid");

            StoreProduct storeProduct = _context.StoreProduct.FirstOrDefault(s => s.ID == id);
            if (storeProduct == null)
            {
                return HttpNotFound();
            }
            return View(storeProduct);
        }

        // GET: Store/Products/Create
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]

        public ActionResult Create()
        {

            return View();
        }

        // POST: Store/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]
        public ActionResult Create(List<int> products)
        {

            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;

            var list = new List<StoreProduct>();
            foreach (var productId in products)
            {
                var item = new StoreProduct()
                {
                    ProductId = productId,
                    StoreId = currentAccount.StoreId,
                    CreateDate = DateTime.Now,
                    StatusId = _context.Code.GetIdByLabel(DataLayer.Enumarables.Enum_Code.STOREPRODUCT_REQUEST_NOT_CHECKED)
                };
                list.Add(item);
            }
            _context.StoreProduct.MultipleInsert(list);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!BaseStore.StoreProductId_IsValid(id.Value, _context))
                return View("Invalid");
            StoreProduct storeProduct = _context.StoreProduct.FirstOrDefault(s => s.ID == id);
            if (storeProduct == null)
            {
                return HttpNotFound();
            }
            return View(storeProduct);
        }

        // POST: Store/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!BaseStore.StoreProductId_IsValid(id, _context))
                return View("Invalid");
            _context.StoreProduct.Delete(id);
            _context.Save();
            return RedirectToAction("Index");
        }

        public ActionResult ProductQuantity(int id)
        {
            if (!BaseStore.StoreProductId_IsValid(id, _context))
                return View("Invalid");
            var model = _context.StoreProduct.FirstOrDefault(s => s.ID == id);
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ProductQuantity(StoreProductQuantity storeProductQuantity)
        {

            if (!BaseStore.StoreProductId_IsValid(storeProductQuantity.StoreProductId, _context))
                return View("Invalid");
            _context.StoreProductQuantity.AddOrEdit(storeProductQuantity, _context);
            //var model = _context.StoreProduct.FirstOrDefault(s => s.ID == storeProductQuantity.StoreProduct.ID);
            return RedirectToAction("ProductQuantity", storeProductQuantity.StoreProductId);
        }

        public ActionResult PriceRange(int storeProductQuantityId)
        {
            var model = _context.StoreProductQuantity.GetById(storeProductQuantityId);
            if (model == null || !BaseStore.StoreProductId_IsValid(model.StoreProductId, _context))
                return View("Invalid");
            ViewBag.PriceList = model.PriceRange.ToList();
            return View(new PriceRange() { StoreProductQuantityId = model.ID });
        }
        [HttpPost]
        public ActionResult PriceRange(PriceRange priceRange)
        {
            var model = _context.StoreProductQuantity.GetById(priceRange.StoreProductQuantityId);
            if (model == null || !BaseStore.StoreProductId_IsValid(model.StoreProductId, _context))
                return View("Invalid");
            _context.PriceRange.AddRange(priceRange);
            model = _context.StoreProductQuantity.GetById(priceRange.StoreProductQuantityId);
            ViewBag.PriceList = model.PriceRange.ToList();
            return Redirect("/store/products/PriceRange?storeProductQuantityId=" + priceRange.StoreProductQuantityId);
        }
        public ActionResult PriceRangeDelete(int id)
        {
            var model = _context.PriceRange.GetById(id);
            if (model != null)
            {
                _context.PriceRange.Delete(model);
                _context.Save();
            }
            return Redirect("/store/products/PriceRange?storeProductQuantityId=" + model.StoreProductQuantityId);

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
