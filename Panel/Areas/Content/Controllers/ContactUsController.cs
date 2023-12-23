using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Panel.Areas.Content.Controllers
{
    public class ContactUsController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(string name = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ContactUs);
            return View(_context.ContactUs.Search(name: name, pageSize: 9999999));
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ContactUs);

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ContactUs contactus = _context.ContactUs.GetById(id);
            if (contactus == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(contactus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactUs contactus)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ContactUs);
            _context.ContactUs.Update(contactus);
            _context.Save();
            ViewBag.Message = BaseMessage.GetMessage();
            //return View(contactus);
            return RedirectToAction("Index");
        }
        //private bool IsModelValid(Gallery gallery)
        //{
        //    bool result = false;
        //    if (gallery.Name == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_GALLERY_NAME);
        //    else
        //        result = true;
        //    return result;
        //}
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