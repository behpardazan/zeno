using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public static class BaseStore
    {
        public static string GetMainPicture(this Product product)
        {
            if (product.PictureId == null)
            {
                if (product.ProductPicture.Count > 0)
                    return product.ProductPicture.FirstOrDefault().Picture.GetUrl();
            }
            else
                return product.Picture.GetUrl();
            return "";
        }





        public static string GetDiscountString(this ViewDiscountGroup group)
        {
            string body = "";
            if (group.Type.Label == Enum_Code.DISCOUNTGROUP_TYPE_PRICE.ToString())
                body = group.PriceValue.GetCurrencyFormat() + " تومان";
            else
                body = group.PercentValue + "% از مبلغ کل";

            body = body.ToPersian();
            return body;
        }


        public static string GetDiscountName(this ViewDiscount discount)
        {
            string body = "";
            if (discount.Type.Label == Enum_Code.DISCOUNT_TYPE_SHOP.ToString())
                body = "فروشگاه - " + discount.Shop.Name + " - ";
            else if (discount.Type.Label == Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString())
                body = "نوع محصول - " + discount.ProductType.Name + " - ";
            else if (discount.Type.Label == Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString())
                body = "دسته بندی - " + discount.ProductCategory.Name + " - ";
            else if (discount.Type.Label == Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString())
                body = "زیردسته محصولات - " + discount.ProductSubCategory.Name + " - ";
            else if (discount.Type.Label == Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString())
                body = "محصول - " + discount.Product.Name + " - ";
            else if (discount.Type.Label == Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString())
                body = "تنوع - " + discount.StoreProductQuantity.FullName;

            string value = "";
            if (discount.PriceValue == null)
                value = discount.PercentValue + "% از مبلغ کل";
            else
                value = discount.PriceValue + " تومان از مبلغ کل";

            body = (body + " (" + value + ")").ToPersian();
            return body;
        }

        public static string GetRebateName(this Rebate rebate)
        {
            string body = "";
            if (rebate.Code.Label == Enum_Code.REBATE_TYPE_SHOP.ToString())
                body = rebate.Name + " (تمامی محصولات فروشگاه)";
            else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTTYPE.ToString())
                body = rebate.Name + " (تمامی محصولات - " + rebate.ProductType.Name + ")";
            else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTCATEGORY.ToString())
                body = rebate.Name + " (تمامی محصولات - " + rebate.ProductCategory.Name + ")";
            else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTSUBCATEGORY.ToString())
                body = rebate.Name + " (تمامی محصولات - " + rebate.ProductSubCategory.Name + ")";
            else if (rebate.Code.Label == Enum_Code.REBATE_TYPE_PRODUCTBRAND.ToString())
                body = rebate.Name + " (تمامی محصولات - " + rebate.ProductBrand.Name + ")";
            else
                body = rebate.Name;
            return body;
        }

        public static double? GetDiscountValue(this ViewDiscount discount)
        {
            if (discount.Type.Label == Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString())
            {
                double body = 0;
                if (discount.PriceValue != null && discount.PriceValue != 0)
                {
                    body = (discount.Product.Price - discount.PriceValue.Value);
                }
                else if (discount.PercentValue != null && discount.PercentValue != 0)
                {
                    body = ((discount.Product.Price / 100) * discount.PercentValue.Value);
                }
                return body;
            }
            return null;
        }

        public static double GetDiscountPrice(this Product product, Discount discount)
        {
            if (discount != null)
            {
                double body = 0;
                if (discount.PriceValue != null && discount.PriceValue != 0)
                {
                    body = discount.PriceValue.GetValueOrDefault();
                }
                else if (discount.PercentValue != null && discount.PercentValue != 0)
                {
                    body = ((product.Price.GetValueOrDefault() / 100) * discount.PercentValue.Value);
                }
                body = body < 0 ? 0 : body;
                return body;
            }
            else
                return product.Price.GetValueOrDefault();
        }

        public static double GetDiscountPrice(this ProductQuantity quantity, Discount discount)
        {
            if (discount != null)
            {
                double body = 0;
                if (discount.PriceValue != null && discount.PriceValue != 0)
                {
                    body = discount.PriceValue.GetValueOrDefault();
                }
                else if (discount.PercentValue != null && discount.PercentValue != 0)
                {
                    body = ((quantity.Price.GetValueOrDefault() / 100) * discount.PercentValue.Value);
                }
                body = body < 0 ? 0 : body;
                return body;
            }
            else
                return quantity.Price.GetValueOrDefault();
        }
        public static double GetDiscountPrice(this Product product)
        {
            UnitOfWork _context = new UnitOfWork();
            List<Discount> list = _context.Discount.GetAllByProduct(product);
            double MaxDiscount = 0;
            foreach (Discount item in list)
            {
                double discountPrice = GetDiscountPrice(product, item);
                if (item.IsTop)
                    MaxDiscount = discountPrice;
                else
                    MaxDiscount = discountPrice > MaxDiscount ? discountPrice : MaxDiscount;
            }
            _context.Dispose();
            return product.Price.GetValueOrDefault() - MaxDiscount;
        }

        public static double GetDiscountPrice(this StoreProductQuantity quantity,int count=1, UnitOfWork _context = null)
        {
            if (_context == null)
            {
                _context = new UnitOfWork();
            }
            var discounts = _context.Discount.GetAllByStoreId(quantity);
            var currentDiscount = new Discount();
            if (discounts.Any())
            {
                if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString());
                }
                else if (discounts.Any(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_SHOP.ToString()))
                {
                    currentDiscount = discounts.FirstOrDefault(s => s.Code.Label == Enum_Code.DISCOUNT_TYPE_SHOP.ToString());
                }
                if (currentDiscount.ValueTypeId != null)
                {
                    if (currentDiscount.Code1.Label == Enumarables.Enum_Code.DISCOUNTGROUP_TYPE_PRICE.ToString() && currentDiscount.PriceValue.HasValue)
                        return currentDiscount.PriceValue.Value;
                   
                    else if (currentDiscount.Code1.Label == Enumarables.Enum_Code.DISCOUNTGROUP_TYPE_PERCENT.ToString() && currentDiscount.PercentValue.HasValue)
                    {
                        var price = Base.BaseStore.GetFromPriceRange(count: count, storeProductQuantity: quantity);

                        var discount= (quantity.Price * (float)((float)currentDiscount.PercentValue.Value / 100));
                        return Math.Round(discount, 0);
                    }
                }
            }
            return 0;
        }

        public static double GetDiscountPrice(this ProductQuantity quantity)
        {
            UnitOfWork _context = new UnitOfWork();
            List<Discount> list = _context.Discount.GetAllByProduct(quantity.Product);
            double MaxDiscount = 0;
            foreach (Discount item in list)
            {
                double discountPrice = GetDiscountPrice(quantity, item);
                if (item.IsTop)
                    MaxDiscount = discountPrice;
                else
                    MaxDiscount = discountPrice > MaxDiscount ? discountPrice : MaxDiscount;
            }
            _context.Dispose();
            return quantity.Price.GetValueOrDefault() - MaxDiscount;
        }

        public static int GetDiscountPercent(this Product product)
        {
            UnitOfWork _context = new UnitOfWork();
            List<Discount> list = _context.Discount.GetAllByProduct(product);
            int discountPercent = 0;
            foreach (Discount item in list)
            {
                int temp = item.PercentValue.GetValueOrDefault();
                if (item.IsTop)
                    discountPercent = temp;
                else
                    discountPercent = temp > discountPercent ? temp : discountPercent;
            }
            _context.Dispose();
            return discountPercent;
        }
        public static int GetDiscountPercentNew(this Product product)
        {
            double price = product.MinPrice;
            double TotalPrice = product.DiscountValue != 0 ? product.MinPrice + product.DiscountValue : product.MinPrice;
            int DiscountPercent = (100 - (int)((price * 100) / TotalPrice));
 
            return DiscountPercent;
        }
        public static int GetDiscountPercentNew(this Product product,double discountPrice)
        {
            double price = product.MinPrice;
            double TotalPrice = product.DiscountValue != 0 ? product.MinPrice + discountPrice : product.MinPrice;
            int DiscountPercent = (100 - (int)((price * 100) / TotalPrice));

            return DiscountPercent;
        }
        public static Discount GetDiscountObject(this Product product, int? colorId = null)
        {
            UnitOfWork _context = new UnitOfWork();
            List<Discount> list = _context.Discount.
                GetAllByProduct(product);
            int discountPercent = 0;
            double discountPrice = 0;

            Discount discount = null;
            foreach (Discount item in list)
            {
                int temp = item.PercentValue.GetValueOrDefault();
                if (item.IsTop ||temp > discountPercent)
                {
                    discountPercent = temp;
                    discount = item;
                }
                double price= item.PriceValue.GetValueOrDefault();
                if (item.IsTop || price > discountPrice)
                {
                    discountPrice = price;
                    discount = item;
                }
            }
            return discount;
        }

        public static string GetDiscountString(this DiscountGroup group)
        {
            string body = "";
            if (group.Code.Label == Enum_Code.DISCOUNTGROUP_TYPE_PRICE.ToString())
                body = group.PriceValue.GetCurrencyFormat() + " تومان";
            else
                body = group.PercentValue + "% از مبلغ کل";

            body = body.ToPersian();
            return body;
        }

        public static ViewSendType GetStoreSendType(int? storeId, int orderPrice)
        {
            var result = new ViewSendType();
            UnitOfWork _context = new UnitOfWork();
            result = new ViewSendType(_context.SendType.GetByStoreId(storeId: storeId));
            result.CurrentPrice = _context.SendType.GetShippingPrice(storeId, orderPrice);
            return result;

        }
        public static string GetAddressString(this AccountAddress address)
        {
            if (address == null)
                return "-";
            else
            {
                string addressString = (address.StateId != null ? (address.State.Name + " - ") : "") + address.AddressValue + "<br />";
                addressString += "<b>شماره تلفن:</b> " + (string.IsNullOrEmpty(address.Phone) ? "-" : address.Phone) + "&nbsp;&nbsp;";
                addressString += "<b>تلفن همراه:</b> " + (string.IsNullOrEmpty(address.Mobile) ? "-" : address.Mobile) + "&nbsp;&nbsp;";
                addressString += "<b>کدپستی:</b> " + (string.IsNullOrEmpty(address.PostalCode) ? "-" : address.PostalCode) + "&nbsp;&nbsp;";
                addressString += "<b>نام تحویل گیرنده:</b> " + (string.IsNullOrEmpty(address.NameFamily) ? "-" : address.NameFamily) + "&nbsp;&nbsp;";
                addressString = addressString.ToPersian();
                return addressString;
            }
        }

        public static string GetCustomValue(this Product product, string syncName)
        {
            ProductCustomValue value = product.ProductCustomValue.FirstOrDefault(p =>
                p.ProductCustomField.SyncName == syncName
            );
            if (value != null)
            {
                if (value.ProductCustomField.Code.Label == Enum_Code.FIELD_TYPE_DROPDOWN.ToString() && value.ItemId != null)
                    return value.ProductCustomItem.Value;
                else
                    return value.Value;
            }
            else
                return "";
        }

        public static ProductCustomValue GetCustomValueObject(this Product product, string syncName)
        {
            ProductCustomValue value = product.ProductCustomValue.FirstOrDefault(p =>
                p.ProductCustomField.SyncName == syncName
            );

            if (value == null)
            {
                value = new ProductCustomValue()
                {
                    FieldId = 0,
                    ItemId = 0,
                    Value = ""
                };
            }
            return value;
        }

        public static double GetPrice(this SendType sendType, int count)
        {
            if (sendType == null)
                return 0;
            double price = (sendType == null) ? 0 : sendType.BasePrice.GetValueOrDefault() + (count * sendType.PerProductPrice.GetValueOrDefault());
            price = sendType.MaxPrice != null && price > sendType.MaxPrice ? sendType.MaxPrice.GetValueOrDefault() : price;
            return price;
        }

        public static double? GetRate(this Product product)
        {
            return product.ProductComment.Any(p => p.Approved == true && p.Rate != null) ?
                   product.ProductComment.Where(p => p.Approved == true && p.Rate != null)
                   .Average(p => p.Rate) : default(double?);
        }


        public static Discount GetDiscountPriceObject(this Product product, int? colorId = null)
        {
            UnitOfWork _context = new UnitOfWork();
            List<Discount> list = _context.Discount.
                GetAllByProduct(product);
            double discountPrice = 0;
            Discount discount = null;
            foreach (Discount item in list)
            {
                if (item.PriceValue != null)
                {
                    double temp = item.PriceValue.Value;
                    if (item.IsTop || temp > discountPrice)
                    {
                        discountPrice = temp;
                        discount = item;
                    }
                }

            }
            return discount;
        }

        public static double? GetBestRate(this Product product)
        {
            Random rd = new Random();
            int rate = rd.Next(1, 5);
            return product.ProductComment.Any(p => p.Approved == true && p.Rate != null) ?
                product.ProductComment.Where(p => p.Approved == true && p.Rate != null)
                    .Max(p => p.Rate) : rate;
        }
        public static double? GetWorstRate(this Product product)
        {
            Random rd = new Random();
            int rate = rd.Next(1, 5);
            return product.ProductComment.Any(p => p.Approved == true && p.Rate != null) ?
                product.ProductComment.Where(p => p.Approved == true && p.Rate != null)
                    .Min(p => p.Rate) : rate;
        }
        public static double? GetCountRate(this Product product)
        {
            Random rd = new Random();
            int count = rd.Next(1, 2000);
            return product.ProductComment.Any(p => p.Approved == true && p.Rate != null) ?
                product.ProductComment.Where(p => p.Approved == true && p.Rate != null)
                    .Count() : count;
        }
        public static bool StoreState(UnitOfWork _context, ViewAccount currentAccount)
        {

            var store = _context.Store.FirstOrDefault(s => s.ID == currentAccount.StoreId && s.Approve);
            if (store == null)
                return false;
            return true;
        }
        public static bool StoreProductId_IsValid(int storeProductId, UnitOfWork _context)
        {
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var storeProduct = _context.StoreProduct.FirstOrDefault(s => s.ID == storeProductId && s.StoreId == currentAccount.StoreId);
            if (storeProduct == null)
                return false;
            return true;
        }
        public static bool StoreCommentId_IsValid(int storeCommentId, UnitOfWork _context)
        {
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var storeComment = _context.StoreComment.FirstOrDefault(s => s.ID == storeCommentId && s.StoreId == currentAccount.StoreId);
            if (storeComment == null)
                return false;
            return true;
        }
        public static bool StoreOrderId_IsValid(int OrderId, UnitOfWork _context)
        {
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var order = _context.AccountOrder.FirstOrDefault(s => s.ID == OrderId && s.StoreId == currentAccount.StoreId);
            if (order == null)
                return false;
            return true;
        }
        public static bool StoreDiscount_IsValid(int discountId, UnitOfWork _context)
        {
            var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
            var storeDiscount = _context.Discount.FirstOrDefault(s => s.ID == discountId && s.StoreId == currentAccount.StoreId);
            if (storeDiscount == null)
                return false;
            return true;
        }
        public static void UpdateProductQuantity(Store store, UnitOfWork unitOfWork = null)
        {
            if (unitOfWork == null)
                unitOfWork = new UnitOfWork();
            var products = unitOfWork.Product.Where(p => p.StoreProduct.Any(s => s.StoreId == store.ID)).Distinct().ToList();
            UpdateProductQuantity(products, unitOfWork);
        }
        public static void UpdateProductQuantity(List<Product> products = null, UnitOfWork unitOfWork = null)
        {
            if (unitOfWork == null)
                unitOfWork = new UnitOfWork();
            if (products != null)
            {
                foreach (var item in products)
                {
                    UpdateProductQuantity(item.ID, unitOfWork);
                }
            }
        }
        public static void UpdateProductQuantity(int productId, UnitOfWork unitOfWork = null)
        {
            //if (unitOfWork == null)
                unitOfWork = new UnitOfWork();

            var product = unitOfWork.Product.GetById(productId);
            if (product.IsAdvertising == null)
            {
                //var currentStatus = product.Code.Label;
                if (Base.BaseWebsite.WebsiteSetting.HasStore)
                {
                    var storeProductQuantity = unitOfWork.StoreProductQuantity.GetActives(productId).ToApi();
                    if (storeProductQuantity.Any())
                    {
                        product.Price = storeProductQuantity.Max(s => s.BasePrice);
                        //add in history price
                        //ProductPiceHistory productPrice = new ProductPiceHistory();
                        //productPrice.Date = DateTime.Now;
                        //productPrice.ProductId = productId;
                        //productPrice.Price = product.MinPrice == 0 ? storeProductQuantity.Min(s => s.Price) : product.MinPrice;
                        //unitOfWork.ProductPiceHistory.Insert(productPrice);
                        unitOfWork.Save();
                        product.MinPrice = storeProductQuantity.Min(s => s.Price);
                        product.DiscountValue = product.Price.Value - product.MinPrice;
                        if (product.MinPrice > 0)
                        {
                            if (product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_SOON.ToString()).ID
                                && product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_SELLING.ToString()).ID)
                                product.StatusId = unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString()).ID;

                        }
                        else
                        {

                            product.MinPrice = 0;
                            product.Price = 0;
                            product.DiscountValue = 0;
                            if (product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_SOON.ToString()).ID
                                && product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_SELLING.ToString()).ID)
                                product.StatusId = unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString()).ID;

                        }
                    }
                    else
                    {

                        product.MinPrice = 0;
                        product.Price = 0;
                        product.DiscountValue = 0;
                        if (product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_SOON.ToString()).ID
                            && product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_SELLING.ToString()).ID)
                            product.StatusId = unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString()).ID;
                    }
                }
                else
                {
                    var productQuantity = unitOfWork.ProductQuantity.GetAllActives(productId).ToApi();
                    if (productQuantity.Any())
                    {
                        //ProductPiceHistory productPrice = new ProductPiceHistory();
                        //productPrice.Date = DateTime.Now;
                        //productPrice.ProductId = productId;
                        //productPrice.Price = product.MinPrice == 0 ? productQuantity.Min(s => s.Price) : product.MinPrice;
                        //unitOfWork.ProductPiceHistory.Insert(productPrice);
                        //unitOfWork.Save();
                        product.Price = productQuantity.Max(s => s.ProductPrice);
                        product.MinPrice = productQuantity.Min(s => s.Price);
                        product.DiscountValue = product.Price.Value - product.MinPrice;

                        if (product.MinPrice > 0)
                        {
                            if (product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_SOON.ToString()).ID
                                && product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_SELLING.ToString()).ID)
                                product.StatusId = unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString()).ID;
                        }
                        else
                        {
                            product.MinPrice = 0;
                            product.Price = 0;
                            product.DiscountValue = 0;
                            if (product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_SOON.ToString()).ID
                                && product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_SELLING.ToString()).ID)
                                product.StatusId = unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString()).ID;

                        }
                    }
                    else
                    {
                        product.MinPrice = 0;
                        product.Price = 0;
                        product.DiscountValue = 0;
                        if (product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_SOON.ToString()).ID
                            && product.StatusId != unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_SELLING.ToString()).ID)
                            product.StatusId = unitOfWork.Code.FirstOrDefault(s => s.Label == Enumarables.Enum_Code.PRODUCT_STATUS_NOT_AVAILABLE.ToString()).ID;
                    }

                }

                unitOfWork.Save();
                if (/*currentStatus!= Enumarables.Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString() &&*/ product.Code.Label == Enumarables.Enum_Code.PRODUCT_STATUS_AVAILABLE.ToString())
                {
                    unitOfWork.ProductNotify.SendMessage(product, unitOfWork);
                }
            }

        }
        public static ViewProductVariety GetStoreProductVariety(UnitOfWork _context, int productId, int? colorId = null, int? sizeId = null, int? optionItemId = null,int count=1,AccountAddress address=null)
        {

            var result = new ViewProductVariety();
            var productQuantity = new List<ViewStoreProductQuantity>();
            var colors = new List<Color>();
            var sizes = new List<Size>();
           
            var optionItems = new List<ProductOptionItem>();

            if (Base.BaseWebsite.WebsiteSetting.HasStore)
            {
                result.ProductQuantity = new List<ViewStoreProductQuantity>();

                IEnumerable<StoreProductQuantity> variety = _context.StoreProductQuantity.GetActives(productId);
                if (variety.Any())
                {
                    productQuantity=variety.ToApi(address:address).OrderBy(o => o.Price).ToList();
                    result.ProductQuantity = productQuantity;
                    if ((!colorId.HasValue) && (!sizeId.HasValue) && (!optionItemId.HasValue))
                    {
                        var cheapest = variety.FirstOrDefault(s => s.ID == productQuantity[0].ID);
                        colorId = cheapest.ProductQuantity.ColorId;

                        sizeId = cheapest.ProductQuantity.SizeId;
                        optionItemId = cheapest.ProductQuantity.OptionItemId;
                        if (colorId.HasValue)
                        {
                            result.ColorName = cheapest.ProductQuantity.Color.GetName();
                            result.ColorHex = cheapest.ProductQuantity.Color.HexValue;
                        }
                        if (sizeId.HasValue)
                            result.SizeName = cheapest.ProductQuantity.Size.GetName();
                        if (optionItemId.HasValue)
                        {
                            result.OptionName = cheapest.ProductQuantity.ProductOptionItem.ProductOption.Name;
                            result.OptionItemValue = cheapest.ProductQuantity.ProductOptionItem.Value;

                        }
                    }
                    colors = variety.Select(s => s.ProductQuantity.Color).Distinct().ToList();
                    sizes = variety.Where(s => colorId.HasValue ? s.ProductQuantity.ColorId == colorId : s.ProductQuantity.Color == colors.First()).Select(s => s.ProductQuantity.Size).Distinct().ToList();
                    optionItems = variety.Where(s => (colorId.HasValue ? s.ProductQuantity.ColorId == colorId : s.ProductQuantity.Color == colors.First()) && (sizeId.HasValue ? s.ProductQuantity.SizeId == sizeId : s.ProductQuantity.Size == sizes.First())).Select(s => s.ProductQuantity.ProductOptionItem).Distinct().ToList();
                    optionItems.RemoveAll(item => item == null);
                }
            }
            else
            {
                result.ProductQuantity = new List<ViewProductQuantity>();

                var variety = _context.ProductQuantity.GetAllActives(productId);
                if (variety.Any())
                {
                    result.ProductQuantity = variety.ToApi().OrderBy(o => o.Price).ToList();
                    if ((!colorId.HasValue) && (!sizeId.HasValue) && (!optionItemId.HasValue))
                    {
                        var cheapest = variety.FirstOrDefault(s => s.ID == ((List<ViewProductQuantity>)result.ProductQuantity)[0].Id);
                        colorId = cheapest.ColorId;
                        sizeId = cheapest.SizeId;
                        optionItemId = cheapest.OptionItemId;
                        if (colorId.HasValue)
                        {
                            result.ColorName = cheapest.Color.GetName();
                            result.ColorHex = cheapest.Color.HexValue;
                        }
                        if (sizeId.HasValue)
                            result.SizeName = cheapest.Size.GetName();
                        if (optionItemId.HasValue)
                            result.OptionName = cheapest.ProductOptionItem.ProductOption.Name;
                    }
                    colors = variety.Select(s => s.Color).Distinct().ToList();
                    sizes = variety.Where(s => colorId.HasValue ? s.ColorId == colorId : s.Color == colors.First()).Select(s => s.Size).Distinct().ToList();
                    optionItems = variety.Where(s => (colorId.HasValue ? s.ColorId == colorId : s.Color == colors.First()) && (sizeId.HasValue ? s.SizeId == sizeId : s.Size == sizes.First())).Select(s => s.ProductOptionItem).Distinct().ToList();
                }
            }

            if (colorId == null && colors.Any() && colors.First() != null)
            {
                colorId = colors.First().ID;
                result.ColorName = colors.First().GetName();
                result.ColorHex = colors.First().HexValue;
            }
            else if (colorId != null && colors.Any())
            {
                result.ColorName = colors.First(s => s.ID == colorId).GetName();
                result.ColorHex = colors.First(s => s.ID == colorId).HexValue;
            }


            if (sizeId == null && sizes.Any() && sizes.First() != null)
            {
                result.SizeName = sizes.First().GetName();
                sizeId = sizes.First().ID;

            }
            else if (sizeId != null && sizes.Any())
            {
                result.SizeName = sizes.First(s => s.ID == sizeId).GetName();
            }
            if (optionItemId == null && optionItems.Any() && optionItems.First() != null)
            {
                result.OptionName = optionItems.First().ProductOption.Name;
                result.OptionItemValue = optionItems.First().Value;
                if (optionItemId == null)
                    optionItemId = optionItems.First().ID;

            }
            else if (optionItemId != null && optionItems.Any())
            {
               
                var option = optionItems.Where(s => s.ID == optionItemId.Value).FirstOrDefault();

                result.OptionName = option.ProductOption.Name;
                result.OptionItemValue = option.Value;
            }


            //var productQuantity = variety.Where(s => s.ProductQuantity.SizeId == sizeId && s.ProductQuantity.ColorId == colorId && s.ProductQuantity.OptionItemId == optionItemId);

            result.ColorId = colorId;
            result.SizeId = sizeId;


            result.OptionItemId = optionItemId;
            if (Base.BaseWebsite.WebsiteSetting.HasStore)
                result.ProductQuantity = productQuantity.Where(s => s.ProductQuantity.SizeId == sizeId && s.ProductQuantity.ColorId == colorId && s.ProductQuantity.OptionItemId == optionItemId).ToList();
            else
                result.ProductQuantity = ((List<ViewProductQuantity>)result.ProductQuantity).Where(s => s.SizeId == sizeId && s.ColorId == colorId && s.OptionItemId == optionItemId).ToList();

            if (!colors.Any(s => s == null))
            {
                result.Colors = colors.ToApi();
            }
            if (!sizes.Any(s => s == null))
            {
                result.Sizes = sizes.ToApi();
            }
            if (!optionItems.Any(s => s == null))
            {
                result.OptionItems = optionItems.ToApi();
            }

            return result;

        }

        public static double GetFromPriceRange(this StoreProductQuantity storeProductQuantity,int count=1)
        {
            if(!storeProductQuantity.PriceRange.Any())
            {
                return storeProductQuantity.Price;
            }
            else
            {
               var range= storeProductQuantity.PriceRange.FirstOrDefault(s => s.StartCount <= count && (s.EndCount == null || s.EndCount >= count));
                if(range !=null)
                {
                    return range.Price;
                }
                else
                {
                    return storeProductQuantity.Price;
                }
            }
        }
        public static double GetGoldPrice()
        {
            try
            {
                //var url = "https://www.tala.ir/gold-price";
                //var web = new HtmlWeb();
                //var doc = web.Load(url);
                //var node = doc.DocumentNode.SelectNodes("//span[@class='value hidden-xs']")[2];
                //    string value = node.Attributes["data-value"].Value;

                var client = new RestClient("http://www.tala.ir/");
                var request = new RestRequest("ajax/price", Method.GET);
                IRestResponse response = client.Execute(request);
                ViewGoldPrice obj = JsonConvert.DeserializeObject<ViewGoldPrice>(response.Content);
                var price = obj.gold.gold_18k.v.Replace(",", "");
                return Convert.ToDouble(price);
            }
            catch
            {
                return -1;
            }
        }

        //public static ViewProductVariety GetProductVariety(UnitOfWork _context, int productId, int? colorId = null, int? sizeId = null,int ? optionItemId = null)
        //{
        //    var result = new ViewProductVariety();
        //    var variety = _context.StoreProductQuantity.GetActives(productId).ToList();
        //    if (variety.Any())
        //    {
        //        var colors = variety.Select(s => s.ProductQuantity.Color).Distinct().ToList();
        //        var sizes = variety.Where(s => colorId.HasValue ? s.ProductQuantity.ColorId == colorId : s.ProductQuantity.Color == colors.First()).Select(s => s.ProductQuantity.Size).Distinct().ToList();
        //        var optionItems = variety.Where(s => colorId.HasValue ? s.ProductQuantity.ColorId == colorId : s.ProductQuantity.Color == colors.First() && sizeId.HasValue ? s.ProductQuantity.SizeId == sizeId : s.ProductQuantity.Size == sizes.First()).Select(s => s.ProductQuantity.ProductOptionItem).Distinct().ToList();
        //        if (colorId == null && colors.First() != null)
        //            colorId = colors.First().ID;
        //        if (sizeId == null && sizes.Any() && sizes.First() != null)
        //            sizeId = sizes.First().ID;
        //        if ( optionItems.Any() && optionItems.First() != null)
        //        {               
        //            result.OptionName = optionItems.First().ProductOption.Name;
        //            if(optionItemId == null)
        //                optionItemId = optionItems.First().ID;
        //        }

        //        var productQuantity = variety.Where(s => s.ProductQuantity.SizeId == sizeId && s.ProductQuantity.ColorId == colorId && s.ProductQuantity.OptionItemId == optionItemId);

        //        result.ColorId = colorId;
        //        result.SizeId = sizeId;
        //        result.OptionItemId = optionItemId;
        //        result.ProductQuantity = productQuantity.ToApi().OrderBy(o => o.Price).ToList();
        //        if (!colors.Any(s => s == null))
        //        {
        //            result.Colors = colors.ToApi();
        //        }
        //        if (!sizes.Any(s => s == null))
        //        {
        //            result.Sizes = sizes.ToApi();
        //        }
        //        if (!optionItems.Any(s => s == null))
        //        {
        //            result.OptionItems = optionItems.ToApi();
        //        }
        //    }
        //    return result;

        //}
    }
}
