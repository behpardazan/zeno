using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.ViewModels;

namespace Panel.Areas.Store.Controllers
{
    public class QuestionSmartOfferController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer);
            var count = _context.SmartOffer.GetAll().Count();
            ViewBag.VisitCount = count;
            return View(_context.QuestionSmartOffer.GetAll());
        }


        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_New);
            ViewMessage result = new ViewMessage();
            return View(new QuestionSmartOffer());
        }

        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(QuestionSmartOffer question)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_New);
            List<QuestionSmartOfferLanguage> listLanguage = question.QuestionSmartOfferLanguage.ToList();

            ViewMessage result = IsModelValid(question);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionSmartOffer.Insert(question);
                _context.Save();
                foreach (QuestionSmartOfferLanguage item in listLanguage)
                {
                    item.QuestionId = question.ID;
                    _context.QuestionSmartOfferLanguage.Insert(item);
                    _context.Save();
                }
            }
            else
            {
                return new JsonResult()
                {
                    Data = result.Body,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionSmartOffer question = _context.QuestionSmartOffer.GetById(id);
            if (question == null)
                return HttpNotFound();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(question);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(QuestionSmartOffer question)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_Edit);
            List<QuestionSmartOfferLanguage> listLanguage = question.QuestionSmartOfferLanguage.ToList();
            question.QuestionSmartOfferLanguage.Clear();

            ViewMessage result = IsModelValid(question);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionSmartOffer.Update(question);
                _context.Save();
                foreach (QuestionSmartOfferLanguage item in listLanguage)
                {
                    item.QuestionId = question.ID;
                    _context.QuestionSmartOfferLanguage.Insert(item);
                }
                _context.Save();
            }
            else
            {
                return new JsonResult()
                {
                    Data = result.Body,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult() { Data = result };
            
        }

        private ViewMessage IsModelValid(QuestionSmartOffer question)
        {
            ViewMessage result = new ViewMessage();
            if (question.Text == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_DATA);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionSmartOffer question = _context.QuestionSmartOffer.GetById(id);
            if (question == null)
                return HttpNotFound();

            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_Delete);
            QuestionSmartOffer question = _context.QuestionSmartOffer.GetById(id);
            try
            {
                _context.QuestionSmartOffer.Delete(question);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(question);
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