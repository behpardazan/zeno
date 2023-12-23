using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Crm.Controllers
{
    public class CreditShopingController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        private UnitOfWork _Unitcontext = new UnitOfWork();


        public ActionResult Index(bool? active = null, string pageIndex = null,
           string pageSize = null)
        {
            int size = pageSize == null ? 50 : pageSize.GetInteger();
            int index = pageIndex == null ? 1 : pageIndex.GetInteger();
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.active = active;

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CreditShoping);
            ViewBag.TotalCount = _context.CreditShoping.SearchCount(active: active);
            return View(_context.CreditShoping.Search(pageSize: size, index: index, active: active));
        }



        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CreditShoping_Answer);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CreditShoping creditShoping = _context.CreditShoping.GetById(id);
            if (creditShoping == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(creditShoping);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreditShoping creditShoping)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.CreditShoping_Answer);
            CreditShoping lastcreditShoping = _Unitcontext.CreditShoping.GetById(creditShoping.ID);
            if (creditShoping.Active == true && lastcreditShoping.Active != true)
            {
                creditShoping.ActiveDate = DateTime.Now;
                
               
                
            }
            _context.CreditShoping.Update(creditShoping);
            _context.Save();
            var account = _context.Account.GetById(creditShoping.AccountId);
            account.IsCreditShopingActive = creditShoping.Active;
            _context.Account.Update(account);
            _context.Save();
            return RedirectToAction("Index");

        }

      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                _Unitcontext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}