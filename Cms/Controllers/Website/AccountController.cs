using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Helpers.Google;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;
using System.Text;
using System.IO;
using System.Collections;

namespace Cms.Controllers
{
    public class AccountController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index()
        {
            return RedirectToAction("index", new { controller = "home" });
        }
        public ActionResult Ladder()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            List<LadderPayment> ladderPayments = account.LadderPayment.OrderByDescending(s => s.Datetime).ToList();
            return PartialView(BaseController.GetView(this), ladderPayments);
        }
        public ActionResult PrintOrders(int id)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            AccountOrder results = _context.AccountOrder.Where(
              s => s.AccountId == tempAccount.Id && s.ID == id).FirstOrDefault();
            return PartialView(BaseController.GetView(this), results);

        }
        [HttpGet]
        public ActionResult Message()
        {
            if (_context.Account.GetCurrentAccount() == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/message" });

            ViewAccount account = _context.Account.GetCurrentAccount();
            List<ShopChat> list = _context.ShopChat.GetAllUnreadAndAccountId(account.Id);

            return PartialView(BaseController.GetView(this), list.ToView());
        }

        [HttpGet]
        public ActionResult MessageShop(int id)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/newaddress/" + id });

            Shop shop = _context.Shop.GetById(id);
            ViewBag.CurrentAccount = account;

            _context.ShopChat.DoReadAllChatByShopForAccount(shop, account.Id);
            _context.Save();

            return PartialView(BaseController.GetView(this), shop);
        }

        [HttpPost]
        public ActionResult MessageShop(int ShopId, string Body)
        {
            if (string.IsNullOrEmpty(Body) == false)
            {
                ShopChat chat = new ShopChat()
                {
                    AccountId = _context.Account.GetCurrentAccount().Id,
                    Body = Body,
                    Datetime = DateTime.Now,
                    IsAccountSend = true,
                    IsRead = false,
                    ShopId = ShopId
                };
                _context.ShopChat.Insert(chat);
                _context.Save();
            }
            return RedirectToAction("MessageShop", new { @id = ShopId });
        }
        public ActionResult Question(
       int? qoIndex = 1,
       int? qoSize = 100000)

        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            qoIndex = qoIndex != null ? qoIndex.Value : 1;
            qoSize = qoSize != null ? qoSize.Value : 10000;
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "account/Question" });
            List<Question> list = _context.Question.Search(
             AccountId: account.Id,
             index: qoIndex.Value,
             pageSize: qoSize.Value
                );

            return PartialView(BaseController.GetView(this), list);
        }

        [HttpGet]
        public ActionResult Follow()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/follow" });

            List<ShopFollow> list = _context.ShopFollow.GetAllByAccountId(account.Id);
            return PartialView(BaseController.GetView(this), list);
        }
        [HttpGet]
        public ActionResult ShowCourse(string productId)
        {
            string[] productIds = productId.Split('*');
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/follow" });
            List<ProductVideo> list = new List<ProductVideo>();
            for (int i = 0; i < productIds.Length; i++)
            {

                if (productIds[i] != "")
                {
                    var product = _context.Product.GetById(int.Parse(productIds[i]));
                    var IsPayment = product.AccountOrderProduct.Any(s => s.AccountOrder.AccountId == account.Id && (s.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() || s.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_POST.ToString()));
                    //var listOrder = _context.AccountOrderProduct.Where(s => s.AccountOrder.AccountId == account.Id && s.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString()).ToList();
                    //var listOrder =_c  _context.AccountOrder.Search(status: Enum_Code.ORDER_STATUS_SUCCESS, accountId: account.Id);
                    if (IsPayment == false)
                    {
                        return Redirect("/");
                    }
                    int id = int.Parse(productIds[i]);
                    var listVideo = _context.ProductVideo.Where(s => s.ProductId == id).ToList();
                    foreach (var item in listVideo)
                    {
                        list.Add(item);

                    }
                }

            }

            return PartialView(BaseController.GetView(this), list);
        }
        [HttpGet]
        public ActionResult ShowFile(int fileId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/follow" });

            var file = _context.DashboardFiles.Where(s => s.FileId == fileId).FirstOrDefault();
            return PartialView(BaseController.GetView(this), file);
        }
        [HttpGet]
        public ActionResult Files()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/follow" });

            var files = _context.DashboardFiles.GetAll();
            return PartialView(BaseController.GetView(this), files);
        }
        [HttpGet]
        public ActionResult Report()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "profile/follow" });

            var reports = _context.AccountReport.Where(s => s.AccountId == account.Id).ToList();
            return PartialView(BaseController.GetView(this), reports);
        }
        [HttpGet]
        public ActionResult Tracking()
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            return PartialView(BaseController.GetView(this), account);
        }

        [HttpGet]
        public ActionResult Login(string back = "/")
        {

            if (_context.Account.GetCurrentAccount() == null)
            {
                ViewBag.back = back;
                return PartialView(BaseController.GetView(this));
            }

            if (Request.QueryString["back"] == null)
                return RedirectToAction("profile", new { controller = "account" });
            if (Request.QueryString["back"] == "creditshoping")
            {
                return Redirect(back);

            }
            else
            {
                return RedirectToAction("profile", new { controller = "account", back = Request.QueryString["back"] });

            }
        }

        [HttpGet]
        public ActionResult Logout(string back = "/")
        {
            _context.Account.Logout();
            return Redirect(back);
        }

        [HttpGet]
        public ActionResult NewAddress(string back = "/login")
        {
            _context.Account.Logout();
            return Redirect(back);
        }

        [HttpPost]
        public ActionResult Login(Account account, bool remember = false, string back = "/", FormCollection form = null)
        {
            string gRecaptchaResponse = form["g-recaptcha-response"];
            if (Recaptcha.Check(gRecaptchaResponse))
            {
                ApiResult result = ApiAccount.Post_DoLogin(_context, account, account.Password, true);
                if (result.Code == ApiResult.ResponseCode.Success)
                {
                    _context.Account.SetCurrentAccount((ViewAccount)result.Value, account.Password, remember);
                    return Redirect(back);
                }
                else
                {
                    int? CaptchaCount = ViewBag.CaptchaCount;
                    CaptchaCount = CaptchaCount.GetValueOrDefault() + 1;
                    ViewBag.CaptchaCount = CaptchaCount;
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, result.Message);
                }
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_RECAPTCHA);
            ViewBag.back = back;
            return PartialView(BaseController.GetView(this), account);
        }
        public ActionResult Recover(string k = null, string a = null)
        {
            AccountPasswordForget forget = null;
            if (k != null || a != null)
            {
                string emailKey = k;
                Guid accountKey = a.GetGuid();
                forget = _context.AccountPasswordForget.GetByEmailKeyAndAccountKey(emailKey, accountKey);
                if (forget != null)
                {
                    return PartialView(BaseController.GetView(this), forget);
                }
                else
                    return Redirect("~/error/404");
            }
            else
                return Redirect("~/error/404");
        }

        [HttpPost]
        public ActionResult Recover(AccountPasswordForget forget, string password, string confirm)
        {
            ViewBag.Password = password;
            ViewBag.PasswordConfirm = confirm;
            ApiResult result = ApiAccountPasswordForget.Post_ChangePassword(_context, forget, password, confirm);
            if (result.Code == ApiResult.ResponseCode.Success)
            {
                Account account = (Account)result.Value;
                ViewAccount CurrentAccount = new ViewAccount(account);
                _context.Account.SetCurrentAccount(CurrentAccount, confirm, true);
                return Redirect("/");
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, result.Message);
            return PartialView(BaseController.GetView(this), forget);
        }
        public ActionResult Forget()
        {
            //if (_context.Account.GetCurrentAccount() == null)
            return PartialView(BaseController.GetView(this));
            //return RedirectToAction("profile", new { controller = "account" });
        }

        [HttpPost]
        public ActionResult Forget(string Email, string Mobile, FormCollection form = null)
        {
            string gRecaptchaResponse = form["g-recaptcha-response"];
            if (Recaptcha.Check(gRecaptchaResponse))
            {
                Account account = new Account()
                {
                    Email = Email,
                    Mobile = Mobile
                };
                ApiResult result = ApiAccountPasswordForget.Post(_context, account);

                ViewBag.Email = Email;
                ViewBag.Mobile = Mobile;
                if (result.Code == ApiResult.ResponseCode.Success)
                {
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, result.Message);
                    ViewBag.SendSms = true;
                }
                else
                {
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, result.Message);
                }
               
                ViewBag.changePass = result.Code.ToString();

                return PartialView(BaseController.GetView(this));
            }
            else
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_RECAPTCHA);
            return PartialView(BaseController.GetView(this));
        }
        //[HttpPost]
        public int LoginAndChangePass(string password, string mobile)
        {
            ApiResult result = new ApiResult();
            password = password.ToEnglishDigit();
            string hashPass = BaseSecurity.HashMd5(password);
            Account account = _context.Account.GetByMobile(mobile);
            if (hashPass != account.Password)
            {
                return -1;
            }
            else
            {
                result = ApiAccount.Post_DoLogin(_context, account, password, true);
                if (result.Code == ApiResult.ResponseCode.Success)
                    _context.Account.SetCurrentAccount((ViewAccount)result.Value, account.Password, true);
            }
            if (result.Code == ApiResult.ResponseCode.Success)
            {
                return 0;
            }
            return -2;
        }
        public ActionResult Register(string back = "/")
        {
            if (_context.Account.GetCurrentAccount() == null)
            {
                ViewBag.back = back;
                return PartialView(BaseController.GetView(this));
            }
            if (back == null)
                return RedirectToAction("profile", new { controller = "account" });
            return RedirectToAction("profile", new { controller = "account", back = back });
        }

        [HttpPost]
        public ActionResult Register(Account account, string ConfirmPassword = null, string Captcha = null, FormCollection form = null, string back = "/")
        {
            ViewBag.back = back;
            string gRecaptchaResponse = form["g-recaptcha-response"];
            if (Recaptcha.Check(gRecaptchaResponse) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_RECAPTCHA);
            else
            {
                if (System.Web.HttpContext.Current.Session["Captcha"] != null && string.IsNullOrEmpty(Captcha))
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_CAPTCHA);

                if (System.Web.HttpContext.Current.Session["Captcha"] != null && Captcha != System.Web.HttpContext.Current.Session["Captcha"].ToString())
                    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.CHECK_ACCOUNT_CAPTCHA);

                ApiResult result = ApiAccount.Post_DoRegister(_context, account, true, ConfirmPassword);
                if (result.Code == ApiResult.ResponseCode.Success)
                {
                    ViewAccount entity = new ViewAccount(account);
                    _context.Account.SetCurrentAccount(entity, account.Password, false);
                    if (string.IsNullOrEmpty(back))
                        return RedirectToAction("index", new { controller = "home" });
                    else
                        return Redirect(back);
                }
                else
                    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, result.Message);
            }
            return PartialView(BaseController.GetView(this), account);
        }
        [HttpPost]
        public new ActionResult Profile(Account updateAccount, string faBirthDay = null)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
            {
                return RedirectToAction("Index", new { controller = "Home" });
            }
            Account account = _context.Account.GetById(tempAccount.Id);
            Boolean flg = true;
            //Validatuon
            if (updateAccount.Mobile != null && !BaseSecurity.IsValidInput(updateAccount.Mobile, Enum_Validation.MOBILE))
            {
                flg = false;
            }
            if (updateAccount.Email != null && !BaseSecurity.IsValidInput(updateAccount.Email, Enum_Validation.EMAIL))
            {
                flg = false;
            }
            if (updateAccount.UserName != null)
            {
                string uName = updateAccount.UserName;
                var accountList = _context.Account.GetAll().Find(a => a.UserName == uName && a.ID != updateAccount.ID);
                if (accountList != null)
                {
                    //ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_USERNAME);
                    return RedirectToAction("EditProfile", new { controller = "account", back = "EditProfile" });
                    //return View("/Account/EditProfile", account);
                }
            }

            if (!flg)
            {
                ViewBag.Message = "اطلاعات با فرمت درست وارد نشده است";
                return PartialView(BaseController.GetView(this), account);
            }

            if (!string.IsNullOrEmpty(faBirthDay))
            {
                updateAccount.BirthDay = BaseDate.GetGregorian(new CustomDate(DateTime.Parse(faBirthDay))).GetValueOrDefault();
            }
            else
            {
                updateAccount.BirthDay = null;
            }

            if (!string.IsNullOrEmpty(updateAccount.NationalCode))
            {
                //pattern
                if (BaseSecurity.IsValidInput(updateAccount.NationalCode, Enum_Validation.NationalCode) == false)
                {

                    ViewBag.Message = "فرمت کد ملی صحیح نمی باشد";
                    return PartialView(BaseController.GetView(this), account);
                }
            }

            if (account.ID == updateAccount.ID)
            {
                if (string.IsNullOrEmpty(updateAccount.FullName))
                {
                    ViewBag.Message = "Enter The FullName,Please";
                    return PartialView(BaseController.GetView(this, "EditProfile"), account);
                }
                account = _context.Account.GetById(account.ID);
                account.Address = updateAccount.Address;
                account.Company = updateAccount.Company;
                account.Email = updateAccount.Email;
                account.FullName = updateAccount.FullName;
                account.IsMale = updateAccount.IsMale;
                account.Job = updateAccount.Job;
                account.Phone = updateAccount.Phone;
                account.NationalCode = updateAccount.NationalCode;
                account.BirthDay = updateAccount.BirthDay;
                account.IsMale = updateAccount.IsMale;
                account.Website = updateAccount.Website;
                account.Fax = updateAccount.Fax;
                account.Instagram = updateAccount.Instagram;
                account.Telegram = updateAccount.Telegram;
                account.WhatsApp = updateAccount.WhatsApp;
                account.CompanyNo = updateAccount.CompanyNo;
                account.Lat = updateAccount.Lat;
                account.Long = updateAccount.Long;
                account.Linkedin = updateAccount.Linkedin;
                account.Facebook = updateAccount.Facebook;
                account.Profession = updateAccount.Profession;
                account.AboutMe = updateAccount.AboutMe;
                account.FavoritLink = updateAccount.FavoritLink;
                account.FavoritTitle = updateAccount.FavoritTitle;

                _context.Account.Update(account);
                _context.Save();
                tempAccount = new ViewAccount(account);
                _context.Account.SetCurrentAccount(tempAccount, tempAccount.Password, false);
                return PartialView(BaseController.GetView(this), account);
            }
            else
                return RedirectToAction("login", new { controller = "account", back = "index" });
        }
        public new ActionResult Profile()
        {
            //var a = GetResxNameByValue("ADDRESS");

            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("Index", new { controller = "Home" });
            //return RedirectToAction("index", new { controller = "account", back = "profile" });

            Account account = _context.Account.GetById(tempAccount.Id);

            return PartialView(BaseController.GetView(this), account);
        }

        public ActionResult EditProfile()
        {

            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "EditProfile" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "EditProfile" });
                return RedirectToAction("index", new { controller = "home" });

            return PartialView(BaseController.GetView(this), account);
        }

        public ActionResult EditUserName(Account updateAccount)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            Account account = _context.Account.GetById(tempAccount.Id);
            Boolean flg = false;
            //Validatuon
            if ((updateAccount.UserName != null) && (updateAccount.UserName != ""))
            {
                string uName = updateAccount.UserName;
                var accountList = _context.Account.FirstOrDefault(a => a.UserName == uName && a.ID != updateAccount.ID);
                if (accountList != null)
                {
                    flg = true;
                }
            }
            if (flg == true)
            {
                ViewBag.Message = "This Username is in Use";
                return PartialView(BaseController.GetView(this), account);
            }
            else if (account.ID == updateAccount.ID && account.ChangeUserName == false)
            {
                account = _context.Account.GetById(account.ID);
                account.UserName = updateAccount.UserName;
                account.ChangeUserName = true;
                _context.Account.Update(account);
                _context.Save();
                tempAccount = new ViewAccount(account);
                _context.Account.SetCurrentAccount(tempAccount, tempAccount.Password, false);
                return View(BaseController.GetView(this), account);
            }
            else
                return RedirectToAction("login", new { controller = "account", back = "index" });
        }

        public ActionResult Address()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this), account);
        }
        public ActionResult Dashboard()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this), account);
        }
        public ActionResult UserComment()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this));
        }
        public ActionResult GiftCard()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this));
        }
        public ActionResult TestSina()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                //return RedirectToAction("login", new { controller = "account", back = "Address" });
                return RedirectToAction("index", new { controller = "home" });

            Account account = _context.Account.GetById(tempAccount.Id);
            return PartialView(BaseController.GetView(this), account);
        }

        //[Route("~/{username}")]
        public ActionResult Preview(string username)
        {

            ViewAccount tempAccount = _context.Account.GetAccountbyUserName(username);
            Account account = _context.Account.GetById(tempAccount.Id);
            if (account == null)
            {
                return RedirectToAction("index", new { controller = "home" });
            }
            else
            {
                return View(BaseController.GetView(this), account);
            }
        }

        [HttpGet]
        public ActionResult Captcha()
        {


            string CapImage = "data:image/png;base64," +
            Convert.ToBase64String(new Utility().VerificationTextGenerator());
            return Json(new { CapImage = CapImage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentAccount(string ViewName = null)
        {
            ViewAccount CurrentAccount = _context.Account.GetCurrentAccount();
            ViewBag.CurrentAccount = CurrentAccount;

            if (ViewName == null)
                return null;
            else
                return PartialView(BaseController.GetView(this, ViewName), CurrentAccount);
        }
        private bool IsModelValid(Account account)
        {
            bool result = false;
            if (account.FullName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);
            else if (account.Email == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_EMAIL);
            else if (BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_ACCOUNT_EMAIL);
            else if (account.Password == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_PASSWORD);
            else if (_context.Account.GetByEmail(account.Email) != null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_EMAIL);
            else if (account.UserName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_USERNAME);
            //else if (_context.Account.GetByUserName(account.UserName) != null)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_USERNAME);

            else
                result = true;

            return result;
        }

        public ActionResult vCardDownload(string username)
        {
            var ua = HttpContext.Request.UserAgent;
            var extention = ".vcf";
            if (ua.Contains("Android"))
                extention = ".txt";


            ViewAccount tempAccount = _context.Account.GetAccountbyUserName(username);
            Account account = _context.Account.GetById(tempAccount.Id);
            string path = "";
            if (account.Picture != null)
            {
                string x = account.Picture.Url.ToString();
                string m = x.Replace("SYSTEM_TYPE_PANEL/Uploads", "");
                m = "/Panel/Uploads" + m;
                path = Server.MapPath(@m);
            }
            var card = new VCard
            {
                FullName = account.FullName,

                Addrress = account.Address,

                JobTitle = account.Job,

                Phone = account.Phone,

                Mobile = account.Mobile,

                Email = account.Email,

                Fax = account.Fax,

                Instagram = account.Instagram,

                Telegram = account.Telegram,
                WhatsApp = account.WhatsApp,
                Website = account.Website,
                HomePage = "Vipbcard.com/" + account.UserName
            };
            if (path != "")
            {
                card.Image = System.IO.File.ReadAllBytes(path);
            }
            string fileName = "v_" + account.UserName + extention;
            string fpath = Server.MapPath("/Panel/Uploads/") + fileName;

            System.IO.File.Create(fpath).Dispose();
            using (var file = System.IO.File.OpenWrite(fpath))

            using (var writer = new System.IO.StreamWriter(file))
            {
                if (extention == ".vcf")
                    writer.Write(card.ToIos());
                else
                    writer.Write(card.ToAndroid());


            }
            // Image = account.PictureId,

            Response.Clear();

            Response.ContentType = "text/vcard";


            var cardString = "";
            if (extention == ".vcf")
                cardString = card.ToIos();
            else
                cardString = card.ToAndroid();

            var inputEncoding = Encoding.Default;

            var outputEncoding = Encoding.GetEncoding("windows-1257");

            var cardBytes = inputEncoding.GetBytes(cardString);

            var outputBytes = Encoding.Convert(inputEncoding, outputEncoding, cardBytes);

            Response.OutputStream.Write(outputBytes, 0, outputBytes.Length);

            Response.End();

            return File(System.IO.File.ReadAllBytes("/Panel/Uploads/" + fileName), "text /vcard");
        }
        private string GetResxNameByValue(string value)
        {
            System.Resources.ResourceManager rm = new global::System.Resources.ResourceManager("Resources.Lang", global::System.Reflection.Assembly.Load("App_GlobalResources"));


            var entry =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() == value);

            var key = entry.Key.ToString();
            return key;

        }
        //private string getInvitationEmail()
        //{
        //    StringBuilder html = new StringBuilder();
        //    try
        //    {
        //        html.Append("<b>دعوت به خوش کالا </b>" + "<br /><br />");
        //        html.Append("<b>خوشحال می شویم از سایت خوش کالا دیدن فرمایید </b>" + "<br /><br />");
        //        html.Append("https://khoshkala.com/" + "<br />");
        //        html.Append("<hr />");
        //    }
        //    catch (Exception) { }
        //    return html.ToString();
        //}
        //[HttpPost]
        //public ActionResult Invitation(string mestr)
        //{
        //    string emailBody = this.getInvitationEmail();
        //    UnitOfWork mainContext = new UnitOfWork();
        //    StringBuilder str_1 = new StringBuilder();
        //    string token_1 = mestr;
        //    string token_2 = "";
        //    string token_3 = "";
        //    if (mestr == null || mestr.Trim() == string.Empty)
        //    {
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, "خطا در ورود اطلاعات");
        //    }
        //    else if (mestr.Contains("@") == true)
        //    {
        //        mainContext.Email.SaveNewEmail(mestr.Trim(), Enum_EmailType.PRODUCT_NOTIFY, emailBody, "دعوت به خوش کالا");
        //    }
        //    else if (mestr.Trim().Substring(0, 2) == "09")
        //    {
        //        mainContext.Sms.SaveNewSms(mestr.Trim(), Enum_SmsType.PRODUCT_NOTIFY, str_1.ToString(), token_1, token_2, token_3);
        //    }
        //    else
        //    {
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, "فرمت اطلاعات وارد شده صحیح نیست");
        //    }


        //    return PartialView(BaseController.GetView(this));
        //}
        public ActionResult Order()
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            return PartialView(BaseController.GetView(this), tempAccount);
        }
        public ActionResult SearchAccountOrder(
                Enum_Code aoStatus = Enum_Code.ORDER_STATUS_NONE,
                string aoViewName = null)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            //poIndex = poIndex != null ? poIndex.Value : 1;
            //poSize = poSize != null ? poSize.Value : 10;
            aoViewName = aoViewName == null ? "Search" : aoViewName;
            List<AccountOrder> results = _context.AccountOrder.Search(
                accountId: account.ID,
                status: aoStatus
                );

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, aoViewName, results);
        }
        public ActionResult SearchProductVisit(
        string pvViewName = null,
        int pvSize = 10)
        {
            ViewAccount tempAccount = _context.Account.GetCurrentAccount();
            if (tempAccount == null)
                return RedirectToAction("index", new { controller = "home" });
            Account account = _context.Account.GetById(tempAccount.Id);
            if (account == null)
                return RedirectToAction("index", new { controller = "home" });
            //poIndex = poIndex != null ? poIndex.Value : 1;
            //poSize = poSize != null ? poSize.Value : 10;
            pvViewName = pvViewName == null ? "Search" : pvViewName;
            List<ProductVisit> results = _context.ProductVisit.GetAllByAccountId(accountId: account.ID, prpageSize: pvSize);

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, pvViewName, results);
        }
        public ActionResult History(int prindex = 1, int prpageSize = 5)
        {
            if (_context.Account.GetCurrentAccount() == null)
                return RedirectToAction("Index", "Home", new { controller = "account", back = "/History" });

            ViewAccount account = _context.Account.GetCurrentAccount();
            List<ProductVisit> results = _context.ProductVisit.GetAllByAccountId(accountId: account.Id, prpageSize: prpageSize, prindex: prindex);


            //var searchQuery = _context.ProductLike.GetSearchQuery(account.Id);
            //List<ProductLike> results = _context.ProductLike.Search(prindex, prpageSize, searchQuery);
            ViewProductVisit search = new ViewProductVisit()
            {
                TotalCount = _context.ProductVisit.SearchDetail(account.Id).TotalCount,
                PageIndex = prindex,
                PageSize = prpageSize,
                Results = results
            };
            return View(BaseController.GetView(this), search);
        }
        [HttpGet]
        public ActionResult Favorite(int prindex = 1,
    int prpageSize = 5)
        {
            if (_context.Account.GetCurrentAccount() == null)

                return RedirectToAction("Index", "Home", new { controller = "account", back = "/favorite" });

            ViewAccount account = _context.Account.GetCurrentAccount();

            var searchQuery = _context.ProductLike.GetSearchQuery(account.Id);
            List<ProductLike> results = _context.ProductLike.Search(prindex, prpageSize, searchQuery);
            ViewProductLikeFavorit search = new ViewProductLikeFavorit()
            {
                TotalCount = _context.ProductLike.SearchDetail(query: searchQuery).Count,
                PageIndex = prindex,
                PageSize = prpageSize,
                Results = results
            };
            return View(BaseController.GetView(this), search);
        }
        public ActionResult FavoriteAd(int prindex = 1,
   int prpageSize = 5)
        {
            if (_context.Account.GetCurrentAccount() == null)

                return RedirectToAction("Index", "Home", new { controller = "account", back = "/FavoriteAd" });

            ViewAccount account = _context.Account.GetCurrentAccount();

            var searchQuery = _context.ProductLike.GetSearchQuery(account.Id, IsAdvertising: true);
            List<ProductLike> results = _context.ProductLike.Search(prindex, prpageSize, searchQuery);
            ViewProductLikeFavorit search = new ViewProductLikeFavorit()
            {
                TotalCount = _context.ProductLike.SearchDetail(query: searchQuery).Count,
                PageIndex = prindex,
                PageSize = prpageSize,
                Results = results
            };
            return View(BaseController.GetView(this), search);
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