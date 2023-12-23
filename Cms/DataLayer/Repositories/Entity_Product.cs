using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DataLayer.Api;
using System.Device.Location;
using static DataLayer.ViewModels.ViewProduct;
using System.Security.Cryptography.X509Certificates;

namespace DataLayer.Repositories
{
    public class Entity_Product : BaseRepository<Product>
    {
        private DatabaseEntities _context;
        public Entity_Product(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        //public Product GetByName(string name)
        //{
        //    return _context.Product.FirstOrDefault(p => p.Name == name);
        //}
        public ViewProductDetail SearchDetail(string Id = null, int? notId = null, string typeId = null, string categoryId = null, int? userId = null, string subCategoryId = null, string brandId = null, string colorId = null, string sizeId = null, string shopId = null, string name = null, string discount = null, int? pricefrom = null, int? priceto = null, string custom = null, string active = "true", bool? showHome = null, bool? isService = null, string latitude = null, string longitude = null, int distance = 0, Expression<Func<Product, bool>> query = null, bool activeLocation = true, string storeId = null, string prstock = "ALL", string option = null, string codeValue = null, int? accountId = null, bool? isAdvertising = null, bool? isPublish = true, bool? isSpecialProduct = null)
        {
            var result = new ViewProductDetail();
            if (query == null)
            {
                query = GetSearchQuery(
                                        Id: Id,
                                        notId: notId,
                                        categoryId: categoryId,
                                        discount: discount,
                                        name: name,
                                        pricefrom: pricefrom,
                                        priceto: priceto,
                                        brandId: brandId,
                                        colorId: colorId,
                                        sizeId: sizeId,
                                        shopId: shopId,
                                        subCategoryId: subCategoryId,
                                        typeId: typeId,
                                        custom: custom,
                                        active: active,
                                        showHome: showHome,
                                        isService: isService,
                                        latitude: latitude,
                                        longitude: longitude,
                                        distance: distance,
                                        storeId: storeId,
                                        option: option,
                                        prstock: prstock,
                                        isSpecialProduct:isSpecialProduct,
                                        activeLocation: activeLocation, codeValue: codeValue, accountId: accountId, isAdvertising: isAdvertising, isPublish: isPublish);
            }
            IEnumerable<Product> q = _context.Product.Where(query);

            if (distance > 0 && (!string.IsNullOrEmpty(latitude)) && (!string.IsNullOrEmpty(longitude)))
            {
                q = from s in q
                    where TrueDistance(s.Latitude, s.Longitude, latitude, longitude, distance) == true
                    select s;
            }
            if (q.Any())
            {
                result.Count = q.Count();

                try
                {
                    result.MinPrice = q.Min(s => s.MinPrice);
                    result.MaxPrice = q.Max(s => s.Price);
                }
                catch
                {

                }

            }
            return result;
        }
        public void UpdateAllProductPrice(UnitOfWork unitOfWork)
        {
            if (BaseWebsite.WebsiteSetting.HasUnitPrice)
            {
                var goldPrice = Base.BaseStore.GetGoldPrice();
                if (goldPrice > 0)
                {
                    unitOfWork.ShopWebsiteSetting.FirstOrDefault().UnitPrice = goldPrice;
                    unitOfWork.Save();
                    List<Product> productList = Where(x => x.Active && (!x.Deleted)).ToList();

                    double computeprice = ComputeFee();
                    foreach (var item in productList)
                    {
                        UpdateProductPriceBaseUnitPrice(item, computeprice, unitOfWork);
                    }
                }
            }
        }

        public double ComputeFee()
        {

            double unitPrice = Base.BaseWebsite.WebsiteSetting.UnitPrice;
            double computeprice = (unitPrice + ((unitPrice * 7) / 100));
            computeprice = ((computeprice * 9) / 100) + computeprice;
            return computeprice;
        }
        public void UpdateProductPriceBaseUnitPrice(Product item, double computeprice, UnitOfWork unitOfWork)
        {
            double fee = 0;
            if (item.ProductType.ProductCustomField.Where(x => x.SyncName == "Fee").Any())
            {
                int ID = item.ProductType.ProductCustomField.FirstOrDefault(x => x.SyncName == "Fee").ID;
                var checkCustomValue = item.ProductCustomValue.FirstOrDefault(x => x.FieldId == ID);
                if (checkCustomValue != null)
                {
                    if (checkCustomValue.Value != null)
                    {
                        fee = double.Parse(checkCustomValue.Value);
                    }
                }
            }
            double computeprice1 = ((computeprice * fee) / 100) + computeprice;
            double price = Math.Round((computeprice1 * item.Weight), 0);

            item.ProductQuantity.SelectMany(s => s.StoreProductQuantity).ToList().ForEach(s => s.Price = price);
            Save();
            Base.BaseStore.UpdateProductQuantity(item.ID, unitOfWork);
        }

        public void UpdateProductPriceBaseUnitPrice(int id, double computeprice, UnitOfWork unitOfWork)
        {
            var item = FirstOrDefault(x => x.ID == id, includes: "ProductType,ProductCustomValue");
            UpdateProductPriceBaseUnitPrice(item, computeprice, unitOfWork);
        }
        public List<Product> Search(string Id = null, int? notId = null, string typeId = null, string categoryId = null, int? userId = null, string subCategoryId = null, string brandId = null, string companyId = null, string storeId = null, string colorId = null, string sizeId = null, string shopId = null, int index = 1, int pageSize = 10, string name = null, string title = null, string discount = null, Enum_ProductOrder order = Enum_ProductOrder.NONE, int? pricefrom = null, int? priceto = null, string custom = null, bool? isService = null, string active = "true", bool? showHome = null, string latitude = null, string longitude = null, int distance = 0, Expression<Func<Product, bool>> query = null, string prstock = "ALL", bool activeLocation = false, string option = null, string codeValue = null, int? accountId = null, bool? isAdvertising = null, bool? isPublish = true, string smartOffer = null,bool ? isSpecialProduct=null)
        {
            List<Product> results = new List<Product>();
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            if (query == null)
            {
                query = GetSearchQuery(
                                        Id: Id,
                                        notId: notId,
                                        categoryId: categoryId,
                                        discount: discount,
                                        name: name,
                                        pricefrom: pricefrom,
                                        priceto: priceto,
                                        brandId: brandId,
                                        storeId: storeId,
                                        colorId: colorId,
                                        sizeId: sizeId,
                                        shopId: shopId,
                                        subCategoryId: subCategoryId,
                                        typeId: typeId,
                                        custom: custom,
                                        active: active,
                                        option: option,
                                        showHome: showHome,
                                        isService: isService,
                                        latitude: latitude,
                                        longitude: longitude,
                                        distance: distance,
                                        prstock: prstock,
                                        activeLocation: activeLocation,
                                        companyId: companyId,
                                        codeValue: codeValue,
                                        isSpecialProduct: isSpecialProduct,
                                        accountId: accountId, isAdvertising: isAdvertising, isPublish: isPublish
                                        );
            }


            string order_status_post = Enum_Code.ORDER_STATUS_POST.ToString();
            //string order_status_process = Enum_Code.ORDER_STATUS_PROCESS.ToString();
            string order_status_store = Enum_Code.ORDER_STATUS_STORE.ToString();
            string order_status_success = Enum_Code.ORDER_STATUS_SUCCESS.ToString();
            IEnumerable<Product> q = _context.Product.Where(query);
            if (distance > 0 && (!string.IsNullOrEmpty(latitude)) && (!string.IsNullOrEmpty(longitude)))
            {
                q = from s in q
                    where TrueDistance(s.Latitude, s.Longitude, latitude, longitude, distance) == true
                    select s;
            }
            if (IsNullOrEmptyId(smartOffer) == false)
            {
                results = q
                   .OrderByDescending(p => p.SpecialProduct)
                   .ThenByDescending(p => p.VisitCount)
                   .Skip(skipValue)
                   .Take(pageValue)
                   .ToList();
            }
            else if (order == Enum_ProductOrder.NONE)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.NEW)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.UPDATE)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.UpdateDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.UPDATE_DESC)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.UpdateDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.NEW_SYNC)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.SyncDatetime)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.OLD)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.ID)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.RANDOM)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => Guid.NewGuid())
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.PRICE_MAX_TO_MIN)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.MinPrice > 0 ? p.MinPrice : p.Price)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.PRICE_MIN_TO_MAX)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.MinPrice > 0 ? p.MinPrice : p.Price)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.MORE_SELL)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.AccountOrderProduct.Count(qu =>
                        qu.AccountOrder.Code.Label == order_status_post ||
                        //qu.AccountOrder.Code.Label == order_status_process ||
                        qu.AccountOrder.Code.Label == order_status_store ||
                        qu.AccountOrder.Code.Label == order_status_success))
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.LESS_SELL)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.AccountOrderProduct.Count(qu =>
                        qu.AccountOrder.Code.Label == order_status_post ||
                        //qu.AccountOrder.Code.Label == order_status_process ||
                        qu.AccountOrder.Code.Label == order_status_store ||
                        qu.AccountOrder.Code.Label == order_status_success))
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.SHOWNUMBER)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenBy(p => p.ShowNumber)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.SHOWNUMBER_DESC)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ShowNumber)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.MORE_VISIT)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.VisitCount)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.MORE_LIKE)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.ProductLike.Count)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.MORE_Rate)
                results = q
                    .OrderBy(p => p.Code.Value)
                    .ThenByDescending(p => p.GetRate() == null ? 0 : p.GetRate().Value)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            else if (order == Enum_ProductOrder.LADDER)
                results = q
                    .OrderByDescending(p => p.LadderDate)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            return results;
        }

        private static object GetPropertyValue<T>(T x, string v)
        {
            throw new NotImplementedException();
        }

        public Expression<Func<Product, object>> GetOrder(Enum_ProductOrder order = Enum_ProductOrder.NONE)
        {
            Expression<Func<Product, object>> orderByFunc = null;
            if (order == Enum_ProductOrder.NEW)
                orderByFunc = (p => p.ID);
            return orderByFunc;
        }

        public Expression<Func<Product, bool>> GetSearchQuery(
            string Id = null,
            int? notId = null,
            string typeId = null,
            string colorId = null,
            string sizeId = null,
            int? userId = null,
            string categoryId = null,
            string subCategoryId = null,
            string shopId = null,
            int? pricefrom = null,
            int? priceto = null,
            string brandId = null,
            string competitiveId = null,
            string storeId = null,
            string name = null,
            string discount = null,
            string custom = null,
            string option = null,
            string active = "true",
            bool? showHome = null,
            string Summary = null,
            bool? isService = null,
            string latitude = null,
            string longitude = null,
            int distance = 0,
            string SyncNameProductCustomField = null,
            string valueProductCustom = null,
            string prstock = "all",
            string prcustomLabel = "",
            bool activeLocation = false,
            string cityId = null,
            string stateId = null,
            string companyId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
               bool? haveDate = null,
               string codeValue = null,
                string smartOffer = null
                , int? accountId = null
              , bool? isAdvertising = null
             , bool? isPublish = null,
                bool ? isSpecialProduct=null
            )
        {
            if (active != null)
            {
                active = active.ToUpper();

            }


            bool? isActive = null;
            if (active == "TRUE")
                isActive = true;
            else if (active == "FALSE")
                isActive = false;
            var MyQuery = PredicateBuilder.True<Product>();
            MyQuery = MyQuery.And(p => p.Deleted == false);
            //if (isAdvertising!=null)
            MyQuery = MyQuery.And(p => p.IsAdvertising == isAdvertising);
            if (IsNullOrEmptyId(smartOffer) == false)
            {
                prstock = "available";
            }
            if (prstock == "available")
                MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString());
            else if (prstock == "not_available")
                MyQuery = MyQuery.And(p => p.Code.Label == Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString());
            if (Id != null)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = Id.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ID == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (isService == true || isService == false)
                MyQuery = MyQuery.And(p => p.IsService == isService);
            if (isPublish != null)
                MyQuery = MyQuery.And(p => p.IsPublish == isPublish);
            if (isSpecialProduct != null)
                MyQuery = MyQuery.And(p => p.IsSpecialProduct == isSpecialProduct);
            if (accountId != null)
                MyQuery = MyQuery.And(p => p.AccountId == accountId);
            if (!IsNullOrEmptyId(companyId))
            {
                int cpId = int.Parse(companyId);
                MyQuery = MyQuery.And(p => p.CompanyId == cpId);
            }
            if (fromDate != null && fromDate != default(DateTime))
                MyQuery = MyQuery.And(p => p.CreateDateTime >= fromDate);
            if (haveDate == true)
            {
                MyQuery = MyQuery.And(p => p.StartDate <= DateTime.Now);
            }
            if (toDate != null && toDate != default(DateTime))
            {
                DateTime toDatetimeTemp = toDate.Value.AddDays(1);
                MyQuery = MyQuery.And(p => p.CreateDateTime <= toDatetimeTemp);
            }
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (pricefrom != null && pricefrom != 0)
                MyQuery = MyQuery.And(p => p.Price >= pricefrom);
            if (priceto != null && priceto != 0)
                MyQuery = MyQuery.And(p => p.Price <= priceto);
            if (isActive != null)
                MyQuery = MyQuery.And(p => p.Active == isActive);
            if (showHome != null)
                MyQuery = MyQuery.And(p => p.ShowHomePage == showHome);
            if (Summary != null)
                MyQuery = MyQuery.And(p => p.Summary == Summary);

            if (userId != null)
                MyQuery = MyQuery.And(p =>
                    p.Shop.ShopUser.Any(s => s.UserId == userId) ||
                    p.UserId == userId);

            if (IsNullOrEmptyId(discount) == false)
            {
                DateTime now = DateTime.Now;
                string type_shop = Enum_Code.DISCOUNT_TYPE_SHOP.ToString();
                string type_producttype = Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString();
                string type_productbrand = Enum_Code.DISCOUNT_TYPE_BRAND.ToString();
                string type_productcategory = Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString();
                string type_productsubcategory = Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString();
                string type_product = Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString();

                MyQuery = MyQuery.And(s =>
                    s.Discount.Any(p =>
                        (p.Code.Label == type_product) &&
                        (p.ProductId == s.ID) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true)
                    ) ||
                    (
                        s.Shop.Discount.Any(p =>
                        (p.Code.Label == type_shop) &&
                        (p.ShopId == s.ShopId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    ) ||
                    (
                        s.ProductType.Discount.Any(p =>
                        (p.Code.Label == type_producttype) &&
                        (p.ProductTypeId == s.ProductTypeId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    ) ||
                    (
                        s.ProductCategory.Discount.Any(p =>
                        (p.Code.Label == type_productcategory) &&
                        (p.ProductCategoryId == s.ProductCategoryId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    ) ||
                    (
                        s.ProductSubCategory.Discount.Any(p =>
                        (p.Code.Label == type_productsubcategory) &&
                        (p.ProductSubCategoryId == s.ProductSubCategoryId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    ) ||
                    (
                        s.ProductBrand.Discount.Any(p =>
                        (p.Code.Label == type_productbrand) &&
                        (p.ProductBrandId == s.BrandId) &&
                        (p.Active == true) &&
                        (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                        (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now)) &&
                        (p.DiscountGroup.Label == discount && p.DiscountGroup.Active == true))
                    )
                );
            }

            if (IsNullOrEmptyId(typeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = typeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductTypeId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(smartOffer) == false)
            {
                var QueryStr = PredicateBuilder.True<Product>();
                string[] Split = smartOffer.Split('*');
                foreach (var item in Split)
                {

                    if (item.Contains("اسپرسوساز") || item.Contains("موکاپات") || item.Contains("ایروپرس") || item.Contains("مینی پرسو"))
                    {
                        string noSpace = item.Replace(" ", "");
                        QueryStr = QueryStr.And(p =>
                          p.ProductCustomValue.Any(s => /*s.Value.Contains(item) ||*/ s.Value.Contains("اسپرسو") /*|| s.Value.Contains(noSpace)*/) /*&& p.StoreProduct.Any(x => x.Store.IsSmartOffer == true)*/
                       );
                    }
                    else if (item.Contains("قهوه‌ساز") | item.Contains("قهوه ساز") ||  item.Contains("فرنچ پرس") || item.Contains("کمکس") || item.Contains("وی 60") || item.Contains("سایفون") || item.Contains("سرد دم") || item.Contains("کلور") )
                    {
                        string noSpace = item.Replace(" ", "");
                        QueryStr = QueryStr.And(p =>
                          p.ProductCustomValue.Any(s =>/* s.Value.Contains(item) ||*/ s.Value.Contains("دمی")/* || s.Value.Contains(noSpace)*/) /*&& p.StoreProduct.Any(x => x.Store.IsSmartOffer == true)*/
                       );
                    }
                    else
                    {

                        QueryStr = QueryStr.And(p =>
                           p.ProductCustomValue.Any(s => s.Value.Contains(item)) /*&& p.StoreProduct.Any(x => x.Store.IsSmartOffer == true)*/);
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(colorId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = colorId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductColor.Any(a => a.ColorId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(sizeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = sizeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductSize.Any(a => a.SizeId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(categoryId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = categoryId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductCategoryId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(subCategoryId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = subCategoryId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductSubCategoryId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(brandId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = brandId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.BrandId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }


            if (IsNullOrEmptyId(storeId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = storeId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.StoreProduct.Any(s => s.StoreId == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(cityId) == false && IsNullOrEmptyId(stateId) == false)
            {
                int cityitemId = cityId.GetInteger();
                int stateitemId = stateId.GetInteger();
                MyQuery = MyQuery.And(p => p.StoreProduct.Any(s => s.Store.Account.CityId == cityitemId && s.Store.Account.StateId == stateitemId));
            }
            if (IsNullOrEmptyId(competitiveId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = competitiveId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.StoreProduct.Any(s => s.Store.StoreCompetitiveFeature.Any(x => x.CompetitiveFeatureId == itemId) && s.StoreProductQuantity.Count > 0)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (IsNullOrEmptyId(shopId) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = shopId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ShopId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (IsNullOrEmptyId(option) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] Split = option.Split('*');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductQuantity.Any(s => s.ProductOptionItem.ID == itemId)
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }


            //if (IsNullOrEmptyId(prcustomLabel) == false && prcustomLabel != "all")
            //    MyQuery = MyQuery.And(p => p.ProductSubCategory.CustomLabel == prcustomLabel || p.ProductType.CustomLabel == prcustomLabel || p.ProductCategory.CustomLabel == prcustomLabel);
            var setting = BaseWebsite.WebsiteSetting;
            if (IsNullOrEmptyId(prcustomLabel) == false && prcustomLabel != "all")
            {
                if (setting.HasMultiCategory == true)
                {
                    MyQuery = MyQuery.And(p => p.ProductsSubCategory.Any(x => x.ProductSubCategory.CustomLabel == prcustomLabel) || p.ProductsType.Any(x => x.ProductType.CustomLabel == prcustomLabel) || p.ProductsCategory.Any(x => x.ProductCategory.CustomLabel == prcustomLabel));

                }
                else
                {
                    MyQuery = MyQuery.And(p => p.ProductSubCategory.CustomLabel == prcustomLabel || p.ProductType.CustomLabel == prcustomLabel || p.ProductCategory.CustomLabel == prcustomLabel);

                }

            }

            int listLanguage = BaseLanguage.GetWebsiteLanguages().Count();
            if (codeValue != null)
            {
                //codeValue = codeValue.ToEnglishDigit();
                MyQuery = MyQuery.And(p => p.CodeValue.Contains(codeValue));
            }


            if (IsNullOrEmptyId(name) == false)
            {
                var nameQuery = PredicateBuilder.True<Product>();
                name = name.ToPersianChar();
                string[] nameSplit = name.Split(' ');
                foreach (var item in nameSplit)
                {
                    nameQuery = nameQuery.And(p =>
                    p.Name.Contains(item) ||
                    //p.ID.ToString() == item ||
                    //(p.ProductSubCategoryId != null && p.ProductSubCategory.Name.Contains(item)) ||
                    //(p.CodeValue.Contains(item)) ||
                    //(p.ProductCategoryId != null && p.ProductCategory.Name.Contains(item)) ||
                    //(p.ProductTypeId != null && p.ProductType.Name.Contains(item)) ||
                    //(p.ProductBrand.Name.Contains(item)) ||
                    ((listLanguage > 1) && (
                      (p.ProductLanguage.Any() && p.ProductLanguage.Any(x => x.Name.Contains(item)))))
                    //||
                    // (p.ProductSubCategoryId != null && p.ProductSubCategory.ProductSubCategoryLanguage.Any() && p.ProductSubCategory.ProductSubCategoryLanguage.Any(x => x.Name.Contains(item))) ||
                    // (p.ProductCategoryId != null && p.ProductCategory.ProductCategoryLanguage.Any() && p.ProductCategory.ProductCategoryLanguage.Any(x => x.Name.Contains(item))) ||
                    // (p.ProductType != null && p.ProductType.ProductTypeLanguage.Any() && p.ProductType.ProductTypeLanguage.Any(x => x.Name.Contains(item)))
                    /*)*//*)*/);
                }
                MyQuery = MyQuery.And(nameQuery);
            }

            if (IsNullOrEmptyId(custom) == false)
            {
                var QueryStr = PredicateBuilder.False<Product>();
                string[] searchParts = custom.Split('*');
                foreach (string oneItem in searchParts)
                {
                    string[] splitedOne = oneItem.Split('@');
                    int fieldId = splitedOne[0].GetInteger();
                    int itemId = splitedOne[1].GetInteger();

                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ProductCustomValue.Any(s => s.FieldId == fieldId && s.ItemId == itemId)
                        );
                    }

                }
                MyQuery = MyQuery.And(QueryStr);

            }

            if (SyncNameProductCustomField != null && SyncNameProductCustomField != string.Empty)
            {
                var itemQuery = PredicateBuilder.True<Product>();

                MyQuery = MyQuery.And(s => s.ProductCustomValue.Any(
                      p => p.ProductCustomField.SyncName == SyncNameProductCustomField
                      && (
                            (p.ItemId != null && p.ProductCustomItem.Value == valueProductCustom) ||
                      p.Value == valueProductCustom)
                      )
                    );
                MyQuery = MyQuery.And(itemQuery);
            }
            //if (distance > 1 && (!string.IsNullOrEmpty(latitude)) && (!string.IsNullOrEmpty(longtude)))
            //{
            //    var latLngQuery = PredicateBuilder.True<Product>();
            //    latLngQuery.And(s => (TrueDistance(s.Latitude, s.Longitude, latitude, longtude, distance) == true));
            //    MyQuery = MyQuery.And(latLngQuery);
            //}

            if (activeLocation == true)
            {
                if (BaseWebsite.WebsiteSetting.HasStore)
                {
                    var location = BaseWebsite.CurrentLocation;

                    MyQuery = MyQuery.And(p => p.StoreProduct.Any(s => s.Store.ShippingCity.Any(x =>
                       (x.CountryId == location.CountryId || x.CountryId == 0) &&
                       (x.CityId == location.CityId || x.CityId == 0) &&
                       (x.StateId == location.StateId || x.StateId == 0))));
                }
            }




            return MyQuery;
        }
        public bool TrueDistance(string lat1, string lng1, string lat2, string lng2, int distanceKm)
        {
            if ((!string.IsNullOrEmpty(lat1)) && (!string.IsNullOrEmpty(lat2)) && (!string.IsNullOrEmpty(lng1)) && (!string.IsNullOrEmpty(lng2)) && distanceKm > 0)
            {
                double dLat1 = double.Parse(lat1);
                double dLng1 = double.Parse(lng1);
                double dLat2 = double.Parse(lat2);
                double dLng2 = double.Parse(lng2);
                var coord1 = new GeoCoordinate(dLat1, dLng1);
                var coord2 = new GeoCoordinate(dLat2, dLng2);
                double meterDistance = distanceKm * 1000;
                var currentDistance = coord1.GetDistanceTo(coord2);
                if (currentDistance <= meterDistance)
                {
                    return true;
                }
            }
            return false;
        }

        public void IncreaseSellProduct(int productId)
        {
            Product productModel = _context.Product.Find(productId);
            productModel.SellCount = productModel.SellCount + 1;
            _context.SaveChanges();
        }
        public void UpdateProductQuantity(int productId)
        {

            var unitOfWork = new UnitOfWork();
            var product = _context.Product.Find(productId);
            var storeProductQuantity = unitOfWork.StoreProductQuantity.GetActives(productId).ToApi();
            if (storeProductQuantity.Any())
            {

                product.MinPrice = storeProductQuantity.Min(s => s.Price);
                if (product.MinPrice > 0)
                {
                    product.StatusId = _context.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString()).ID;
                }
                else
                {
                    product.StatusId = _context.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString()).ID;

                }
            }
            else
            {
                product.MinPrice = 0;
                product.StatusId = _context.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString()).ID;
            }
            Save();
            unitOfWork.Dispose();
        }
        public void Hide(int productId)
        {
            var product = GetById(productId);
            product.Deleted = true;
        }

    }
}
