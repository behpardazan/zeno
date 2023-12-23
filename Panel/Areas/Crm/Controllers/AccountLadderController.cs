using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using DataLayer.Helpers.Common;
using System.Data;

namespace Panel.Areas.Crm.Controllers
{
    public class AccountLadderController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

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
            string pageIndex = null,
            string pageSize = null
            )
        {

            if (statusId == null && status != null)
            {
                statusId = status.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
            }

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountLadder);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();

            ViewBag.FromOrderId = fromorderid;
            ViewBag.ToOrderId = toorderid;
            ViewBag.StatusId_2 = statusId;
            ViewBag.Status = status;
            ViewBag.FromDatetime = from;
            ViewBag.ToDatetime = to;
            ViewBag.RefId = refid;
            ViewBag.Customer = customer;
            ViewBag.SelectedId = 0;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.MerchantId = merchantId;

            FillDropDowns();
            var test = _context.LadderPayment.Where(s => s.ID != 0).ToList();
            ViewBag.TotalCount = _context.LadderPayment.SearchCount(
                    merchantId: merchantId,
                 
                    statusId: statusId,
                    refId: refid,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                   
                    toorderid: toorderid);

            List<LadderPayment> listOrder = _context.LadderPayment.Search(
                    merchantId: merchantId,
                    statusId: statusId,
                    pageSize: size,
                    
                    index: index,
                    refId: refid,
                   
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                  
                    toorderid: toorderid)
                .ToList();
            ViewBag.SumPrice = listOrder.Sum(s => s.Amount);


            return View(listOrder.ToView());
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountLadder_Details);
            LadderPayment order = _context.LadderPayment.GetById(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(order.StatusId);
            return View(order);
        }



        [HttpPost]
        public ActionResult ChangeStatus(int OrderId, int StatusId, string PostalCode, string Description, string SmsBody)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountLadder_Details);
            LadderPayment order = _context.LadderPayment.GetById(OrderId);
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
            
                order.Description = Description;
               
                _context.LadderPayment.Update(order);
                _context.Save();

               
            }

            FillDropDowns(order.StatusId);

            return RedirectToAction("Index", new { status = last_status });
        }

        public ActionResult Print(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountLadder_Details);
            LadderPayment order = _context.LadderPayment.GetById(id);
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

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountLadder_Details);
            LadderPayment order = _context.LadderPayment.GetById(id);
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
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountLadder_Details);
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
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountLadder_Details);
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



        public ActionResult ExportExcel(
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
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountLadder_Details);

            DateTime fromDate = string.IsNullOrEmpty(from) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(from))).GetValueOrDefault();
            DateTime toDate = string.IsNullOrEmpty(to) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(to))).GetValueOrDefault();
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            int[] statusId = status.Contains("-") ? status.Split('-').Select(n => Convert.ToInt32(n)).ToArray() : new int[] { status.GetInteger() };

            List<AccountOrder> listOrders = _context.AccountOrder.Search(
                    index: index,
                    pageSize: size,
                    statusId: statusId.ToArray(),
                    merchantId: merchantId,
                    refId: refid,
                    customer: customer,
                    fromDatetime: fromDate,
                    toDatetime: toDate,
                    fromorderid: fromorderid,
                    rebate: rebate,
                    toorderid: toorderid)
                .OrderByDescending(p => p.ID)
                .ToList();

            DataTable table = new DataTable();
            table.Columns.Add("ردیف");
            table.Columns.Add("شماره فاکتور");
            table.Columns.Add("نام و نام خانوادگی");
            table.Columns.Add("تلفن");
            table.Columns.Add("استان");
            table.Columns.Add("آدرس");
            table.Columns.Add("کدپستی");
            table.Columns.Add("محصولات");
            table.Columns.Add("تعداد");
            table.Columns.Add("هزینه ارسال");
            table.Columns.Add("تخفیف");
            table.Columns.Add("مبلغ بدون ارسال");
            table.Columns.Add("درصد کل تخفیف");
            table.Columns.Add("مبلغ کل (تومان)");
            table.Columns.Add("نحوه ارسال");
            table.Columns.Add("تاریخ");
            table.Columns.Add("زمان");
            table.Columns.Add("کد رهگیری بانک");
            table.Columns.Add("درگاه بانک");
            table.Columns.Add("کدرهگیری پستی");

            for (int i = 0; i < listOrders.Count; i++)
            {
                AccountOrder order = listOrders[i];
                StringBuilder products = new StringBuilder();
                int count = 0;
                double sumOnlyProductPrice = 0;
                foreach (AccountOrderProduct item in order.AccountOrderProduct)
                {
                    products.AppendLine(item.Product.Name + "(" + item.Count + " عدد)");
                    count = count + item.Count;
                    sumOnlyProductPrice += item.Price;
                }

                StringBuilder phones = new StringBuilder();
                phones.AppendLine(order.Account.Mobile.GetEnglish());
                string stateValue = "-";
                string addressValue = "-";
                string postalCode = "-";
                string sendType = "-";
                string refNumber = "-";
                string merchantName = "-";
                string discount = order.DiscountPrice.GetCurrencyFormat();
                string price = order.Price.GetCurrencyFormat();
                string sendPrice = order.SendPrice.GetCurrencyFormat();
                string priceWithoutSend = (order.Price - order.SendPrice).GetCurrencyFormat();
                string discountPercent = sumOnlyProductPrice != 0 ? ((order.DiscountPrice / sumOnlyProductPrice) * 100).GetCurrencyFormat() : "0";

                if (order.AddressId != null)
                {
                    if (order.Account.Mobile != null && order.Account.Mobile.Contains(order.AccountAddress.Mobile) == false)
                        phones.AppendLine(order.AccountAddress.Mobile.GetEnglish());
                    if (order.Account.Phone != null && order.Account.Phone.Contains(order.AccountAddress.Phone) == false)
                        phones.AppendLine(order.AccountAddress.Phone.GetEnglish());
                    if (order.AccountAddress.StateId != null)
                        stateValue = order.AccountAddress.State.Name;
                    addressValue = order.AccountAddress.AddressValue;
                    postalCode = order.AccountAddress.PostalCode.GetEnglish();
                }
                if (order.SendTypeId != null)
                {
                    sendType = order.SendType.Name;
                }
                if (order.PaymentProductOrder.Count > 0)
                {
                    string payment_success_status = Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString();
                    string payment_service_failed = Enum_Code.PAYMENT_STATUS_SERVICE_FAILED.ToString();
                    PaymentProductOrder payment = order.PaymentProductOrder.FirstOrDefault(p =>
                        p.Payment.Code.Label == payment_success_status ||
                        p.Payment.Code.Label == payment_service_failed);
                    if (payment != null)
                    {
                        refNumber = payment.Payment.RefNumber;
                        merchantName = payment.Payment.Merchant.Bank.Name;
                    }
                }

                table.Rows.Add(
                    (i + 1),
                    order.ID,
                    order.Account.FullName,
                    phones.ToString(),
                    stateValue,
                    addressValue,
                    postalCode,
                    products,
                    count,
                    sendPrice,
                    discount,
                    priceWithoutSend,
                    discountPercent,
                    price,
                    sendType,
                    order.Datetime.ToPersian(),
                    order.Datetime.ToShortTimeString().ToPersian(),
                    refNumber,
                    merchantName,
                    order.PostalCode.ToPersian());

            }

            BaseExcel.ExportExcel(table);
            return View(listOrders);
        }
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
            var a= Json(model ,JsonRequestBehavior.AllowGet);
            return a;
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