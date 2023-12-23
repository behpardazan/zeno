using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;
using DataLayer.ViewModels;

namespace Cms.Controllers.Website
{
    public class CreditShopingController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            CreditShoping creditShoping = new CreditShoping();
            ViewAccount account = _context.Account.GetCurrentAccount();
            
            if (account == null)
                return RedirectToAction("login", new { controller = "account", back = "CreditShoping"  });
            else
            {
                var tempAccount = _context.Account.GetById(account.Id);
                ViewBag.IsCreditShopingActive = tempAccount.IsCreditShopingActive;
                creditShoping = _context.CreditShoping.Where(s => s.AccountId == account.Id).FirstOrDefault();
            }
            return PartialView(BaseController.GetView(this), creditShoping);

        }
        public ActionResult GetActiveCreditShoping()
        {
            bool ? activeCredit = false;
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account != null)
            {
                activeCredit = _context.Account.GetById(account.Id).IsCreditShopingActive;
            }
            activeCredit = activeCredit == null ? false : activeCredit;
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "CreditShoping", activeCredit);
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