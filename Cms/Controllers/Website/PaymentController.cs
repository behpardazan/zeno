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

namespace Cms.Controllers.Website
{
    public class PaymentController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Start(int id,bool isMobile=false)
        {
            Payment payment = _context.Payment.GetById(id);
            bool isLink = false;
            payment.IsMobile = isMobile;
            _context.Payment.Update(payment);
            _context.Save();
            if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_INSERTED.ToString())
            {
                string html = "";
                try
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
                    _context.Payment.Save();
                    if (payment.Merchant.Bank.Label == Enum_Bank.PASARGAD.ToString())
                    {
                        html = BasePasargad.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.UP.ToString())
                    {
                        html = BaseAsanPardakht.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        html = BaseZarinPal.StartPayment(_context, payment);
                        isLink = true;
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.NEXTPAY.ToString())
                    {
                        html = BaseNextPay.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.MELLAT.ToString())
                    {
                        html = BaseBehPardakht.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.SAMAN.ToString())
                    {
                        html = BaseSaman.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PARSIAN.ToString())
                    {
                        html = BaseParsian.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.MELLI.ToString())
                    {
                        html = BaseMelli.StartPayment(_context, payment);
                        isLink = true;
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PAYPAL.ToString())
                    {
                        html = BasePayPal.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.EGHTESAD_NOVIN.ToString())
                    {
                        html = BaseEghtesadNovin.StartPayment(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.CASHBILL.ToString())
                    {
                        html = BaseCashbill.StartPayment(_context, payment);
                        if (payment.PaymentSubject.Label == "EXTERNAL")
                        {
                            ViewBag.PaymentForm = html;
                            ViewBag.PaymentId = payment.ID.ToString();
                            ViewBag.Currency = payment.CurrencyTypeId.HasValue ? payment.Code1.Label : "";
                            ViewBag.Domain = payment.PaymentWebsiteId.HasValue ? payment.PaymentWebsite.Domain : "";
                            ViewBag.Amount = payment.Amount;
                            ViewBag.PaymentGateway = Enum_Bank.CASHBILL.ToString();
                            return PartialView(BaseController.GetView(this));
                        }
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.FONDY.ToString())
                    {
                        html = BaseFondy.StartPayment(_context, payment);
                        if (payment.PaymentSubject.Label == "EXTERNAL")
                        {
                            ViewBag.PaymentForm = html;
                            ViewBag.PaymentId = payment.ID.ToString();
                            ViewBag.Currency = "EUR";
                            ViewBag.Domain = payment.PaymentWebsiteId.HasValue ? payment.PaymentWebsite.Domain : "";
                            ViewBag.Amount = payment.Amount;
                            ViewBag.PaymentGateway = Enum_Bank.FONDY.ToString();
                            return PartialView(BaseController.GetView(this));
                        }
                    }
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                    _context.Payment.Update(payment);
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
            Payment payment = new Payment();
            // CASHBILL
            if (id == 0)
            {
                var url = Request.Url.AbsoluteUri;
                Uri myUri = new Uri(url);
                string userdata = HttpUtility.ParseQueryString(myUri.Query).Get("userdata");
                id = int.Parse(userdata);
            }
            payment = _context.Payment.GetById(id);
            if (payment != null)
            {
                if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_STARTED.ToString())
                {
                    if (payment.Merchant.Bank.Label == Enum_Bank.PASARGAD.ToString())
                    {
                        BasePasargad.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.UP.ToString())
                    {
                        BaseAsanPardakht.DoCallBack(_context, payment);

                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        BaseZarinPal.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.NEXTPAY.ToString())
                    {
                        BaseNextPay.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.MELLAT.ToString())
                    {
                        BaseBehPardakht.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.SAMAN.ToString())
                    {
                        BaseSaman.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PARSIAN.ToString())
                    {
                        BaseParsian.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.MELLI.ToString())
                    {
                        BaseMelli.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.PAYPAL.ToString())
                    {
                        BasePayPal.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.CASHBILL.ToString())
                    {
                        BaseCashbill.DoCallBack(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.FONDY.ToString())
                    {
                        BaseFondy.DoCallBack(_context, payment, status, amount, merchantId, currency);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.EGHTESAD_NOVIN.ToString())
                    {
                        BaseEghtesadNovin.DoCallBack(_context, payment);
                    }

                }
                else if (
                    payment.Code.Label == Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString() &&
                    payment.PaymentSubject.Label == Enum_PaymentSubject.ORDER.ToString())
                {
                    PaymentProductOrder orderPayment = payment.PaymentProductOrder.OrderByDescending(p => p.ID).FirstOrDefault();
                    if (orderPayment.AccountOrder.PaymentType.Label == Enum_PaymentType.PLACE.ToString() || orderPayment.AccountOrder.IsCreditShoping==true)
                    {
                        _context.Payment.DoPaymentServices(_context, payment);
                        _context.Payment.Save();
                    }
                }

                if (payment.PaymentSubject.Label == "EXTERNAL")
                {
                    return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "ExternalCallbak", payment);

                }

                return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "CallBack", payment);

            }
            else
                return Redirect("/");
        }
    
        public ActionResult FondyCallBack(int id)
        {
            try
            {
                string status = Request.Params["response_status"];
                string amount = Request.Params["amount"];
                string merchantId = Request.Params["merchant_id"];
                string currency = Request.Params["currency"];

                return Callback(id, status, amount, merchantId, currency);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                throw;
            }
        }
        public ActionResult FondyResponse(int id)
        {
            try
            {
                string status = Request.Params["response_status"];
                string amount = Request.Params["amount"];
                string merchantId = Request.Params["merchant_id"];
                string currency = Request.Params["currency"];

                var a = _context.PaymentWebsite.GetById(2);
                a.Name = "res " + id + "-" + status + "-" + amount + "-" + merchantId + "-" + currency;
                _context.PaymentWebsite.Update(a);
                _context.Save();
                return Callback(id, status, amount, merchantId, currency);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                throw;
            }
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




