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

namespace Panel.Areas.Content.Controllers
{
    public class DashboardFilesController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DashboardFiles);
            return View(_context.DashboardFiles.GetAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DashboardFiles_Details);
            DashboardFiles DashboardFiles = _context.DashboardFiles.GetById(id);
            if (DashboardFiles == null)
            {
                return HttpNotFound();
            }
            return View(DashboardFiles);
        }

        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DashboardFiles_New);
            return View(new DashboardFiles());
        }

        [HttpPost]
        public ActionResult Create(DashboardFiles DashboardFiles)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DashboardFiles_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

           
            if (IsModelValid(DashboardFiles))
            {
              
                _context.DashboardFiles.Insert(DashboardFiles);
                bool result = _context.Save();
                
            }

            return new JsonResult() { Data = error };

        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DashboardFiles_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DashboardFiles DashboardFiles = _context.DashboardFiles.GetById(id);
            if (DashboardFiles == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(DashboardFiles);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(DashboardFiles DashboardFiles)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DashboardFiles_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
          
            if (IsModelValid(DashboardFiles))
            {
                _context.DashboardFiles.Update(DashboardFiles);
                _context.Save();
                
                _context.Save();
            }
            return new JsonResult() { Data = error };

        }

        private bool IsModelValid(DashboardFiles DashboardFiles)
        {
            bool result = false;
            //if (DashboardFiles.FileId == null)
            //    ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_DashboardFiles_NAME);
            //else
                result = true;


            return result;
        }

     
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DashboardFiles_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DashboardFiles DashboardFiles = _context.DashboardFiles.GetById(id);
            if (DashboardFiles == null)
                return HttpNotFound();

            return View(DashboardFiles);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.DashboardFiles_Delete);
            DashboardFiles DashboardFiles = _context.DashboardFiles.GetById(id);
            try
            {
                //---
              
                //----
                _context.DashboardFiles.Delete(DashboardFiles);
                _context.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(DashboardFiles);
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