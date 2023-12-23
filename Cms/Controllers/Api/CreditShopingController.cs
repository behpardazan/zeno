using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class CreditShopingController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(CreditShoping creditShoping)
        {

            var account = _context.Account.GetById(creditShoping.AccountId);
            account.IsCreditShopingActive = false;
            _context.Account.Update(account);
            _context.Save();
            creditShoping.CreateDate = DateTime.Now;

            _context.CreditShoping.Insert(creditShoping);
            _context.Save();
            StringBuilder str = new StringBuilder();
            string token1 = creditShoping.FullName;
            string token2 = "";
            string token3 = "";
            str.AppendLine("همکار گرامی، درخواست شما با موفقیت ثبت و در حال بررسی میباشد.");
            _context.Sms.SaveNewSms(account.Mobile, Enum_SmsType.CREDIT, str.ToString(), token1, token2, token3);
            _context.Save();
            ApiResult result = new ApiResult()
            {
                Code = ApiResult.ResponseCode.Success,
                Message = BaseMessage.GetMessage(Enum_Message.SUCCESSFULL_CONTACTUS).Body
            };
            return result;
        }
        [HttpGet]
        public ApiResult getTrackingCode(string TrackingCode)
        {
            return ApiContactUs.GetTrackingCode(_context, TrackingCode);
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
