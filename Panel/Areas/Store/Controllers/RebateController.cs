﻿using DataLayer.Base;
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
    [ValidateInput(false)]
    public class RebateController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? index = null,
            int? pagesize = null,
            string name = null,
            string codevalue = null)
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Rebate);
            List<Rebate> list = _context.Rebate.Search(index: index.GetValueOrDefault(), pageSize: pagesize.GetValueOrDefault(), name: name, codeValue: codevalue);
            ViewBag.TotalCount = _context.Rebate.SearchCount(name: name, codeValue: codevalue);
            return View(list.ToView());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Rebate_Details);
            Rebate rebate = _context.Rebate.GetById(id);
            if (rebate == null)
            {
                return HttpNotFound();
            }
            return View(rebate);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Rebate_New);
            Rebate rebate = new Rebate() {
                Active = true
            };
            ViewBag.Message = BaseMessage.GetMessage();
            FillDropDowns(rebate);
            return View(rebate);
        }

        public ActionResult Print(string id)
        {
            Rebate rebate = _context.Rebate.GetFromRebateValue(id);
            return View(rebate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rebate rebate)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Rebate_New);

            if (IsModelValid(rebate))
            {
                rebate.UserId = _context.SiteUser.GetCurrentUser().ID;
                _context.Rebate.Insert(rebate);
                _context.Save();
                return RedirectToAction("Index");
            }

            FillDropDowns(rebate);
            return View(rebate);
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Rebate_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Rebate rebate = _context.Rebate.GetById(id);
            if (rebate == null)
                return HttpNotFound();

            FillDropDowns(rebate);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(rebate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rebate rebate)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Rebate_Edit);
            if (IsModelValid(rebate))
            {
                _context.Rebate.Update(rebate);
                _context.Save();
                return RedirectToAction("Index");
            }
            FillDropDowns(rebate);
            return View(rebate);
        }

        private bool IsModelValid(Rebate rebate)
        {
            Code type = _context.Code.GetById(rebate.TypeId);
            rebate.StartDatetime = rebate.StartDatetime.GetGregorian();
            rebate.EndDatetime = rebate.EndDatetime.GetGregorian();

            bool result = false;
            if (rebate.Name == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_REBATE_NAME);
            else if (rebate.CodeValue == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_REBATE_CODEVALUE);
            //else if (type.Label == Enum_Code.REBATE_TYPE_SHOP.ToString() && rebate.ShopId == null)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_REBATE_SHOP);
            else if (type.Label == Enum_Code.REBATE_TYPE_PRODUCTTYPE.ToString() && rebate.ProductTypeId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_REBATE_PRODUCTTYPE);
            else if (type.Label == Enum_Code.REBATE_TYPE_PRODUCTCATEGORY.ToString() && rebate.ProductCategoryId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_REBATE_PRODUCTCATEGORY);
            else if (type.Label == Enum_Code.REBATE_TYPE_PRODUCTSUBCATEGORY.ToString() && rebate.ProductSubCategoryId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_REBATE_PRODUCTSUBCATEGORY);
            else if (type.Label == Enum_Code.REBATE_TYPE_PRODUCTBRAND.ToString() && rebate.ProductBrandId == null)
                ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_REBATE_PRODUCTBRAND);
            else
                result = true;
            return result;
        }

        private void FillDropDowns(Rebate rebate)
        {
            ViewBag.TypeId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.REBATE_TYPE), "ID", "Name", rebate != null ? rebate.TypeId : 0);
            ViewBag.ShopId = new SelectList(_context.Shop.GetAllShopForUserId(_context.SiteUser.GetCurrentUser().ID), "ID", "Name", rebate != null ? rebate.ShopId : 0);
            ViewBag.ProductTypeId = new SelectList(_context.ProductType.GetAll(), "ID", "Name", rebate != null ? rebate.ProductTypeId : 0);
            ViewBag.ProductCategoryId = new SelectList(_context.ProductCategory.GetAll(), "ID", "Name", rebate != null ? rebate.ProductCategoryId : 0);
            ViewBag.ProductSubCategoryId = new SelectList(_context.ProductSubCategory.GetAll(), "ID", "Name", rebate != null ? rebate.ProductSubCategoryId : 0);
            ViewBag.ProductBrandId = new SelectList(_context.ProductBrand.GetAll(), "ID", "Name", rebate != null ? rebate.ProductBrandId : 0);
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Rebate_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Rebate rebate = _context.Rebate.GetById(id);
            if (rebate == null)
                return HttpNotFound();

            return View(rebate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Rebate_Delete);
            Rebate rebate = _context.Rebate.GetById(id);
            try
            {
                _context.Rebate.Delete(rebate);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(rebate);
            }

        }

        [HttpPost]
        public ActionResult CheckRebate(Rebate rebate)
        {
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.ERROR };
            if (rebate == null || string.IsNullOrEmpty(rebate.CodeValue))
                error = BaseMessage.GetMessage(Enum_Message.REQUIRED_REBATE_CODEVALUE);
            Rebate entity = _context.Rebate.GetFromRebateValue(rebate.CodeValue);
            if (entity == null)
                error = BaseMessage.GetMessage(Enum_Message.INVALID_REBATE_VALUE);
            else if (entity.Active == false)
                error = BaseMessage.GetMessage(Enum_Message.ERROR_REBATE_USED_BEFORE);
            else if (entity != null && entity.Active)
                error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            return new JsonResult() { Data = error };
        }

        [HttpPost]
        public ActionResult CancelRebate(Rebate rebate)
        {
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.ERROR };
            Rebate entity = _context.Rebate.GetFromRebateValue(rebate.CodeValue);
            if (entity != null)
            {
                entity.Active = false;
                entity.UserId = _context.SiteUser.GetCurrentUser().ID;
                _context.Save();
                error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            }
            return new JsonResult() { Data = error };
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