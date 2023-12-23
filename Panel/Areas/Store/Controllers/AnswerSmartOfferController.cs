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

namespace Panel.Areas.Store.Controllers
{
    public class AnswerSmartOfferController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? questionId = null)
        {
            
            if (questionId == null )
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.QuestionId = questionId;
          
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer);

            List<AnswerSmartOffer> list = _context.AnswerSmartOffer.Search(questionId: questionId);
            return View(list.ToView());
        }


        public ActionResult Create(int? questionId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_New);

            ViewBag.QuestionId = questionId;
          

            AnswerSmartOffer answerSmartOffer = new AnswerSmartOffer() {
                QuestionId = questionId.Value
            };

            ViewBag.Message = BaseMessage.GetMessage();

            return View(answerSmartOffer);
        }

        [HttpPost]
        public ActionResult Create(AnswerSmartOffer answerSmartOffer)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_New);
            List<AnswerSmartOfferLanguage> listLanguage = answerSmartOffer.AnswerSmartOfferLanguage.ToList();
            ViewMessage result = IsModelValid(answerSmartOffer);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.AnswerSmartOffer.Insert(answerSmartOffer);
                _context.Save();
                foreach (AnswerSmartOfferLanguage item in listLanguage)
                {
                    item.AnswerId = answerSmartOffer.ID;
                    _context.AnswerSmartOfferLanguage.Insert(item);
                    _context.Save();
                }

            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AnswerSmartOffer answerSmartOffer = _context.AnswerSmartOffer.GetById(id);
            if (answerSmartOffer == null)
                return HttpNotFound();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(answerSmartOffer);
        }

        [HttpPost]
        public ActionResult Edit(AnswerSmartOffer answerSmartOffer)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_Edit);
            List<AnswerSmartOfferLanguage> listLanguage = answerSmartOffer.AnswerSmartOfferLanguage.ToList();
            answerSmartOffer.AnswerSmartOfferLanguage.Clear();


            ViewMessage result = IsModelValid(answerSmartOffer);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.AnswerSmartOffer.Update(answerSmartOffer);
                _context.Save();
                foreach (AnswerSmartOfferLanguage item in listLanguage)
                {
                    item.AnswerId = answerSmartOffer.ID;
                    _context.AnswerSmartOfferLanguage.Insert(item);
                }
                _context.Save();
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(AnswerSmartOffer answerSmartOffer)
        {
            ViewMessage result = new ViewMessage();
            if (answerSmartOffer.Text == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_DATA);
            return result;
        }

     

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AnswerSmartOffer answerSmartOffer = _context.AnswerSmartOffer.GetById(id);
            if (answerSmartOffer == null)
                return HttpNotFound();

            return View(answerSmartOffer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionSmartOffer_Delete);
            AnswerSmartOffer answerSmartOffer = _context.AnswerSmartOffer.GetById(ID);
            try
            {
                int? questionId = answerSmartOffer.QuestionId;
            
                _context.AnswerSmartOffer.Delete(answerSmartOffer);
                _context.Save();

                return RedirectToAction("Index", new
                {
                    @questionId = questionId
                });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(answerSmartOffer);
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