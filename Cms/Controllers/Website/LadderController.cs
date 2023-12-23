using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class LadderController : Controller
    {
        // GET: Merchant
        UnitOfWork _context = new UnitOfWork();
        public ActionResult GetActiveLeddares()
        {
            var activeLadders = _context.LadderSetting.GetAllActive();
          

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "Ladders", activeLadders);
        }
        public ActionResult GetCurrentLeddar()
        {
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            var account = _context.Account.GetById(CurrentUser.Id);
            LadderPayment ladderPayment = account.LadderPayment.Where(s => s.StatusId == 2068).OrderByDescending(s => s.Datetime).FirstOrDefault();
            //if (ladderPayment == null)
            //{
            //    LadderPayment laderpaymentTemp = new LadderPayment();
            //    laderpaymentTemp.LedderSettingId = 3;
            //    laderpaymentTemp.AccountId = CurrentUser.Id;
            //    laderpaymentTemp.StatusId = 2068;
            //    laderpaymentTemp.MerchantId = 2;
            //    laderpaymentTemp.RefNumber = "12345678";
            //    laderpaymentTemp.Amount = 0;
            //    laderpaymentTemp.Datetime = DateTime.Now;
            //    _context.LadderPayment.Insert(laderpaymentTemp);
            //    _context.Save();
            //}
            //ladderPayment = account.LadderPayment.Where(s => s.StatusId == 2068).OrderByDescending(s => s.Datetime).FirstOrDefault();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, "ActiveLadder", ladderPayment);
        }
    }
}