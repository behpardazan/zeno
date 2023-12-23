using DataLayer.Base;
using DataLayer.Helpers;
using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiQuestionForm : ApiResponse
    {
        public static ApiResult Post_Register(UnitOfWork _context, Question form)
        {
            if (form == null)
                return CreateErrorResult(Enum_Message.REQUIRED_CONTACT_FORM);
            if (string.IsNullOrEmpty(form.QuestionText))
                return CreateErrorResult(Enum_Message.REQUIRED_CONTACT_FORM_BODY);
            form.AccountId = form.AccountId;
            form.CreateDate = DateTime.Now;
            form.IsActive = false;
            form.IsPrivate = true;
            form.Answer = "";
            _context.Question.Insert(form);
            _context.Save();
            return CreateSuccessResult();
        }
    }
}
