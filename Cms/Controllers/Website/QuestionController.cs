using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Cms.Controllers
{
    public class QuestionController : Controller
    {
        UnitOfWork _context = new UnitOfWork();


        public ActionResult Search(

            int? qoIndex = null,
            int? qoSize = null,
            string qoviewName = null,
            string keyword = "")

        {
            qoIndex = qoIndex != null ? qoIndex.Value : 1;
            qoSize = qoSize != null ? qoSize.Value : 40;
            qoviewName = qoviewName == null ? "Search" : qoviewName;
            var currentLang = BaseLanguage.GetCurrentLanguage();
            List<Question> results = _context.Question.Search(
                IsActive: true,
                IsPrivate: false,
             keyword: keyword,
             index: qoIndex.Value
             , pageSize: qoSize.Value
                );
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, qoviewName, results);

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