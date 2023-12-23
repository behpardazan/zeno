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
    public class QuestionProductCategoryController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct);
            List<QuestionProductCategory> listQuestionProductCategory = _context.QuestionProductCategory.GetAllCategoryId(id.GetValueOrDefault());
            ViewBag.ProductCategory = _context.ProductCategory.GetById(id);
            return View(listQuestionProductCategory);
        }

       

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);
            ViewBag.ProductCategory = _context.ProductCategory.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(QuestionProductCategory QuestionProductCategory)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);


            ViewMessage result = IsModelValid(QuestionProductCategory);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionProductCategory.Insert(QuestionProductCategory);
                _context.Save();

                
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionProductCategory QuestionProductCategory = _context.QuestionProductCategory.GetById(id);
            if (QuestionProductCategory == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(QuestionProductCategory);
        }

        [HttpPost]
        public ActionResult Edit(QuestionProductCategory QuestionProductCategory)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Edit);
            ViewMessage result = IsModelValid(QuestionProductCategory);
            if (IsModelValid(QuestionProductCategory).Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionProductCategory.Update(QuestionProductCategory);
                _context.Save();

               
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(QuestionProductCategory QuestionProductCategory)
        {
            ViewMessage result = new ViewMessage();
            if (QuestionProductCategory.Question == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionProductCategory QuestionProductCategory = _context.QuestionProductCategory.GetById(id);
            if (QuestionProductCategory == null)
                return HttpNotFound();

            return View(QuestionProductCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            QuestionProductCategory QuestionProductCategory = _context.QuestionProductCategory.GetById(id);
            try
            {
                int? productId = QuestionProductCategory.CategoryId;
                _context.QuestionProductCategory.Delete(QuestionProductCategory);
                _context.Save();
                return RedirectToAction("index", new { id = productId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(QuestionProductCategory);
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