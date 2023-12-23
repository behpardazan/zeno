using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class AccountController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
        [HttpGet]
        public ApiResult Get()
   {
            ViewAccount currentAccount = _context.Account.GetCurrentAccount();
            if (currentAccount == null)
                return ApiResponse.CreateInvalidKeyResult();
            if (currentAccount.BirthDay != null)
            {
                currentAccount.FaBirthDay = currentAccount.BirthDay.ToPersian();
            }
            return ApiResponse.CreateSuccessResult(currentAccount);
        }

        [HttpPut]
        public ApiResult Put(ViewAccount account)
        {
            ApiResult result = new ApiResult();
            ViewAccount currentAccount = _context.Account.GetCurrentAccount();
            if (currentAccount == null)
                return ApiResponse.CreateInvalidKeyResult();
            account.Id = currentAccount.Id;
            result = ApiAccount.Put_Account(_context, account, account.Id);
            if (result.Code == ApiResult.ResponseCode.Success)
                _context.Account.SetCurrentAccount((ViewAccount)result.Value, currentAccount.Password, true);
            return result;
        }

        [HttpPut]
        public ApiResult Put(string oldPassword, string newPassword, string confirmPassword)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return ApiResponse.CreateInvalidKeyResult();
            ApiResult result = ApiAccount.Put_ChangePassword(_context, account.Id, oldPassword, newPassword, confirmPassword);
            if (result.Code == ApiResult.ResponseCode.Success)
                _context.Account.SetCurrentAccount((ViewAccount)result.Value, newPassword, false);
            return result;
        }

        [HttpPut]
        public ApiResult Put(string userName)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return ApiResponse.CreateInvalidKeyResult();
            ApiResult result = ApiAccount.Put_ChangeUserName(_context, account.Id, userName);
            //(_context, account.Id, oldPassword, newPassword, confirmPassword);
            if (result.Code == ApiResult.ResponseCode.Success)
                _context.Account.SetCurrentAccount((ViewAccount)result.Value, userName, false);
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
