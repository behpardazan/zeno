﻿using DataLayer.Entities;
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
    public class ProductOptionItemController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int productOptionId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption);
            var productOption = _context.ProductOption.GetById(productOptionId);
            if(productOption==null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.productTypeId = productOption.ProductTypeId;
            ViewBag.productOptionId = productOptionId;

            List<ProductOptionItem> list = _context.ProductOptionItem.Search(productOptionId: productOptionId, pageSize: 2000);
            return View(list);
        }

        public ActionResult Create(int productOptionId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_New);
            ViewBag.Message = BaseMessage.GetMessage();

            return View(new ProductOptionItem() {OptionId=productOptionId });
        }

        [HttpPost]
        public ActionResult Create(ProductOptionItem productOptionItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_New);            
            ViewMessage result = IsModelValid(productOptionItem);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                _context.ProductOptionItem.Insert(productOptionItem);
                _context.Save();
            }
            return RedirectToAction("Index",new { productOptionId = productOptionItem.OptionId});
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductOptionItem productOptionItem = _context.ProductOptionItem.GetById(id);
            if (productOptionItem == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(productOptionItem);
        }

        [HttpPost]
        public ActionResult Edit(ProductOptionItem productOptionItem)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Edit);
            
            ViewMessage result = IsModelValid(productOptionItem);
            ProductOptionItem productOptionItemTemp = _context.ProductOptionItem.GetById(productOptionItem.ID);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                productOptionItemTemp.Value = productOptionItem.Value;
                _context.ProductOptionItem.Update(productOptionItemTemp);
                _context.Save();
            }
            return RedirectToAction("Index", new { productOptionId = productOptionItemTemp.OptionId });
        }

        private ViewMessage IsModelValid(ProductOptionItem field)
        {
            ViewMessage result = new ViewMessage();
            if (field.Value == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_OPTION_ITEM_FIELD_NAME);
            return result;
        }


        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductOptionItem productOption = _context.ProductOptionItem.GetById(id);
            if (productOption == null)
                return HttpNotFound();

            return View(productOption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ProductOption_Delete);
            ProductOptionItem field = _context.ProductOptionItem.GetById(id);
            try
            {
                _context.ProductOptionItem.Delete(field);
                _context.Save();
                return RedirectToAction("Index", new { productOptionId = field.OptionId });
            }

            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(field);
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