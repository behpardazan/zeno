using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ZarinPal;

namespace DataLayer.Helpers.Merchant
{
    public class BaseZarinPal : BasePayment
    {
        public static string StartPayment(UnitOfWork _context, Payment payment)
        {
            string host = GetHost();
            string redirectAddress = payment.Merchant.ExternalInfo2  + "/payment/callback?id=" + payment.ID;

            try
            {
                ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
                PaymentRequest pr = new PaymentRequest(payment.Merchant.MerchantId, (long)payment.Amount, redirectAddress, payment.Description)
                {
                    Email = payment.Account.Email,
                    Mobile = payment.Account.Mobile
                };

                if (!string.IsNullOrEmpty(payment.Merchant.Username) && payment.Merchant.Username.ToUpper() == "SANDBOX")
                    zarinpal.EnableSandboxMode();
                else
                    zarinpal.DisableSandboxMode();

                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                    return res.PaymentURL;
                else
                    return res.Status.ToString();
        }
            catch
            {
                return null;
            }
        }
        public static string StartPaymentLadder(UnitOfWork _context, LadderPayment payment)
        {
            string host = GetHost();
            string redirectAddress = payment.Merchant.ExternalInfo2 + "/LadderPayment/callback?id=" + payment.ID;

            try
            {
                ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
                PaymentRequest pr = new PaymentRequest(payment.Merchant.MerchantId, (long)payment.Amount, redirectAddress, payment.Description)
                {
                    Email = payment.Account.Email,
                    Mobile = payment.Account.Mobile
                };

                if (!string.IsNullOrEmpty(payment.Merchant.Username) && payment.Merchant.Username.ToUpper() == "SANDBOX")
                    zarinpal.EnableSandboxMode();
                else
                    zarinpal.DisableSandboxMode();

                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                    return res.PaymentURL;
                else
                    return res.Status.ToString();
            }
            catch
            {
                return null;
            }
        }
        public static string StartPayment2(UnitOfWork _context, ShippingSubscriptionPayment payment)
        {
            string host = GetHost();
            string redirectAddress = payment.Merchant.ExternalInfo2 + "/PaymentSubscription/callback?id=" + payment.ID;

            try
            {
                ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
                PaymentRequest pr = new PaymentRequest(payment.Merchant.MerchantId, (long)payment.Amount, redirectAddress, payment.Description)
                {
                    Email = payment.Account.Email,
                    Mobile = payment.Account.Mobile
                };

                if (!string.IsNullOrEmpty(payment.Merchant.Username) && payment.Merchant.Username.ToUpper() == "SANDBOX")
                    zarinpal.EnableSandboxMode();
                else
                    zarinpal.DisableSandboxMode();

                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                    return res.PaymentURL;
                else
                    return res.Status.ToString();
            }
            catch
            {
                return null;
            }
        }


        public static Payment DoCallBack(UnitOfWork _context, Payment payment)
        {
            try
            {
                String Status = HttpContext.Current.Request.QueryString["Status"];
                String Authority = HttpContext.Current.Request.QueryString["Authority"];

                if (Status != "OK")
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }

                var zarinpal = ZarinPal.ZarinPal.Get();
                var pv = new PaymentVerification(payment.Merchant.MerchantId, (long)payment.Amount, Authority);
                var verificationResponse = zarinpal.InvokePaymentVerification(pv);
                if (verificationResponse.Status == 100)
                {
                    try
                    {
                        payment.RefNumber = verificationResponse.RefID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SUCCESSFUL);
                        _context.Payment.DoPaymentServices(_context, payment);
                        _context.Payment.Save();
                    }
                    catch (Exception ex)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }
                _context.Save();
                payment = _context.Payment.GetById(payment.ID);
                return payment;
            }
            catch
            {
                return null;
            }
        }
        public static ShippingSubscriptionPayment DoCallBack2(UnitOfWork _context, ShippingSubscriptionPayment payment)
        {
            try
            {
                String Status = HttpContext.Current.Request.QueryString["Status"];
                String Authority = HttpContext.Current.Request.QueryString["Authority"];

                if (Status != "OK")
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }

                var zarinpal = ZarinPal.ZarinPal.Get();
                var pv = new PaymentVerification(payment.Merchant.MerchantId, (long)payment.Amount, Authority);
                var verificationResponse = zarinpal.InvokePaymentVerification(pv);
                if (verificationResponse.Status == 100)
                {
                    try
                    {
                        payment.RefNumber = verificationResponse.RefID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_SUCCESS);
                        var account = _context.Account.GetById(payment.AccountId);
                        account.ShippingSubscriptionPaymentDate = DateTime.Now;
                        account.ShippingSubscriptionPaymentActive = true;
                        _context.Account.Update(account);
                        _context.Account.Save();
                        _context.Payment.Save();
                    }
                    catch (Exception ex)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }
                _context.Save();
                payment = _context.ShippingSubscriptionPayment.GetById(payment.ID);
                return payment;
            }
            catch
            {
                return null;
            }
        }
        public static LadderPayment DoCallBackLadder(UnitOfWork _context, LadderPayment payment)
        {
            try
            {
                String Status = HttpContext.Current.Request.QueryString["Status"];
                String Authority = HttpContext.Current.Request.QueryString["Authority"];

                if (Status != "OK")
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }

                var zarinpal = ZarinPal.ZarinPal.Get();
                var pv = new PaymentVerification(payment.Merchant.MerchantId, (long)payment.Amount, Authority);
                var verificationResponse = zarinpal.InvokePaymentVerification(pv);
                if (verificationResponse.Status == 100)
                {
                    try
                    {
                        var account = _context.Account.GetById(payment.AccountId);
                        payment.RefNumber = verificationResponse.RefID;
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_SUCCESS);
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
                    catch (Exception)
                    {
                        payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_SERVICE_FAILED);
                    }
                }
                else
                {
                    payment.StatusId = _context.Code.GetIdByLabel(Enum_Code.PAYMENT_STATUS_VERIFY_ERROR);
                }
                _context.Save();
                payment = _context.LadderPayment.GetById(payment.ID);
                return payment;
            }
            catch
            {
                return null;
            }
        }

    }
}
