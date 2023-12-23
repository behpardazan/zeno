using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Helpers.Merchant
{
    public class BaseNextPay : BasePayment
    {
        public static string StartPayment(UnitOfWork _context, Payment payment)
        {
            string host = GetHost();
            string redirectAddress = payment.Merchant.ExternalInfo2 + "/payment/callback?id=" + payment.ID;

            try
            {
                Services.OnlinePayments.NextPay.ApplicationClient application = new Services.OnlinePayments.NextPay.ApplicationClient();
                Services.OnlinePayments.NextPay.TokenGenerator generator = new Services.OnlinePayments.NextPay.TokenGenerator();
                generator.amount = payment.Amount.ToString();
                generator.api_key = payment.Merchant.MerchantId;
                generator.callback_uri = redirectAddress;
                generator.order_id = payment.ID.ToString();
                Services.OnlinePayments.NextPay.TokenGeneratorResponse result = application.TokenGenerator(generator);
                if (result.TokenGeneratorResult.code == "-1")
                {
                    return GetForm(payment.Merchant.Bank.PaymentUrl + result.TokenGeneratorResult.trans_id);
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string StartPaymentLadder(UnitOfWork _context, LadderPayment payment)
        {
            string host = GetHost();
            string redirectAddress = payment.Merchant.ExternalInfo2 + "/LadderPayment/callback?id=" + payment.ID;

            try
            {
                Services.OnlinePayments.NextPay.ApplicationClient application = new Services.OnlinePayments.NextPay.ApplicationClient();
                Services.OnlinePayments.NextPay.TokenGenerator generator = new Services.OnlinePayments.NextPay.TokenGenerator();
                generator.amount = payment.Amount.ToString();
                generator.api_key = payment.Merchant.MerchantId;
                generator.callback_uri = redirectAddress;
                generator.order_id = payment.ID.ToString();
                Services.OnlinePayments.NextPay.TokenGeneratorResponse result = application.TokenGenerator(generator);
                if (result.TokenGeneratorResult.code == "-1")
                {
                    return GetForm(payment.Merchant.Bank.PaymentUrl + result.TokenGeneratorResult.trans_id);
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            try
            {
                var transid = HttpContext.Current.Request.Form["trans_id"].ToString();
                var orderid = HttpContext.Current.Request.Form["order_id"].ToString();
                Services.OnlinePayments.NextPay.ApplicationClient application = new Services.OnlinePayments.NextPay.ApplicationClient();
                Services.OnlinePayments.NextPay.PaymentVerification verification = new Services.OnlinePayments.NextPay.PaymentVerification();
                verification.amount = payment.Amount.ToString();
                verification.api_key = payment.Merchant.MerchantId;
                verification.trans_id = transid;
                verification.order_id = orderid;
                payment.RefNumber = transid;
                var resultverify = application.PaymentVerification(verification);
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                if (resultverify.PaymentVerificationResult.code == "0")
                {
                    try
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        _context.Payment.DoPaymentServices(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);

                payment.ExternalInfo1 = resultverify.PaymentVerificationResult.card_holder;
                payment.ExternalInfo2 = resultverify.PaymentVerificationResult.message;
                payment.ExternalInfo3 = resultverify.PaymentVerificationResult.code;
            }
            catch
            {
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
            }

            _context.Save();
            payment = _context.Payment.GetById(payment.ID);
            return payment;
        }
        public static LadderPayment DoCallBackLadder(UnitOfWork _context, LadderPayment payment)
        {
            try
            {
                var transid = HttpContext.Current.Request.Form["trans_id"].ToString();
                var orderid = HttpContext.Current.Request.Form["order_id"].ToString();
                Services.OnlinePayments.NextPay.ApplicationClient application = new Services.OnlinePayments.NextPay.ApplicationClient();
                Services.OnlinePayments.NextPay.PaymentVerification verification = new Services.OnlinePayments.NextPay.PaymentVerification();
                verification.amount = payment.Amount.ToString();
                verification.api_key = payment.Merchant.MerchantId;
                verification.trans_id = transid;
                verification.order_id = orderid;
                payment.RefNumber = transid;
                var resultverify = application.PaymentVerification(verification);
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY);
                if (resultverify.PaymentVerificationResult.code == "0")
                {
                    try
                    {
                        try
                        {
                            var account = _context.Account.GetById(payment.AccountId);

                            if (payment.ProductId != null)
                            {
                                var setting = _context.LadderSetting.GetById(payment.LedderSettingId);
                                var product = _context.Product.GetById(payment.ProductId);
                                product.LadderDate = DateTime.Now;
                                product.LadderDateExpire = DateTime.Now.AddDays(setting.LadderDays.Value);
                                product.LadderActive = true;
                                _context.Product.Update(product);
                                _context.Save();

                            }

                            _context.LadderPayment.DoLadderPaymentServices(_context, payment);
                            _context.LadderPayment.Save();
                        }
                        catch
                        {

                        }
                    }
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);

                payment.ExternalInfo1 = resultverify.PaymentVerificationResult.card_holder;
                payment.ExternalInfo2 = resultverify.PaymentVerificationResult.message;
                payment.ExternalInfo3 = resultverify.PaymentVerificationResult.code;
            }
            catch
            {
                payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
            }

            _context.Save();
            payment = _context.LadderPayment.GetById(payment.ID);
            return payment;
        }

        private static string GetForm(string url)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head></head>");
            html.AppendLine(string.Format("<body onload=\"document.formpayment.submit()\">"));
            html.AppendLine(string.Format("<form name='formpayment' method='get' action='" + url + "' >"));
            html.AppendLine("</form>");
            html.AppendLine("</body></html>");
            return html.ToString();
        }
    }
}
