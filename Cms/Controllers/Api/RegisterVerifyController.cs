using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class RegisterVerifyController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(string mobile, string token,string password)
        {
            token = token.ToEnglishDigit();
            Account account = _context.Account.GetByMobile(mobile);

            ApiResult result = ApiAccount.Post_RegisterVerify(_context, mobile, token);
            if (result.Code == ApiResult.ResponseCode.Success)
            {
                _context.Account.SetCurrentAccount((ViewAccount)result.Value, account.Password, true);
            }
            return result;
        }
        [HttpGet]
        public ApiResult Get(string mobile,string password = "")
        {

            ApiResult result = ApiAccount.Post_RegisterVerify(_context, mobile,true,password);
            return result;
        }
        [HttpGet]
        public ApiResult Get(bool isCustom,string mobile, string password = "")
        {

            ApiResult result = ApiAccount.Post_RegisterVerify2(_context, mobile, true, password,isCustom);
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
