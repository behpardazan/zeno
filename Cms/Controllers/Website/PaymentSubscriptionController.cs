using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Helpers.Merchant;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.ViewModels;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using DataLayer.Base;

namespace Cms.Controllers.Website
{
    public class PaymentSubscriptionController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult index()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            ShippingSubscriptionPayment payment = new ShippingSubscriptionPayment();
            ViewShippingSubscriptionPayment viewPayment = new ViewShippingSubscriptionPayment();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            var AddressList = _context.AccountAddress.Where(s => s.AccountId == account.ID).OrderByDescending(s => s.ID).ToList();
            if (AddressList.Count != 0)
            {
                var address = AddressList.FirstOrDefault();
                var shippingSubscription = _context.ShippingSubscription.Where(s => s.StateId == address.StateId).FirstOrDefault();
                if (shippingSubscription != null)
                {
                    payment.Amount = shippingSubscription.Price;
                    payment.AccountId = account.ID;
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_INSERTED);
                    payment.MerchantId = 1;
                    payment.SubjectId = 1;
                    payment.AccountAddressId = address.ID;

                    payment.Datetime = DateTime.Now;

                    payment.IpAddress = BaseSecurity.GetClientIPAddress();
                    _context.ShippingSubscriptionPayment.Insert(payment);
                    _context.Save();
                    viewPayment.Address = new ViewAccountAddress(address);
                    viewPayment.Price = payment.Amount;
                    viewPayment.Id = payment.ID;
                }
                else
                {
                    return RedirectToAction("index", new { controller = "home" });

                }
            }
            else
            {
                return RedirectToAction("index", new { controller = "home" });
            }
            return PartialView(BaseController.GetView(this), viewPayment);
        }
        public ActionResult Start(int id, int merchantId, bool isMobile = false)
        {
            ShippingSubscriptionPayment payment = _context.ShippingSubscriptionPayment.GetById(id);
            bool isLink = false;
            payment.IsMobile = isMobile;
            payment.MerchantId = merchantId;

            payment.Description = "Your Order Code : " + id;
            payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_INSERTED);
            _context.ShippingSubscriptionPayment.Update(payment);
            _context.Save();
            if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_INSERTED.ToString())
            {
                string html = "";
                try
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
                    _context.ShippingSubscriptionPayment.Save();
                    //if (payment.Merchant.Bank.Label == Enum_Bank.PASARGAD.ToString())
                    //{
                    //    html = BasePasargad.StartPayment(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.UP.ToString())
                    //{
                    //    html = BaseAsanPardakht.StartPayment(_context, payment);
                    //}
                    /*else*/
                    if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        html = BaseZarinPal.StartPayment2(_context, payment);
                        isLink = true;
                    }
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.NEXTPAY.ToString())
                    //{
                    //    html = BaseNextPay.StartPayment(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.MELLAT.ToString())
                    //{
                    //    html = BaseBehPardakht.StartPayment(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.SAMAN.ToString())
                    //{
                    //    html = BaseSaman.StartPayment(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.PARSIAN.ToString())
                    //{
                    //    html = BaseParsian.StartPayment(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.MELLI.ToString())
                    //{
                    //    html = BaseMelli.StartPayment(_context, payment);
                    //    isLink = true;
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.PAYPAL.ToString())
                    //{
                    //    html = BasePayPal.StartPayment(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.EGHTESAD_NOVIN.ToString())
                    //{
                    //    html = BaseEghtesadNovin.StartPayment(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.CASHBILL.ToString())
                    //{
                    //    html = BaseCashbill.StartPayment(_context, payment);
                    //    if (payment.PaymentSubject.Label == "EXTERNAL")
                    //    {
                    //        ViewBag.PaymentForm = html;
                    //        ViewBag.PaymentId = payment.ID.ToString();
                    //        ViewBag.Currency = payment.CurrencyTypeId.HasValue ? payment.Code1.Label : "";
                    //        ViewBag.Domain = payment.PaymentWebsiteId.HasValue ? payment.PaymentWebsite.Domain : "";
                    //        ViewBag.Amount = payment.Amount;
                    //        ViewBag.PaymentGateway = Enum_Bank.CASHBILL.ToString();
                    //        return PartialView(BaseController.GetView(this));
                    //    }
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.FONDY.ToString())
                    //{
                    //    html = BaseFondy.StartPayment(_context, payment);
                    //    if (payment.PaymentSubject.Label == "EXTERNAL")
                    //    {
                    //        ViewBag.PaymentForm = html;
                    //        ViewBag.PaymentId = payment.ID.ToString();
                    //        ViewBag.Currency = "EUR";
                    //        ViewBag.Domain = payment.PaymentWebsiteId.HasValue ? payment.PaymentWebsite.Domain : "";
                    //        ViewBag.Amount = payment.Amount;
                    //        ViewBag.PaymentGateway = Enum_Bank.FONDY.ToString();
                    //        return PartialView(BaseController.GetView(this));
                    //    }
                    //}
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                    _context.ShippingSubscriptionPayment.Update(payment);
                    _context.Save();
                }
                ViewBag.PaymentForm = html;

                if (isLink)
                    return Redirect(html);

                return PartialView("~/Views/Default/Payment/Start.cshtml");

            }
            else
            {
                if (payment.PaymentSubject.Label == "EXTERNAL")
                {
                    var baseWebsiteUrl = $"https://{payment.PaymentWebsite.Domain}/";
                    return Redirect(baseWebsiteUrl);
                }
                return Redirect("/");
            }
        }
        public ActionResult Callback(int id = 0, string status = "", string amount = "", string merchantId = "", string currency = "")
        {
            ShippingSubscriptionPayment payment = new ShippingSubscriptionPayment();
            // CASHBILL
            if (id == 0)
            {
                var url = Request.Url.AbsoluteUri;
                Uri myUri = new Uri(url);
                string userdata = HttpUtility.ParseQueryString(myUri.Query).Get("userdata");
                id = int.Parse(userdata);
            }
            payment = _context.ShippingSubscriptionPayment.GetById(id);
            if (payment != null)
            {
                if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_STARTED.ToString())
                {
                    //if (payment.Merchant.Bank.Label == Enum_Bank.PASARGAD.ToString())
                    //{
                    //    BasePasargad.DoCallBack(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.UP.ToString())
                    //{
                    //    // DO CALLBACK
                    //}
                    //else
                    if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        BaseZarinPal.DoCallBack2(_context, payment);
                    }
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.NEXTPAY.ToString())
                    //{
                    //    BaseNextPay.DoCallBack(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.MELLAT.ToString())
                    //{
                    //    BaseBehPardakht.DoCallBack(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.SAMAN.ToString())
                    //{
                    //    BaseSaman.DoCallBack(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.PARSIAN.ToString())
                    //{
                    //    BaseParsian.DoCallBack(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.MELLI.ToString())
                    //{
                    //    BaseMelli.DoCallBack(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.PAYPAL.ToString())
                    //{
                    //    BasePayPal.DoCallBack(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.CASHBILL.ToString())
                    //{
                    //    BaseCashbill.DoCallBack(_context, payment);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.FONDY.ToString())
                    //{
                    //    BaseFondy.DoCallBack(_context, payment, status, amount, merchantId, currency);
                    //}
                    //else if (payment.Merchant.Bank.Label == Enum_Bank.EGHTESAD_NOVIN.ToString())
                    //{
                    //    BaseEghtesadNovin.DoCallBack(_context, payment);
                    //}

                }
                //else if (
                //    payment.Code.Label == Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString() &&
                //    payment.PaymentSubject.Label == Enum_PaymentSubject.ORDER.ToString())
                //{
                //    PaymentProductOrder orderPayment = payment.PaymentProductOrder.OrderByDescending(p => p.ID).FirstOrDefault();
                //    if (orderPayment.AccountOrder.PaymentType.Label == Enum_PaymentType.PLACE.ToString())
                //    {
                //        _context.Payment.DoPaymentServices(_context, payment);
                //        _context.Payment.Save();
                //    }
                //}

                //if (payment.PaymentSubject.Label == "EXTERNAL")
                //{
                //    return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "ExternalCallbak", payment);

                //}

                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "CallBack", payment);

            }
            else
                return Redirect("/");
        }

        public ActionResult External(string token, string sign)
        {
            var payment = _context.Payment.GetByTokenAndSign(token, sign);
            if (payment != null)
            {
                return Redirect("/payment/start/" + payment.ID);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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




