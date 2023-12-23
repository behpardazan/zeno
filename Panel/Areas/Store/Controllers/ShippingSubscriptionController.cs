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
    public class ShippingSubscriptionController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShippingSubscription);
            List<ShippingSubscription> listShippingSubscription = _context.ShippingSubscription.GetAll();
            FillDropDowns(null);
            return View(listShippingSubscription.ToView());
        }
        private void FillDropDowns(ShippingSubscription shipping)
        {
           
       
            ViewBag.StateId = new SelectList(_context.State.Where(s=>s.ID!=0), "ID", "Name", shipping != null ? shipping.StateId : 0);

        }


        public ActionResult Create()
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShippingSubscription_New);           
            FillDropDowns(null);
            return View();
        }

        [HttpPost]
        public ActionResult Create(ShippingSubscription ShippingSubscription)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShippingSubscription_New);


            FillDropDowns(null);
            ViewMessage result = IsModelValid(ShippingSubscription);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                var shipping = _context.ShippingSubscription.Where(s => s.StateId == ShippingSubscription.StateId).FirstOrDefault();
                if (shipping == null)
                {
                    _context.ShippingSubscription.Insert(ShippingSubscription);
                    _context.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_ShippingSubscription_Duplicate);
                }
                
               
            }
            ViewBag.Message = result;
            FillDropDowns(ShippingSubscription);
            return View(ShippingSubscription);
          
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShippingSubscription_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ShippingSubscription ShippingSubscription = _context.ShippingSubscription.GetById(id);
            if (ShippingSubscription == null)
                return HttpNotFound();
            FillDropDowns(ShippingSubscription);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(ShippingSubscription);
        }

        [HttpPost]
        public ActionResult Edit(ShippingSubscription ShippingSubscription)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.ShippingSubscription_Edit);
           
            ViewMessage result = IsModelValid(ShippingSubscription);
            if (IsModelValid(ShippingSubscription).Type == Enum_MessageType.SUCCESS)
            {
                var shipping = _context.ShippingSubscription.Where(s => s.StateId == ShippingSubscription.StateId && s.ID!= ShippingSubscription.ID).FirstOrDefault();
                if (shipping == null)
                {
                    _context.ShippingSubscription.Update(ShippingSubscription);
                    _context.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_ShippingSubscription_Duplicate);
                }
                     
            }
         
            ViewBag.Message = result;
            FillDropDowns(ShippingSubscription);
            return View(ShippingSubscription);
        }

        private ViewMessage IsModelValid(ShippingSubscription ShippingSubscription)
        {
            ViewMessage result = new ViewMessage();
            if (ShippingSubscription.Price == 0)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_ShippingSubscription_Price);
            return result;
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