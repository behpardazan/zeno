using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Content.Controllers
{
    public class QuestionController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Question);
            return View(_context.Question.GetAll().ToList().OrderByDescending(s=>s.ID));
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Question);

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Question question = _context.Question.GetById(id);
            if (question == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Question);
            _context.Question.Update(question);
            _context.Save();
            ViewBag.Message = BaseMessage.GetMessage();
            //return View(contactus);
            return RedirectToAction("Index");
        }
        //private bool IsModelValid(Gallery gallery)
        //{
        //    bool result = false;
        //    if (gallery.Name == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_GALLERY_NAME);
        //    else
        //        result = true;
        //    return result;
        //}
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