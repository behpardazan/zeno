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


namespace Panel.Areas.Content.Controllers
{
    public class QuestionPostController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct);
            List<QuestionPost> listQuestionPost = _context.QuestionPost.GetAllPostId(id.GetValueOrDefault());
            ViewBag.Post = _context.Post.GetById(id);
            return View(listQuestionPost);
        }

       

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);
            ViewBag.Post = _context.Post.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(QuestionPost QuestionPost)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);

            List<QuestionPostLanguage> listLanguage = QuestionPost.QuestionPostLanguage.ToList();
            QuestionPost.QuestionPostLanguage.Clear();

            ViewMessage result = IsModelValid(QuestionPost);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionPost.Insert(QuestionPost);
                _context.Save();

                foreach (QuestionPostLanguage item in listLanguage)
                {
                    item.QuestionId = QuestionPost.ID;
                    _context.QuestionPostLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionPost QuestionPost = _context.QuestionPost.GetById(id);
            if (QuestionPost == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(QuestionPost);
        }

        [HttpPost]
        public ActionResult Edit(QuestionPost QuestionPost)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Edit);
            List<QuestionPostLanguage> listLanguage = QuestionPost.QuestionPostLanguage.ToList();
            QuestionPost.QuestionPostLanguage.Clear();
            ViewMessage result = IsModelValid(QuestionPost);
            if (IsModelValid(QuestionPost).Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionPost.Update(QuestionPost);
                _context.Save();

                _context.QuestionPostLanguage.DeleteByQuestionId(QuestionPost.ID);
                _context.Save();
                foreach (QuestionPostLanguage item in listLanguage)
                {
                    item.QuestionId = QuestionPost.ID;
                    _context.QuestionPostLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(QuestionPost QuestionPost)
        {
            ViewMessage result = new ViewMessage();
            if (QuestionPost.Question == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionPost QuestionPost = _context.QuestionPost.GetById(id);
            if (QuestionPost == null)
                return HttpNotFound();

            return View(QuestionPost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            QuestionPost QuestionPost = _context.QuestionPost.GetById(id);
            try
            {
                int? PostId = QuestionPost.PostId;
                _context.QuestionPost.Delete(QuestionPost);
                _context.Save();
                return RedirectToAction("index", new { id = PostId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(QuestionPost);
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