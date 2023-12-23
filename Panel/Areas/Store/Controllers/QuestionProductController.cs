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
    public class QuestionProductController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct);
            List<QuestionProduct> listQuestionProduct = _context.QuestionProduct.GetAllProductId(id.GetValueOrDefault());
            ViewBag.Product = _context.Product.GetById(id);
            return View(listQuestionProduct);
        }

       

        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);
            ViewBag.Product = _context.Product.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(QuestionProduct QuestionProduct)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_New);

            List<QuestionProductLanguage> listLanguage = QuestionProduct.QuestionProductLanguage.ToList();
            QuestionProduct.QuestionProductLanguage.Clear();

            ViewMessage result = IsModelValid(QuestionProduct);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionProduct.Insert(QuestionProduct);
                _context.Save();

                foreach (QuestionProductLanguage item in listLanguage)
                {
                    item.QuestionId = QuestionProduct.ID;
                    _context.QuestionProductLanguage.Insert(item);
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

            QuestionProduct QuestionProduct = _context.QuestionProduct.GetById(id);
            if (QuestionProduct == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(QuestionProduct);
        }

        [HttpPost]
        public ActionResult Edit(QuestionProduct QuestionProduct)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Edit);
            List<QuestionProductLanguage> listLanguage = QuestionProduct.QuestionProductLanguage.ToList();
            QuestionProduct.QuestionProductLanguage.Clear();
            ViewMessage result = IsModelValid(QuestionProduct);
            if (IsModelValid(QuestionProduct).Type == Enum_MessageType.SUCCESS)
            {
                _context.QuestionProduct.Update(QuestionProduct);
                _context.Save();

                _context.QuestionProductLanguage.DeleteByQuestionId(QuestionProduct.ID);
                _context.Save();
                foreach (QuestionProductLanguage item in listLanguage)
                {
                    item.QuestionId = QuestionProduct.ID;
                    _context.QuestionProductLanguage.Insert(item);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(QuestionProduct QuestionProduct)
        {
            ViewMessage result = new ViewMessage();
            if (QuestionProduct.Question == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            QuestionProduct QuestionProduct = _context.QuestionProduct.GetById(id);
            if (QuestionProduct == null)
                return HttpNotFound();

            return View(QuestionProduct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.QuestionProduct_Delete);
            QuestionProduct QuestionProduct = _context.QuestionProduct.GetById(id);
            try
            {
                int? productId = QuestionProduct.ProductId;
                _context.QuestionProduct.Delete(QuestionProduct);
                _context.Save();
                return RedirectToAction("index", new { id = productId });
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(QuestionProduct);
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