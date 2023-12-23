using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

using DataLayer.Base;
using DataLayer.Entities;



namespace Cms.Controllers
{
    public class SmartOfferController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            var model = _context.QuestionSmartOffer.Search(active: true);
            return PartialView(BaseController.GetView(this), model.ToView());
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