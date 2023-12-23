using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class NewsletterController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup);
            return View(_context.NewsLetter.GetAll());
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_New);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Newsletter newsletter)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_New);

            newsletter.Datetime = DateTime.Now;
            newsletter.WebsiteId = null;
            newsletter.Active = true;
            _context.NewsLetter.Insert(newsletter);
            _context.Save();
            return RedirectToAction("Index");

        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Newsletter newsLetter = _context.NewsLetter.GetById(id);
            if (newsLetter == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(newsLetter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Newsletter newsLetter)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Edit);

            _context.NewsLetter.Update(newsLetter);
            _context.Save();
            return RedirectToAction("Index");

        }

        public ActionResult Sms()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Sms);
            ViewBag.Message = BaseMessage.GetMessage();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sms(string Body)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Sms);

            if (Body == null)
            {
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_SMS_BODY);
                return View(model: Body);
            }
            else
            {
                var items = _context.NewsLetter.ReadyToSend(type: false);
                foreach (Newsletter item in items)
                {

                    _context.Sms.SaveNewSms(item.Mobile, Enum_SmsType.NEWSLETTER, Body);
                    _context.Save();
                }

                ViewBag.Body = Body;
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_NEWSLETTER_SENT);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Email()
        {
            var listUser = _context.Account.Where(s => s.Email != null && s.Email != "").ToList();
            var listNewLater = _context.NewsLetter.Where(s => s.Email != null && s.Email != "").ToList();
            foreach(var item in listNewLater)
            {
                if (listUser.Where(s => s.Email == item.Email).FirstOrDefault()==null){
                    listUser.Add(new Account { Email = item.Email });
                }
            }
            ViewBag.Users = new SelectList(listUser, "Email", "Email");
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Sms);
            ViewBag.Message = BaseMessage.GetMessage();
            DataLayer.Entities.Email email = new DataLayer.Entities.Email();
            return View(email);
        }

        [HttpPost]
        //[AllowHtml(true)]
        public ActionResult Email(ViewEmail email)
        {
            DataLayer.Entities.Email model = new DataLayer.Entities.Email();
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Sms);
          
            if (email.Body == null)
            {
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_EMAIL_BODY);
                return View(model: model);
            }
            else
            {
                for(int i=0;i< email.UserEmails.Length; i++)
                {
                    _context.Email.SaveNewEmail(email.UserEmails[i], Enum_EmailType.NEWSLETTER, email.Body, Resource.Lang.Newsletters);
                    _context.Save();
                }
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.SUCCESS, Enum_Message.SUCCESSFULL_NEWSLETTER_SENT);
                return RedirectToAction("Index");
            }


        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Newsletter group = _context.NewsLetter.GetById(id);
            if (group == null)
                return HttpNotFound();

            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.NewsletterGroup_Delete);
            Newsletter group = _context.NewsLetter.GetById(id);
            try
            {
                _context.NewsLetter.Delete(group);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(group);
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