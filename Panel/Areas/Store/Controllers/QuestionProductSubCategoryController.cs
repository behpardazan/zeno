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
    public class QuestionProductSubCategoryController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct);
            List<QuestionProductSubCategory> listQuestionProductSubCategory = _context.QuestionProductSubCategory.GetAllSubCategoryId(id.GetValueOrDefault());
            ViewBag.ProductSubCategory = _context.ProductSubCategory.GetById(id);
            return View(listQuestionProductSubCategory);
        }

       

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);
            ViewBag.ProductSubCategory = _context.ProductSubCategory.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(QuestionProductSubCategory QuestionProductSubCategory)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);


            ViewMessage result = IsModelValid(QuestionProductSubCategory);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionProductSubCategory.Insert(QuestionProductSubCategory);
                _context.Save();

                
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionProductSubCategory QuestionProductSubCategory = _context.QuestionProductSubCategory.GetById(id);
            if (QuestionProductSubCategory == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(QuestionProductSubCategory);
        }

        [HttpPost]
        public ActionResult Edit(QuestionProductSubCategory QuestionProductSubCategory)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Edit);
            ViewMessage result = IsModelValid(QuestionProductSubCategory);
            if (IsModelValid(QuestionProductSubCategory).Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionProductSubCategory.Update(QuestionProductSubCategory);
                _context.Save();

               
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(QuestionProductSubCategory QuestionProductSubCategory)
        {
            ViewMessage result = new ViewMessage();
            if (QuestionProductSubCategory.Question == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionProductSubCategory QuestionProductSubCategory = _context.QuestionProductSubCategory.GetById(id);
            if (QuestionProductSubCategory == null)
                return HttpNotFound();

            return View(QuestionProductSubCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            QuestionProductSubCategory QuestionProductSubCategory = _context.QuestionProductSubCategory.GetById(id);
            try
            {
                int? productId = QuestionProductSubCategory.SubCategoryId;
                _context.QuestionProductSubCategory.Delete(QuestionProductSubCategory);
                _context.Save();
                return RedirectToAction("index", new { id = productId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(QuestionProductSubCategory);
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