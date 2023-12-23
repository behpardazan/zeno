using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using DataLayer.Helpers.Common;
using System.Data;
using OfficeOpenXml;

namespace Panel.Areas.Crm.Controllers
{
    public class AccountOrderController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index2(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            int[] statusId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string product = null,
            string rebate = null,
            string pageIndex = null,
            string pageSize = null,
            int? storeId = null,
            int[] stores = null

            )
        {

            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();

            ViewBag.FromOrderId = fromorderid;
            ViewBag.ToOrderId = toorderid;
            ViewBag.StatusId_2 = statusId;
            ViewBag.Status = status;
            ViewBag.Product = product;
            ViewBag.FromDatetime = from;
            ViewBag.ToDatetime = to;
            ViewBag.RefId = refid;
            ViewBag.Customer = customer;
            ViewBag.Rebate = rebate;
            ViewBag.SelectedId = 0;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.MerchantId = merchantId;
            ViewBag.StoreId = storeId != null ? storeId.ToString() : null;

            FillDropDowns();

            ViewBag.TotalCount = _context.AccountOrder.SearchCount(
                    merchantId: merchantId,
                    statusId: statusId,
                    product: product,
                    storeId: storeId,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid,
                    stores: stores,
                    isParent: false);

            List<AccountOrder> listOrder = _context.AccountOrder.Search(
                    merchantId: merchantId,
                    statusId: statusId,
                    pageSize: size,
                    product: product,
                    storeId: storeId,
                    index: index,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid,
                    stores: stores, isParent: false)
                .ToList();

            double? SumDiscount = 0;
            double? SumRebate = 0;

            var list = listOrder.GroupBy(x => x.ParentId)
        .Select(x => new { ParentId = x.Key, Prices = x.Select(s => s.AccountOrder2.DiscountPrice) });
            foreach (var item in list)
            {
                var price = item.Prices.FirstOrDefault();
                SumDiscount += price;
            }
            var list2 = listOrder.GroupBy(x => x.ParentId)
       .Select(x => new { ParentId = x.Key, Prices = x.Select(s => s.AccountOrder2.RebatePrice) });
            foreach (var item in list2)
            {
                var price = item.Prices.FirstOrDefault();
                SumRebate += price;
            }
            ViewBag.Discount = SumDiscount;
            ViewBag.Rebate = SumRebate;

            ViewBag.SumPrice = listOrder.Sum(s => s.Price);
            return View(listOrder.ToView());
        }
        public ActionResult OrderUser(
          string status = null,
          int? merchantId = null,
          int[] statusId = null,
          string from = null,
          string to = null
         
          )
        {
           
            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            DateTime toDatetimeTemp = toDate.AddDays(1);
            ViewBag.StatusId_2 = statusId;
            ViewBag.Status = status;
            ViewBag.FromDatetime = from;
            ViewBag.ToDatetime = to;
            ViewBag.SelectedId = 0;
            FillDropDowns();
            var list = _context.AccountOrder.Where(s=>s.PaymentProductOrder.Any(x=>x.Payment.RefNumber == null && (x.Payment.StatusId == 1067 || x.Payment.StatusId == 1070 || x.Payment.StatusId == 1068) && x.Payment.Datetime >= fromDate && x.Payment.Datetime <= toDatetimeTemp) && (s.StatusId == 2068 || s.StatusId == 2093 || s.StatusId == 2072 || s.StatusId == 2073) && s.ParentId==null ).ToList();
            ViewBag.TotalCount = list.Count();
            ViewBag.TotalPrice = list.Sum(s=>s.ProductsPrice);

            return View(list);
        }
        public ActionResult Index(
    string status = null,
    int? fromorderid = null,
    int? toorderid = null,
    int? merchantId = null,
    int[] statusId = null,
    string from = null,
    string to = null,
    string refid = null,
    string customer = null,
    string product = null,
    string rebate = null,
    string pageIndex = null,
    string pageSize = null,
    int? storeId = null,
    int[] stores = null

    )
        {

            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();

            ViewBag.FromOrderId = fromorderid;
            ViewBag.ToOrderId = toorderid;
            ViewBag.StatusId_2 = statusId;
            ViewBag.Status = status;
            ViewBag.Product = product;
            ViewBag.FromDatetime = from;
            ViewBag.ToDatetime = to;
            ViewBag.RefId = refid;
            ViewBag.Customer = customer;
            ViewBag.Rebate = rebate;
            ViewBag.SelectedId = 0;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.MerchantId = merchantId;
            ViewBag.StoreId = storeId != null ? storeId.ToString() : null;
            ViewBag.SelectedStores = stores != null ? stores : new int[0];


            FillDropDowns();



            ViewShowAccountOrder result = _context.AccountOrder.Search2(
                    merchantId: merchantId,
                    statusId: statusId,
                    pageSize: size,
                    product: product,
                    storeId: storeId,
                    index: index,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid,
                    stores: stores,
                    isParent: false);


            return View(result);
        }

        public ActionResult ProductList(
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            string status = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string pageIndex = null,
            string pageSize = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            int[] statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

            List<AccountOrder> list = _context.AccountOrder.Search(
                merchantId: merchantId,
                statusId: statusId,
                pageSize: size,
                index: index,
                refId: refid,
                customer: customer,
                fromDatetime: fromDate,
                toDatetime: toDate,
                fromorderid: fromorderid,
                toorderid: toorderid);

            List<KeyValuePair<Product, int>> productList = new List<KeyValuePair<Product, int>>();
            List<AccountOrderProduct> listOrder = new List<AccountOrderProduct>();
            foreach (AccountOrder order in list)
            {
                listOrder.AddRange(order.AccountOrderProduct.ToList());
            }

            foreach (AccountOrderProduct product in listOrder)
            {
                var item = productList.Where(p => p.Key.ID == product.ProductId);
                if (item.Any())
                {
                    int oldCount = item.First().Value;
                    productList.Remove(item.First());
                    productList.Add(new KeyValuePair<Product, int>(product.Product, product.Count + oldCount));
                }
                else
                {
                    productList.Add(new KeyValuePair<Product, int>(product.Product, product.Count));
                }
            }

            productList = productList.OrderByDescending(p => p.Value).ToList();

            return View(productList);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrder order = _context.AccountOrder.GetById(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            SmsSetting setting = _context.SmsSetting.GetBySmsType(Enum_SmsType.ORDER_POST);
            if (setting != null)
            {
                StringBuilder body = new StringBuilder();
                body.AppendLine("سفارش شما در سایت " /*+ order.Shop.Name + " برای شما ارسال شد"*/);
                body.Append("کد رهگیری مرسوله پستی: " + order.PostalCode);
                ViewBag.SmsBody = body;
            }
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(order.StatusId);
            return View(order);
        }

        [HttpPost]
        public ActionResult ChangeStatus(int OrderId, int StatusId, string PostalCode, string Description, string SmsBody)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrder order = _context.AccountOrder.GetById(OrderId);
            int last_status = order.StatusId;
            Code status = _context.Code.GetById(StatusId);

            if (status.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() ||
                status.Label == Enum_Code.ORDER_STATUS_ARCHIVES.ToString() ||
                status.Label == Enum_Code.ORDER_STATUS_PROCESS.ToString() ||
                status.Label == Enum_Code.ORDER_STATUS_CANCEL.ToString() ||
                status.Label == Enum_Code.ORDER_STATUS_STORE.ToString() ||
                status.Label == Enum_Code.ORDER_STATUS_POST.ToString())
            {
                order.StatusId = StatusId;
                order.PostalCode = PostalCode;
                order.Description = Description;
                order.SyncDatetime = null;
                _context.AccountOrder.Update(order);
                _context.Save();

                if (status.Label == Enum_Code.ORDER_STATUS_POST.ToString() &&
                    string.IsNullOrEmpty(SmsBody) == false)
                {
                    _context.Sms.SaveNewSms(order.Account.Mobile, Enum_SmsType.ORDER_POST, SmsBody, order.PostalCode);
                    _context.Email.SaveNewEmail(order.Account.Email, Enum_EmailType.ORDER_POST, SmsBody, "ارسال خرید شماره " + order.ID);
                    _context.Save();
                }
            }

            FillDropDowns(order.StatusId);

            return RedirectToAction("Index", new { status = last_status });
        }

        public ActionResult Print(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrder order = _context.AccountOrder.GetById(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        public ActionResult PrintAddress(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            AccountOrder order = _context.AccountOrder.GetById(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        private void FillDropDowns(int selected = 0)
        {
            bool hasDefault = selected == 0 ? false : true;
            ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.ORDER_STATUS, hasDefault), "ID", "Name", selected);
            ViewBag.MerchantList = _context.Merchant.GetAll();
            ViewBag.Stores = new SelectList(_context.Store.GetAll(), "ID", "Name");

        }

        public ActionResult GoToStore(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string pageIndex = null,
            string pageSize = null,
            string orderId = null)
        {
            int[] statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (orderId != null)
            {
                List<string> listOrder = orderId.Split(',').ToList();
                foreach (string IdStr in listOrder)
                {
                    int Id = IdStr.GetInteger();
                    if (Id != 0)
                    {
                        AccountOrder order = _context.AccountOrder.GetById(Id);
                        if (order != null &&
                            (
                                order.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() ||
                                order.Code.Label == Enum_Code.ORDER_STATUS_PROCESS.ToString())
                            )
                        {
                            order.SyncDatetime = null;
                            order.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_STORE);
                            _context.AccountOrder.Update(order);
                            _context.Save();
                        }
                    }
                }
            }

            return RedirectToAction("index", new
            {
                @fromorderid = fromorderid,
                @toorderid = toorderid,
                @statusId = statusId,
                @merchantid = merchantId,
                @from = from,
                @to = to,
                @refid = refid,
                @customer = customer,
                @pagesize = pageSize,
                @pageIndex = pageIndex
            });
        }

        public ActionResult GoToProcess(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string pageIndex = null,
            string pageSize = null,
            string orderId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            if (orderId != null)
            {
                List<string> listOrder = orderId.Split(',').ToList();
                foreach (string IdStr in listOrder)
                {
                    int Id = IdStr.GetInteger();
                    if (Id != 0)
                    {
                        AccountOrder order = _context.AccountOrder.GetById(Id);
                        if (order != null &&
                            (
                                order.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() ||
                                order.Code.Label == Enum_Code.ORDER_STATUS_PROCESS.ToString())
                            )
                        {
                            order.SyncDatetime = null;
                            order.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_PROCESS);
                            _context.AccountOrder.Update(order);
                            _context.Save();
                        }
                    }
                }
            }

            int[] statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

            return RedirectToAction("index", new
            {
                @fromorderid = fromorderid,
                @toorderid = toorderid,
                @merchantid = merchantId,
                @statusId = statusId,
                @from = from,
                @to = to,
                @refid = refid,
                @customer = customer,
                @pagesize = pageSize,
                @pageIndex = pageIndex
            });
        }

        public ActionResult AddressList(
            string status = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            string from = null,
            string to = null,
            string refid = null,
            string customer = null,
            string pageIndex = null,
            string pageSize = null,
            string rebate = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();

            int[] statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();

            List<AccountOrder> listOrders = _context.AccountOrder.Search(
                    statusId: statusId.ToArray(),
                    pageSize: size,
                    index: index,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    merchantId: merchantId,
                    toorderid: toorderid)
                .OrderByDescending(p => p.ID)
                .ToList();
            return View(listOrders);
        }

        //public ActionResult ExportExcel(
        //    string status = null,
        //    int? fromorderid = null,
        //    int? toorderid = null,
        //    int? merchantId = null,
        //    string from = null,
        //    string to = null,
        //    string refid = null,
        //    string customer = null,
        //    string pageIndex = null,
        //    string pageSize = null,
        //    string rebate = null)
        //{
        //    _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);

        //    DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
        //    DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
        //    int size = pageSize == null ? 50 : pageSize.GetInteger();
        //    int index = pageIndex == null ? 1 : pageIndex.GetInteger();
        //    int[] statusId = status.Contains("-") ? status.Split('-').Select(n => Convert.ToInt32(n)).ToArray() : new int[] { status.GetInteger() };

        //    List<AccountOrder> listOrders = _context.AccountOrder.Search(
        //            index: index,
        //            pageSize: size,
        //            statusId: statusId.ToArray(),
        //            merchantId: merchantId,
        //            refId: refid,
        //            customer: customer,
        //            fromDatetime: fromDate,
        //            toDatetime: toDate,
        //            fromorderid: fromorderid,
        //            rebate: rebate,
        //            toorderid: toorderid)
        //        .OrderByDescending(p => p.ID)
        //        .ToList();

        //    DataTable table = new DataTable();
        //    table.Columns.Add("ردیف");
        //    table.Columns.Add("شماره فاکتور");
        //    table.Columns.Add("نام و نام خانوادگی");
        //    table.Columns.Add("تلفن");
        //    table.Columns.Add("استان");
        //    table.Columns.Add("آدرس");
        //    table.Columns.Add("کدپستی");
        //    table.Columns.Add("محصولات");
        //    table.Columns.Add("تعداد");
        //    table.Columns.Add("هزینه ارسال");
        //    table.Columns.Add("تخفیف");
        //    table.Columns.Add("مبلغ بدون ارسال");
        //    table.Columns.Add("درصد کل تخفیف");
        //    table.Columns.Add("مبلغ کل (تومان)");
        //    table.Columns.Add("نحوه ارسال");
        //    table.Columns.Add("تاریخ");
        //    table.Columns.Add("زمان");
        //    table.Columns.Add("کد رهگیری بانک");
        //    table.Columns.Add("درگاه بانک");
        //    table.Columns.Add("کدرهگیری پستی");

        //    for (int i = 0; i < listOrders.Count; i++)
        //    {
        //        AccountOrder order = listOrders[i];
        //        StringBuilder products = new StringBuilder();
        //        int count = 0;
        //        double sumOnlyProductPrice = 0;
        //        foreach (AccountOrderProduct item in order.AccountOrderProduct)
        //        {
        //            products.AppendLine(item.Product.Name + "(" + item.Count + " عدد)");
        //            count = count + item.Count;
        //            sumOnlyProductPrice += item.Price;
        //        }

        //        StringBuilder phones = new StringBuilder();
        //        phones.AppendLine(order.Account.Mobile.GetEnglish());
        //        string stateValue = "-";
        //        string addressValue = "-";
        //        string postalCode = "-";
        //        string sendType = "-";
        //        string refNumber = "-";
        //        string merchantName = "-";
        //        string discount = order.DiscountPrice.GetCurrencyFormat();
        //        string price = order.Price.GetCurrencyFormat();
        //        string sendPrice = order.SendPrice.GetCurrencyFormat();
        //        string priceWithoutSend = (order.Price - order.SendPrice).GetCurrencyFormat();
        //        string discountPercent = sumOnlyProductPrice != 0 ? ((order.DiscountPrice / sumOnlyProductPrice) * 100).GetCurrencyFormat() : "0";

        //        if (order.AddressId != null)
        //        {
        //            if (order.Account.Mobile != null && order.Account.Mobile.Contains(order.AccountAddress.Mobile) == false)
        //                phones.AppendLine(order.AccountAddress.Mobile.GetEnglish());
        //            if (order.Account.Phone != null && order.Account.Phone.Contains(order.AccountAddress.Phone) == false)
        //                phones.AppendLine(order.AccountAddress.Phone.GetEnglish());
        //            if (order.AccountAddress.StateId != null)
        //                stateValue = order.AccountAddress.State.Name;
        //            addressValue = order.AccountAddress.AddressValue;
        //            postalCode = order.AccountAddress.PostalCode.GetEnglish();
        //        }
        //        if (order.SendTypeId != null)
        //        {
        //            sendType = order.SendType.Name;
        //        }
        //        if (order.PaymentProductOrder.Count > 0)
        //        {
        //            string payment_success_status = Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString();
        //            string payment_service_failed = Enum_Code.PAYMENT_STATUS_SERVICE_FAILED.ToString();
        //            PaymentProductOrder payment = order.PaymentProductOrder.FirstOrDefault(p =>
        //                p.Payment.Code.Label == payment_success_status ||
        //                p.Payment.Code.Label == payment_service_failed);
        //            if (payment != null)
        //            {
        //                refNumber = payment.Payment.RefNumber;
        //                merchantName = payment.Payment.Merchant.Bank.Name;
        //            }
        //        }

        //        table.Rows.Add(
        //            (i + 1),
        //            order.ID,
        //            order.Account.FullName,
        //            phones.ToString(),
        //            stateValue,
        //            addressValue,
        //            postalCode,
        //            products,
        //            count,
        //            sendPrice,
        //            discount,
        //            priceWithoutSend,
        //            discountPercent,
        //            price,
        //            sendType,
        //            order.Datetime.ToPersian(),
        //            order.Datetime.ToShortTimeString().ToPersian(),
        //            refNumber,
        //            merchantName,
        //            order.PostalCode.ToPersian());

        //    }

        //    BaseExcel.ExportExcel(table);
        //    return View(listOrders);
        //}
        [HttpGet]
        public ActionResult Report(int? accountId = null,
    int? storeId = null,
    int[] stores = null,
    int? merchantId = null,
    string refId = null,
    int[] statusId = null,
    List<string> statusStr = null,
    string customer = null,
    string product = null,
    int? fromorderid = null,
    int? toorderid = null,
    string from = null,
            string to = null,
    string rebate = null,
    bool? clear = null,
    Enum_Report reportType = Enum_Report.ORDER_STORE_SELL)
        {
            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();

            var model = BaseReport.AccountOrder(
                unitOfWork: _context,
                accountId: accountId,
                storeId: storeId,
                stores: stores,
                merchantId: merchantId,
                customer: customer,
                fromDatetime: fromDate,
                fromorderid: fromorderid,
                product: product,
                rebate: rebate,
                refId: refId,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDate,
                toorderid: toorderid,
                clear: clear,
                reportType: reportType);
            var a = Json(model, JsonRequestBehavior.AllowGet);
            return a;
        }
        public void ExportExcel(
            string status = null,
    int? fromorderid = null,
    int? toorderid = null,
    int? merchantId = null,
    int[] statusId = null,
    string from = null,
    string to = null,
    string refid = null,
    string customer = null,
    string product = null,
    string rebate = null,
    string pageIndex = null,
    string pageSize = null,
    int? storeId = null,
    int[] stores = null)
        {
            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();

            ViewShowAccountOrder model = _context.AccountOrder.Search2(
                    merchantId: merchantId,
                    statusId: statusId,
                    pageSize: size,
                    product: product,
                    storeId: storeId,
                    index: index,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid,
                    stores: stores,
                    isParent: false);

            int row = 1;
            var excelPackage = new ExcelPackage();
            var sheet = excelPackage.Workbook.Worksheets.Add(Resource.Lang.Orders + DateTime.Now.ToString("yyyyMMddhhmmss"));
            sheet.View.RightToLeft = true;


            sheet.Cells[$"A{row}"].Value = "#";
            sheet.Cells[$"B{row}"].Value = "تاریخ";
            sheet.Cells[$"C{row}"].Value = "نام فروشنده";
            sheet.Cells[$"D{row}"].Value = "شماره فاکتور اصلی";
            sheet.Cells[$"E{row}"].Value = "شماره فاکتور فروشگاه";
            sheet.Cells[$"F{row}"].Value = "نام خریدار";
            sheet.Cells[$"G{row}"].Value = "نام کالا";
            sheet.Cells[$"H{row}"].Value = "سایز کالا";
            sheet.Cells[$"I{row}"].Value = "رنگ کالا";
            sheet.Cells[$"J{row}"].Value = "گزینه خرید";
            sheet.Cells[$"K{row}"].Value = "مبلغ واحد (ریال)";
            sheet.Cells[$"L{row}"].Value = "تخفیف واحد (ریال)";
            sheet.Cells[$"M{row}"].Value = "درصد تخفیف";
            sheet.Cells[$"N{row}"].Value = "تعداد";
            sheet.Cells[$"O{row}"].Value = "مبلغ کل (ریال)";
            sheet.Cells[$"P{row}"].Value = "شرح کد تخفیف";
            sheet.Cells[$"Q{row}"].Value = "کد تخفیف (درصد)";
            sheet.Cells[$"R{row}"].Value = "کد تخفیف (ریال)";
            sheet.Cells[$"S{row}"].Value = "مبلغ كل پس از کد تخفیف (ریال)";
            sheet.Cells[$"T{row}"].Value = "هزینه ارسال (ریال)";
            sheet.Cells[$"U{row}"].Value = "وضعیت";
            sheet.Cells[$"A{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"B{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"C{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"D{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"E{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"F{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"G{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"H{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"I{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"J{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"K{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"L{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"M{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"N{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"O{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"P{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"Q{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"R{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"S{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"T{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"U{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            foreach (var order in model.List)
            {
                foreach (var subOrder in order.SubAccountOrders)
                {
                    foreach (var orderProduct in subOrder.AccountOrderProducts)
                    {
                        row++;
                        sheet.Cells[$"A{row}"].Value = (row - 1);
                        sheet.Cells[$"B{row}"].Value = subOrder.Datetime.ToPersian();
                        sheet.Cells[$"C{row}"].Value = subOrder.StoreName;
                        sheet.Cells[$"D{row}"].Value = order.Id;
                        sheet.Cells[$"E{row}"].Value = subOrder.Id;
                        sheet.Cells[$"F{row}"].Value = subOrder.Account.FullName;
                        sheet.Cells[$"G{row}"].Value = orderProduct.Product.Name;
                        sheet.Cells[$"H{row}"].Value = orderProduct.Size != null ? orderProduct.Size.Name : "-";
                        sheet.Cells[$"I{row}"].Value = orderProduct.Color != null ? orderProduct.Color.Name : "-";
                        sheet.Cells[$"J{row}"].Value = orderProduct.ProductOptionItem != null ? orderProduct.ProductOptionItem.OptionName + " : " + orderProduct.ProductOptionItem.Value : "-";
                        sheet.Cells[$"K{row}"].Value = (orderProduct.ProductPrice.HasValue ? orderProduct.ProductPrice * 10 : 0);
                        sheet.Cells[$"L{row}"].Value = (orderProduct.ProductDiscount.HasValue ? orderProduct.ProductDiscount * 10 : 0);
                        double discountPercent = 0;
                        if (orderProduct.ProductDiscount > 0)
                        {
                            discountPercent = (((orderProduct.ProductPrice.Value * orderProduct.Count) - orderProduct.Price) * 100) / (orderProduct.ProductPrice.Value * orderProduct.Count);
                        }
                        sheet.Cells[$"M{row}"].Value = Math.Round(discountPercent, 0);
                        sheet.Cells[$"N{row}"].Value = orderProduct.Count;
                        sheet.Cells[$"O{row}"].Value = (orderProduct.Price * 10);

                        sheet.Cells[$"P{row}"].Value = (order.Rebate != null ? order.Rebate.Name : " - ");
                        sheet.Cells[$"Q{row}"].Value = ((order.Rebate != null && order.Rebate.PercentValue.HasValue) ? order.Rebate.PercentValue : 0);
                        sheet.Cells[$"R{row}"].Value = ((order.Rebate != null && order.Rebate.PriceValue.HasValue) ? order.Rebate.PriceValue * 10 : 0);
                        var priceWithRebate = orderProduct.Price;
                        if (order.Rebate != null)
                        {
                            if (order.Rebate.PercentValue.HasValue)
                            {
                                priceWithRebate = priceWithRebate - (priceWithRebate * ((double)order.Rebate.PercentValue / 100));

                            }
                            else if (order.Rebate.PriceValue.HasValue)
                            {
                                var productRebate = order.Rebate.PriceValue.Value / order.ProductsTotalCount;
                                priceWithRebate = priceWithRebate - productRebate;
                            }
                        }
                        sheet.Cells[$"S{row}"].Value = (priceWithRebate * 10);

                        sheet.Cells[$"T{row}"].Value = (subOrder.SendPrice.HasValue ? ((subOrder.SendPrice.Value / subOrder.ProductsCount) * 10) : 0);
                        sheet.Cells[$"U{row}"].Value = subOrder.Status.Name;


                        sheet.Cells[$"A{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"B{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"C{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"D{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"E{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"F{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"G{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"H{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"I{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"J{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"K{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"L{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"M{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"N{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"O{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"P{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"Q{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"R{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"S{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"T{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[$"U{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        sheet.Cells[$"K{row}"].Style.Numberformat.Format = "#,##0";
                        sheet.Cells[$"L{row}"].Style.Numberformat.Format = "#,##0";
                        sheet.Cells[$"O{row}"].Style.Numberformat.Format = "#,##0";
                        sheet.Cells[$"R{row}"].Style.Numberformat.Format = "#,##0";
                        sheet.Cells[$"S{row}"].Style.Numberformat.Format = "#,##0";
                        sheet.Cells[$"T{row}"].Style.Numberformat.Format = "#,##0";

                    }
                }


            }
            sheet.Cells[1, 1, row, 21].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            row++;
            sheet.Cells[$"K{row}"].Formula = $"SUM(K2:K{row - 1})";
            sheet.Cells[$"K{row}"].Style.Numberformat.Format = "#,##0";

            sheet.Cells[$"K{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"K{row}"].Style.Font.Bold = true;
            sheet.Cells[$"L{row}"].Formula = $"SUM(L2:L{row - 1})";
            sheet.Cells[$"L{row}"].Style.Numberformat.Format = "#,##0";

            sheet.Cells[$"L{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"L{row}"].Style.Font.Bold = true;
            sheet.Cells[$"O{row}"].Formula = $"SUM(O2:O{row - 1})";
            sheet.Cells[$"O{row}"].Style.Numberformat.Format = "#,##0";

            sheet.Cells[$"O{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"O{row}"].Style.Font.Bold = true;
            sheet.Cells[$"R{row}"].Formula = $"SUM(R2:R{row - 1})";
            sheet.Cells[$"R{row}"].Style.Numberformat.Format = "#,##0";
            sheet.Cells[$"R{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"R{row}"].Style.Font.Bold = true;
            sheet.Cells[$"S{row}"].Formula = $"SUM(S2:S{row - 1})";
            sheet.Cells[$"S{row}"].Style.Numberformat.Format = "#,##0";
            sheet.Cells[$"S{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"S{row}"].Style.Font.Bold = true;
            sheet.Cells[$"T{row}"].Formula = $"SUM(T2:T{row - 1})";
            sheet.Cells[$"T{row}"].Style.Numberformat.Format = "#,##0";
            sheet.Cells[$"T{row}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[$"T{row}"].Style.Font.Bold = true;

            sheet.Cells[1, 1, 1, 21].AutoFilter = true;
            sheet.Cells[1, 1, 1, 21].Style.Font.Bold = true;
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Orderslist" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");
            Response.BinaryWrite(excelPackage.GetAsByteArray());
            Response.End();
        }
        public ActionResult AddAddress()
        {
            var account = _context.Account.Where(s => s.AccountAddress.Count() > 0 && s.AccountOrder.Count() == 0 && s.AccountBasket.Count()>0).ToList();
            return View(account);
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