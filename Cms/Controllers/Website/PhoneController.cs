using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class PhoneController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index()
        {
            bool isHoliday = false;
            bool isThursday = false;
           
            var phone = "";
            DateTime date = DateTime.Now;
            var hour = date.Hour;
            var perDate = date.ToPersianComplete();
            var temp = date.DayOfWeek;
            var custom = date.ToPersian();
            try
            {
                isHoliday = BaseDate.IsHoliday(custom.Replace("/", "-"));
            }
            catch
            {
                isHoliday = false;
            }
           

            switch (temp)
            {

                case DayOfWeek.Thursday:
                    isThursday =true;
                    break;
                case DayOfWeek.Friday:
                    isHoliday = true;
                    break;
               
            }
            if (isHoliday)
            {
                phone = BaseContent.WebsiteDetail.Mobile;
            }
            else if (isThursday)
            {
                
                if(hour>=9 && hour <= 12)
                {
                    phone = BaseContent.WebsiteDetail.Phone;
                }
                else
                {
                    phone = BaseContent.WebsiteDetail.Mobile;
                }
            }
            else
            {
                if (hour >= 9 && hour <= 16)
                {
                    phone = BaseContent.WebsiteDetail.Phone;
                }
                else
                {
                    phone = BaseContent.WebsiteDetail.Mobile;
                }
            }
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, null, phone);
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