using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Enumarables;
using DataLayer.ViewModels;

namespace Panel.Areas.Content.Controllers
{
    public class StateController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.Post);
            return View(Context.State.GetAll());
        }



        public ActionResult Create()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.State_New);
            State State = new State();
            return View(State);
        }

        [HttpPost]

        public ActionResult Create(State State)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.State_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<StateLanguage> listLanguage = State.StateLanguage.ToList();
            State.StateLanguage.Clear();
            //--------
          
            //-----
            if (IsModelValid(State, out error))
            {
                Context.State.Insert(State);
                bool result = Context.Save();
                if (result == true)
                {
                    foreach (StateLanguage item in listLanguage)
                    {
                        item.StateId = State.ID;
                        Context.StateLanguage.Insert(item);
                    }
                    Context.Save();
                }
            }
            return new JsonResult() { Data = error };
        }

        public ActionResult Edit(int? id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.State_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            State State = Context.State.GetById(id);
            if (State == null)
                return HttpNotFound();
            ViewBag.Message = BaseMessage.GetMessage();
            return View(State);
        }

        [HttpPost]
        public ActionResult Edit(State State )
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.State_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<StateLanguage> listLanguage = State.StateLanguage.ToList();
            State.StateLanguage.Clear();
            Context.StateLanguage.DeleteByStateId(State.ID);
            //--------
            
            //-----

            if (IsModelValid(State, out error))
            {
                Context.State.Update(State);
                Context.Save();
                foreach (StateLanguage item in listLanguage)
                {
                    item.StateId = State.ID;
                    Context.StateLanguage.Insert(item);
                }
                Context.Save();
            }
            return new JsonResult() { Data = error };
        }

        //private bool IsModelValid(State State, HttpPostedFileBase file)
        //{
        //    bool result = false;
        //    if (State.Name == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_State_NAME);
        //    else if (State.WebsiteId == 0)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_State_WEBSITE_NAME);
        //    else
        //        result = true;

        //    if (file != null)
        //    {
        //        Picture picture = Context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
        //        State.PictureId = picture.ID;
        //    }
        //    return result;
        //}


        private bool IsModelValid(State State, out ViewMessage msg)
        {
            bool result = false;
            //product.DocId = product.DocId != 0 ? product.DocId : null;
            //product.PictureId = product.PictureId != 0 ? product.PictureId : null;
            //product.ProductTypeId = product.ProductTypeId != 0 ? product.ProductTypeId : null;
            //product.ProductStateId = product.ProductStateId != 0 ? product.ProductStateId : null;
            //product.ProductSubStateId = product.ProductSubStateId != 0 ? product.ProductSubStateId : null;
            //if (product.ShopId == 0)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SHOP);
            if (State.Name == null)
            {
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            }
            //else if (product.StatusId == 0)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            //else if (product.Name.Length > 40)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            else
            {
                msg = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
                result = true;
            }
            return result;
        }

       
      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}