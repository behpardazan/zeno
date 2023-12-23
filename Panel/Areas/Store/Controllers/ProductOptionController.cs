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
    public class ProductOptionController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? productTypeId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption);

            ViewBag.ProductTypeId = productTypeId;           

            List<ProductOption> list = _context.ProductOption.Search(typeId: productTypeId, pageSize: 2000);
            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Details);
            ProductOption field = _context.ProductOption.GetById(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        public ActionResult Create(int? productypeId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_New);

            FillDropDowns(productypeId);
            ViewBag.Message = BaseMessage.GetMessage();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductOption productOption)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_New);            
            ViewMessage result = IsModelValid(productOption);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.ProductOption.Insert(productOption);
                _context.Save();
            }
            else
            {
                FillDropDowns(productOption.ProductTypeId);

            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductOption productOption = _context.ProductOption.GetById(id);
            if (productOption == null)
                return HttpNotFound();

            FillDropDowns(productOption.ProductTypeId);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(productOption);
        }

        [HttpPost]
        public ActionResult Edit(ProductOption productOption)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Edit);
            
            ViewMessage result = IsModelValid(productOption);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.ProductOption.Update(productOption);
                _context.Save();
            }
            return RedirectToAction("Index");
        }

        private ViewMessage IsModelValid(ProductOption field)
        {
            ViewMessage result = new ViewMessage();
            if (field.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_OPTION_FIELD_NAME);
            return result;
        }

        private void FillDropDowns(int? productTypeId)
        {
            ViewBag.ProductTypeId = new SelectList(_context.ProductType.GetAll(), "ID", "Name", productTypeId);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductOption productOption = _context.ProductOption.GetById(id);
            if (productOption == null)
                return HttpNotFound();

            return View(productOption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Delete);
            ProductOption item = _context.ProductOption.GetById(id);
            try
            {
                _context.ProductOption.Delete(item);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(item);
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