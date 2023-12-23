using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;
using DataLayer.Api;
using System.Data;
using DataLayer.Helpers.Common;

namespace Panel.Areas.Crm.Controllers
{
    public class AccountController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string fullName = null, string fromDate = null,
            string toDate = null, string pageIndex = null,
           string pageSize = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account);
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            DateTime from = string.IsNullOrEmpty(fromDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(fromDate))).GetValueOrDefault();
            DateTime to = string.IsNullOrEmpty(toDate) == true ? default(DateTime) : BaseDate.GetGregorian(new CustomDate(DateTime.Parse(toDate))).GetValueOrDefault();
            ViewBag.FromDatetime = fromDate;
            ViewBag.ToDatetime = toDate;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.fullName = fullName;
            ViewBag.TotalCount = _context.Account.SearchCount(
                   fullName: fullName,
                    fromDatetime: from,
                    toDatetime: to);

            List<Account> listOrder = _context.Account.Search(
                    fullName: fullName,
                     fromDatetime: from,
                    toDatetime: to,
                    pageSize: size,
                    index: index
                  )
                .ToList();

            return View(listOrder.ToView());
            //return View(_context.Account.Where(s=>s.Deleted==false).ToList().ToView());
        }
        public ActionResult UploadFile(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.AccountOrder_Details);
            var account = _context.Account.Where(s=>s.ID==id).FirstOrDefault();
            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account);
        }
        [HttpPost]
        public ActionResult UpdateFileNew(int accountId, List<ViewFile> FileIds, string description)
        {
            var account = _context.Account.Where(s => s.ID == accountId).FirstOrDefault();
            ViewMessage result = new ViewMessage();
            if (account != null)
            {

                List<DataLayer.Entities.AccountReport> accountFiles = new List<DataLayer.Entities.AccountReport>();
                if (FileIds != null)
                {
                    foreach (var item in FileIds)
                    {
                        accountFiles.Add(new DataLayer.Entities.AccountReport
                        {
                            FileId = item.FileId,
                            AccountId = accountId,
                        });
                    }
                }

                var files = _context.AccountReport.Where(s => s.AccountId == accountId).ToList();
                foreach (var item in files)
                {
                    _context.AccountReport.Delete(item);
                    var file = _context.File.FirstOrDefault(s => s.FileId == item.FileId);
                    if (file != null)
                    {
                        _context.File.Delete(file);
                    }
                }
                foreach (var itemfile in accountFiles)
                {
                    _context.AccountReport.Insert(itemfile);
                }
                _context.Save();
                result.Body = "ثبت با موفقیت انجام شد";
                result.Type = Enum_MessageType.SUCCESS;
                return new JsonResult()
                {
                    Data = result,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            else
            {
                result.Body = "خطا در انجام عملیات";
                result.Type = Enum_MessageType.ERROR;
                return new JsonResult()
                {
                    Data = result,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [HttpGet]
        public ActionResult GetFile(int accountId)
        {
            var fileAccount = _context.AccountReport.Where(s => s.AccountId == accountId).ToList();

            if (fileAccount != null)
            {
                List<ViewFile> files = new List<ViewFile>();
                files = fileAccount.Select(x=>x.File).ToList().ToApi();
                return new JsonResult()
                {
                    Data = files,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            }
            else
            {

                return new JsonResult()
                {
                    Data = "Error",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }


        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Details);
            Account account = _context.Account.GetById(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_New);
            account.Password = BaseSecurity.HashMd5(account.Password);
            account.Mobile = account.Mobile.GetEnglish();
            if (IsModelValid(account))
            {
                _context.Account.Insert(account);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(account);
            return View(account);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Account account = _context.Account.GetById(id);
            if (account == null)
                return HttpNotFound();

            FillDropDowns(account);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (!string.IsNullOrEmpty(account.UserName))
            {
                account.UserName = account.UserName.Trim().ToLower();
            }
            else
            {
                account.UserName = "";
            }

            if (IsModelValid(account, true))
            {

                var temp = _context.Account.GetById(account.ID);

                if (!string.IsNullOrEmpty(account.UserName))
                {
                    account.UserName = account.UserName.Trim().Replace(" ", "");
                    if (_context.Account.UserNameNotDuplicate(temp, account.UserName))
                    {
                        temp.UserName = account.UserName;
                    }
                    else
                    {
                        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.DUPLICATED_ACCOUNT_USERNAME);
                        FillDropDowns(account);
                        return View(account);
                    }
                }
                temp.TypeId = account.TypeId;
                temp.FullName = account.FullName;
                temp.ChangeUserName = account.ChangeUserName;
                temp.UpdateDatetime = DateTime.Now;
                temp.NationalCode = account.NationalCode;
                temp.Mobile = account.Mobile.GetEnglish();
                temp.Email = account.Email;
                temp.Phone = account.Phone;
                temp.Address = account.Address;
                temp.Job = account.Job;
                temp.IsMale = account.IsMale;
                temp.Description = account.Description;


                _context.Account.Update(temp);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(account);
            return View(account);
        }

        public ActionResult Password(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Account account = _context.Account.GetById(id);
            if (account == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(account);
        }

        [HttpPost]
        public ActionResult Password(int? id, string newPassword, string confirmPassword)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Account account = _context.Account.GetById(id);
            if (account == null)
                return HttpNotFound();

            bool result = false;
            if (newPassword == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD);
            else if (confirmPassword == "")
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PASSWORD_CONFIRM);
            else if (newPassword != confirmPassword)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_PASSWORD_CONFIRM);
            else
                result = true;

            if (result == true)
            {
                account.Password = BaseSecurity.HashMd5(newPassword);
                account.UpdateDatetime = DateTime.Now;
                _context.Account.Update(account);
                _context.Save();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        private bool IsModelValid(Account account, bool edit = false)
        {
            bool result = false;
            if (account.FullName == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_FULLNAME);
            else if (account.UserName != null && BaseSecurity.IsValidInput(account.UserName, Enum_Validation.USERNAME) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_USERNAME);

            //else if (account.Email == null)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_EMAIL);
            //else if (BaseSecurity.IsValidInput(account.Email, Enum_Validation.EMAIL) == false)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_ACCOUNT_EMAIL);
            //else if ((_context.Account.CheckEmailAndMobile(account, false)) == false)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_EMAIL);
            else if (account.Mobile == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_MOBILE);
            else if (BaseSecurity.IsValidInput(account.Mobile, Enum_Validation.MOBILE) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.INVALID_ACCOUNT_MOBILE);
            else if ((_context.Account.CheckEmailAndMobile(account, true)) == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.DUPLICATED_ACCOUNT_MOBILE);

            else if (account.Password == null && edit == false)
                ViewBag.Message = BaseMessage.GetMessage(Enum_Message.REQUIRED_ACCOUNT_PASSWORD);

            else
                result = true;
            return result;
        }

        private void FillDropDowns(Account account)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.ACCOUNT_TYPE), "ID", "Name", account != null ? account.TypeId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Account account = _context.Account.GetById(id);
            if (account == null)
                return HttpNotFound();

            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Account_Delete);
            Account account = _context.Account.GetById(id);
            try
            {
                _context.AccountLog.DeleteByAccountId(account.ID);
                _context.Account.Delete(account);
                _context.Save();
            }
            catch
            {
                account.Deleted = true;
                _context.Account.Update(account);
                _context.Save();
            }

            return RedirectToAction("Index");
        }
        public ActionResult ExportExcel()
        {
            int size = 100000000;
            int index = 1;
            string status_inserted = Enum_Code.ORDER_STATUS_INSERTED.ToString();
            string status_canceled = Enum_Code.ORDER_STATUS_CANCEL.ToString();


            List<AccountOrderProduct> list = _context.AccountOrderProduct.Where(s=>s.ProductId==5988 && s.AccountOrder.StatusId!= 67 && s.AccountOrder.StatusId != 2088 && s.AccountOrder.StatusId != 2094)
                .ToList();



            DataTable table = new DataTable();

            table.Columns.Add("FullName");
            table.Columns.Add("Mobile");

            foreach (var item in list)
            {
                table.Rows.Add(
                    item.AccountOrder.Account.FullName,
                    item.AccountOrder.Account.Mobile);

            }

            BaseExcel.ExportExcel(table);
            return View(list);
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