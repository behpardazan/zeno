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
    public class LadderPaymentController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Start(int id)
        {
            LadderPayment payment = _context.LadderPayment.GetById(id);
            bool isLink = false;

            if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_INSERTED.ToString())
            {
                string html = "";
                try
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_STARTED);
                    _context.Payment.Save();

                    if (payment.Merchant.Bank.Label == Enum_Bank.PASARGAD.ToString())
                    {
                        html = BasePasargad.StartPaymentLadder(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.UP.ToString())
                    {
                        html = BaseAsanPardakht.StartPaymentLadder(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        html = BaseZarinPal.StartPaymentLadder(_context, payment);
                        isLink = true;
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.NEXTPAY.ToString())
                    {
                        html = BaseNextPay.StartPaymentLadder(_context, payment);
                    }
                   
                    else if (payment.Merchant.Bank.Label == Enum_Bank.EGHTESAD_NOVIN.ToString())
                    {
                        html = BaseEghtesadNovin.StartPaymentLadder(_context, payment);
                    }
                   
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_START_ERROR);
                    _context.LadderPayment.Update(payment);
                    _context.Save();
                }
                ViewBag.PaymentForm = html;

                if (isLink)
                    return Redirect(html);

                return PartialView("~/Views/Default/Payment/Start.cshtml");

            }
            else
            {
                
                return Redirect("/");
            }
        }
        public ActionResult Callback(int id = 0, string status = "", string amount = "", string merchantId = "", string currency = "")
        {
            LadderPayment payment = new LadderPayment();
            // CASHBILL
            if (id == 0)
            {
                var url = Request.Url.AbsoluteUri;
                Uri myUri = new Uri(url);
                string userdata = HttpUtility.ParseQueryString(myUri.Query).Get("userdata");
                id = int.Parse(userdata);
            }
            payment = _context.LadderPayment.GetById(id);
            if (payment != null)
            {
                if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_STARTED.ToString())
                {

                    if (payment.Merchant.Bank.Label == Enum_Bank.ZARINPAL.ToString())
                    {
                        BaseZarinPal.DoCallBackLadder(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.NEXTPAY.ToString())
                    {
                        BaseNextPay.DoCallBackLadder(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.EGHTESAD_NOVIN.ToString())
                    {
                        BaseEghtesadNovin.DoCallBackLadder(_context, payment);
                    }
                    else if (payment.Merchant.Bank.Label == Enum_Bank.UP.ToString())
                    {
                        BaseAsanPardakht.DoCallBackLadder(_context, payment);
                    }

                }
                //else if (payment.Code.Label == Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString()
                //    //payment.Code.Label == Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString() &&
                //    //payment.PaymentSubject.Label == Enum_PaymentSubject.ORDER.ToString())
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

        public ActionResult Index(int? productId = null)
        {
            return PartialView(BaseController.GetView(this), productId);
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




