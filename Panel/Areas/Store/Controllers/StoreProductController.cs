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
    public class StoreProductController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(int? index = null, int? pagesize = null, int? statusId = null, int? storeId = null, string name = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreProduct);

            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;
            var model = _context.StoreProduct.Search(statusId: statusId, index: index, pageSize: pagesize, name: name, storeId: storeId);
            ViewBag.TotalCount = _context.StoreProduct.SearchCount(statusId: statusId, name: name, storeId: storeId);
            FillDropDowns();
            return View(model);
        }
        public void ChangeStatus(int storeProductId, int statusId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.StoreProduct);

            var model = _context.StoreProduct.GetById(storeProductId);
            if (model != null)
            {
                model.StatusId = statusId;
                _context.Save();
            }
        }
        private void FillDropDowns()
        {
            ViewBag.Statuses = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.STORE_REQUEST), "ID", "Name");
            ViewBag.Stores = new SelectList(_context.Store.GetAll(), "ID", "Name");
        }
    }
}