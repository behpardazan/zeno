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

namespace Panel.Areas.Store.Controllers
{
    public class QuestionProductTypeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct);
            List<QuestionProductType> listQuestionProductType = _context.QuestionProductType.GetAllProductTypeId(id.GetValueOrDefault());
            ViewBag.ProductType = _context.ProductType.GetById(id);
            return View(listQuestionProductType);
        }

       

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);
            ViewBag.ProductType = _context.ProductType.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(QuestionProductType QuestionProductType)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);

            List<QuestionProductTypeLanguage> listLanguage = QuestionProductType.QuestionProductTypeLanguage.ToList();
            QuestionProductType.QuestionProductTypeLanguage.Clear();

            ViewMessage result = IsModelValid(QuestionProductType);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionProductType.Insert(QuestionProductType);
                _context.Save();

                foreach (QuestionProductTypeLanguage item in listLanguage)
                {
                    item.QuestionId = QuestionProductType.ID;
                    _context.QuestionProductTypeLanguage.Insert(item);
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

            QuestionProductType QuestionProductType = _context.QuestionProductType.GetById(id);
            if (QuestionProductType == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(QuestionProductType);
        }

        [HttpPost]
        public ActionResult Edit(QuestionProductType QuestionProductType)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Edit);
            List<QuestionProductTypeLanguage> listLanguage = QuestionProductType.QuestionProductTypeLanguage.ToList();
            QuestionProductType.QuestionProductTypeLanguage.Clear();
            ViewMessage result = IsModelValid(QuestionProductType);
            if (IsModelValid(QuestionProductType).Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionProductType.Update(QuestionProductType);
                _context.Save();

                _context.QuestionProductTypeLanguage.DeleteByQuestionId(QuestionProductType.ID);
                _context.Save();
                foreach (QuestionProductTypeLanguage item in listLanguage)
                {
                    item.QuestionId = QuestionProductType.ID;
                    _context.QuestionProductTypeLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(QuestionProductType QuestionProductType)
        {
            ViewMessage result = new ViewMessage();
            if (QuestionProductType.Question == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionProductType QuestionProductType = _context.QuestionProductType.GetById(id);
            if (QuestionProductType == null)
                return HttpNotFound();

            return View(QuestionProductType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            QuestionProductType QuestionProductType = _context.QuestionProductType.GetById(id);
            try
            {
                int? ProductTypeId = QuestionProductType.ProductTypeId;
                _context.QuestionProductType.Delete(QuestionProductType);
                _context.Save();
                return RedirectToAction("index", new { id = ProductTypeId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(QuestionProductType);
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