using DataLayer.Api;
using DataLayer.Api.Model;
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
    public class QuestionFormController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(Question form)
        {

               ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            form.AccountId = CurrentUser.Id;
            return ApiQuestionForm.Post_Register(_context, form);
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
