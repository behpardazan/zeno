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
    public class CityController : Controller
    {
        private UnitOfWork Context = new UnitOfWork();

        public ActionResult Index()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.City);
            return View(Context.City.GetAll());
        }



        public ActionResult Create()
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.City_New);
            FillDropDowns(null);
            City City = new City();
            return View(City);
        }

        [HttpPost]

        public ActionResult Create(City City)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.City_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<CityLanguage> listLanguage = City.CityLanguage.ToList();
            City.CityLanguage.Clear();
            //--------
          
            //-----
            if (IsModelValid(City, out error))
            {
                Context.City.Insert(City);
                bool result = Context.Save();
                if (result == true)
                {
                    foreach (CityLanguage item in listLanguage)
                    {
                        item.CityId = City.ID;
                        Context.CityLanguage.Insert(item);
                    }
                    Context.Save();
                }
            }
            return new JsonResult() { Data = error };
        }

        public ActionResult Edit(int? id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.City_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            City City = Context.City.GetById(id);
            if (City == null)
                return HttpNotFound();

            FillDropDowns(City);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(City);
        }

        [HttpPost]
        public ActionResult Edit(City City )
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.City_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            List<CityLanguage> listLanguage = City.CityLanguage.ToList();
            City.CityLanguage.Clear();
            Context.CityLanguage.DeleteByCityId(City.ID);
            //--------
            
            //-----

            if (IsModelValid(City, out error))
            {
                Context.City.Update(City);
                Context.Save();
                foreach (CityLanguage item in listLanguage)
                {
                    item.CityId = City.ID;
                    Context.CityLanguage.Insert(item);
                }
                Context.Save();
            }
            return new JsonResult() { Data = error };
        }

        //private bool IsModelValid(City City, HttpPostedFileBase file)
        //{
        //    bool result = false;
        //    if (City.Name == null)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_City_NAME);
        //    else if (City.WebsiteId == 0)
        //        ViewBag.Message = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_City_WEBSITE_NAME);
        //    else
        //        result = true;

        //    if (file != null)
        //    {
        //        Picture picture = Context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
        //        City.PictureId = picture.ID;
        //    }
        //    return result;
        //}


        private bool IsModelValid(City City, out ViewMessage msg)
        {
            bool result = false;
            //product.DocId = product.DocId != 0 ? product.DocId : null;
            //product.PictureId = product.PictureId != 0 ? product.PictureId : null;
            //product.ProductTypeId = product.ProductTypeId != 0 ? product.ProductTypeId : null;
            //product.ProductCityId = product.ProductCityId != 0 ? product.ProductCityId : null;
            //product.ProductSubCityId = product.ProductSubCityId != 0 ? product.ProductSubCityId : null;
            //if (product.ShopId == 0)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SHOP);
            if (City.Name == null)
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

        private void FillDropDowns(City City)
        {
            ViewBag.StateId = new SelectList(Context.State.GetAll(), "ID", "Name", City != null ? City.StateId : 0);
        }

        public ActionResult Delete(int? id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.City_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            City City = Context.City.GetById(id);
            if (City == null)
                return HttpNotFound();

            return View(City);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Context.Permission.CheckPagePermission(Context.SiteUser.GetCurrentUser(), Enum_Permission.City_Delete);
            City City = Context.City.GetById(id);
            Context.City.Delete(City);
            Context.Save();
            return RedirectToAction("Index");
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