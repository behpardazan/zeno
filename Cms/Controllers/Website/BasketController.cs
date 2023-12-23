using DataLayer.Base;
using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class BasketController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            List<AccountBasket> listBasket = new List<AccountBasket>();
            if (account != null)
                listBasket = _context.AccountBasket.GetAllByAccount(account.Id);
            else
                listBasket = _context.AccountBasket.GetAllFromCookie();
            ViewBag.PaymentType = _context.PaymentType.GetAllActive();
            return View(BaseController.GetView(this), listBasket);
        }

        public ActionResult Search(string bsviewname = null)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            List<AccountBasket> listBasket = new List<AccountBasket>();
            if (account != null)
                listBasket = _context.AccountBasket.GetAllByAccount(account.Id);
            else
                listBasket = _context.AccountBasket.GetAllFromCookie();
            return PartialView(BaseController.GetView(this, bsviewname), listBasket);
        }

        //public ActionResult Address(int id)
        //{
        //    ViewAccount account = _context.Account.GetCurrentAccount();
        //    if (account == null)
        //        return RedirectToAction("login", new { controller = "account", back = "basket/address/" + id });

        //    ViewBag.StateId = new SelectList(_context.State.GetAll(), "ID", "Name", 0);
        //    AccountOrder order = _context.AccountOrder.GetById(id);
        //    if (order.AccountId != account.Id)
        //        return Redirect("/");

        //    ViewBag.OrderId = order.ID;
        //    return PartialView(BaseController.GetView(this), order);
        //}
        public ActionResult Address()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "basket/address/" });
            return PartialView(BaseController.GetView(this));
        }
        [HttpPost]
        public ActionResult SaveAddress(int orderId, string Description, string GiftDescription, int? addressId = null, bool HasFactor = false, bool IsGift = false, string nextLevel = "")
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "basket/address/" + orderId });
            else if (addressId == 0 || addressId == null)
                return RedirectToAction("address", new { controller = "basket", id = orderId });

            AccountOrder order = _context.AccountOrder.GetById(orderId);
            order.AddressId = addressId;

            order.HasFactor = HasFactor;
            order.GiftDescription = GiftDescription;
            order.IsGift = IsGift;
            order.Description = Description;

            order.StatusId = _context.Code.GetIdByLabel(Enum_Code.ORDER_STATUS_ADDRESS);
            _context.AccountOrder.Update(order);
            _context.Save();
            return RedirectToAction(nextLevel, new { id = order.ID });
        }

        //public ActionResult Confirm(int id)
        //{
        //    ViewAccount account = _context.Account.GetCurrentAccount();
        //    if (account == null)
        //        return RedirectToAction("login", new { controller = "account", back = "basket/confirm/" });
        //    AccountOrder order = _context.AccountOrder.GetById(id);
        //    if (account.Id != order.AccountId)
        //        return Redirect("/");

        //    ViewBag.OrderId = order.ID;
        //    ViewBag.Merchant = _context.Merchant.GetAll().Where(p => p.Active == true).ToList();
        //    return PartialView(BaseController.GetView(this), order);
        //}
        public ActionResult Confirm(bool ?onlineProduct=null)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "basket/confirm/" });
            //else
            //{

            //}
            return PartialView(BaseController.GetView(this));
        }
        public ActionResult Local(int id)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "basket/confirm/" + id });
            AccountOrder order = _context.AccountOrder.GetById(id);
            if (account.Id != order.AccountId)
                return Redirect("/");

            ViewBag.OrderId = order.ID;
            return PartialView(BaseController.GetView(this), order);
        }

        public ActionResult NewOrder(string nextLevel = "startpayment", int? SendTypeId = null, int? addressId = null, string rebateValue = null, int? paymentTypeId = null, int? merchantId = null, bool store = false, bool IsGift = false, string GiftDescription = null, bool HasFactor = false, string Description = null,bool ? IsCreditShoping=null)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("Index", new { controller = "login", back = "basket" });
            else
            {
                ApiResult result = new ApiResult();
                result = ApiAccountOrder.Post(_context: _context, paymentTypeId: paymentTypeId, accountId: account.Id, sendTypeId: SendTypeId, rebateValue: rebateValue, addressId: addressId, IsGift: IsGift, GiftDescription: GiftDescription, HasFactor: HasFactor, Description: Description, store: store, IsCreditShoping: IsCreditShoping);
                if (result.Code == ApiResult.ResponseCode.Success)
                {
                    ViewAccountOrder order = (ViewAccountOrder)result.Value;
                    if (nextLevel != "startpayment")
                    {
                        return RedirectToAction(nextLevel, new { id = order.Id });
                    }
                    else
                    {
                        return RedirectToAction(nextLevel, new { orderId = order.Id, merchantId = merchantId });
                    }
                }
                else
                    return RedirectToAction("index");
            }
        }
        public ActionResult NewLadderOrder(string nextLevel = "startLeaderpayment", int? productId = null, int? merchantId = null, int? LadderSettingId = null)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("Index", new { controller = "login", back = "basket" });
            else
            {

                return RedirectToAction(nextLevel, new { productId = productId, merchantId = merchantId, LadderSettingId = LadderSettingId });
            }

        }


        [HttpPost]
        public ActionResult Add(int productId, string backurl = null)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
            {
                return RedirectToAction("login", "account", new { back = backurl });
            }
            else
            {
                ApiBasket.Post(_context: _context, accountId: account.Id, productId: productId);
                return Redirect(backurl);
            }
        }

        //[HttpPost]
        public ActionResult StartPayment(int orderId, int? merchantId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });
            else
            {
                AccountOrder order = _context.AccountOrder.GetById(orderId);
                if (order.PaymentType.Label == Enum_PaymentType.PLACE.ToString())
                {
                    ViewPayment payment = new ViewPayment()
                    {
                        Description = "پرداخت سفارش با کد " + orderId + " (پرداخت در محل)",
                        OrderId = orderId,
                        Price = order.Price,
                        PaymentSubject = new ViewPaymentSubject()
                        {
                            Label = Enum_PaymentSubject.ORDER.ToString()
                        },
                        Status = new ViewCode()
                        {
                            Label = Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString()
                        }
                    };
                    ApiResult result = ApiPayment.Post(_context, account.Id, null, payment);
                    ViewPayment tempPayment = (ViewPayment)result.Value;
                    return RedirectToAction("callback", new { controller = "payment", id = tempPayment.Id });
                }
                else if (order.IsCreditShoping==true)
                {
                    ViewPayment payment = new ViewPayment()
                    {
                        Description = "پرداخت سفارش با کد " + orderId + " (پرداخت به صورت اعتباری)",
                        OrderId = orderId,
                        Price = order.Price,
                        PaymentSubject = new ViewPaymentSubject()
                        {
                            Label = Enum_PaymentSubject.ORDER.ToString()
                        },
                        Status = new ViewCode()
                        {
                            Label = Enum_Code.PAYMENT_STATUS_SUCCESSFUL.ToString()
                        }
                    };
                    ApiResult result = ApiPayment.Post(_context, account.Id, null, payment);
                    ViewPayment tempPayment = (ViewPayment)result.Value;
                    return RedirectToAction("callback", new { controller = "payment", id = tempPayment.Id });
                }
                else
                {
                    if (merchantId == null)
                        //merchantId = _context.Merchant.FirstOrDefault(p => p.Active).ID;
                        merchantId = _context.Merchant.FirstOrDefault(p => p.Bank.Label == DataLayer.Enumarables.Enum_Bank.FONDY.ToString()).ID;


                    ViewPayment payment = new ViewPayment()
                    {
                        Description = "Your Order Code : " + orderId,
                        OrderId = orderId,
                        Price = order.Price,
                        PaymentSubject = new ViewPaymentSubject()
                        {
                            Label = Enum_PaymentSubject.ORDER.ToString()
                        },
                        Merchant = new ViewMerchant()
                        {
                            Id = merchantId.GetValueOrDefault()
                        }
                    };
                    ApiResult result = ApiPayment.Post(_context, account.Id, null, payment);
                    ViewPayment tempPayment = (ViewPayment)result.Value;
                    return RedirectToAction("start", new { controller = "payment", id = tempPayment.Id });
                }
            }
        }
        public ActionResult startLeaderpayment(int LadderSettingId, int? merchantId, int? productId = null)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });
            else
            {
                if (merchantId == null)
                    merchantId = _context.Merchant.FirstOrDefault(p => p.Active).ID;
                var laddersetting = _context.LadderSetting.Where(s => s.ID == LadderSettingId && s.Active == true).FirstOrDefault();

                ViewLadderPayment payment = new ViewLadderPayment()
                {
                    LadderSettingId = LadderSettingId,
                    Description = "Your Order Code : " + productId,
                    ProductId = productId,
                    Price = laddersetting.LadderPrice != null ? laddersetting.LadderPrice.Value : 0,
                    PaymentSubject = new ViewPaymentSubject()
                    {
                        Label = Enum_PaymentSubject.ORDER.ToString()
                    },
                    Merchant = new ViewMerchant()
                    {
                        Id = merchantId.GetValueOrDefault()
                    }
                };
                ApiResult result = ApiLadderPayment.Post(_context, account.Id, null, payment);
                ViewLadderPayment tempPayment = (ViewLadderPayment)result.Value;
                return RedirectToAction("start", new { controller = "LadderPayment", id = tempPayment.Id });

            }
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