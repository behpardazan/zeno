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
    public class SurveyQuestionController : Controller
    {
        UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(int? quid, string quname = "")
        {
            SurveyQuestion question = _context.SurveyQuestion.GetById(quid);
            return PartialView(BaseController.GetView(this), question);
        }
        public ActionResult Search(
            int? quNotId = null,
            string quQuestion = null,
            string quSurveyLabel = null,
            int? quSurveyId = null,
            int? quPageSize = 0,
            int? quIndex = null,
            string quViewName = null,
            Boolean? quActive = null,
            Enum_SurveyQuestion quOrder = Enum_SurveyQuestion.NONE)

        {
            quIndex = quIndex != null ? quIndex.Value : 1;
            quPageSize = quPageSize != null ? quPageSize.Value : 40;
            quViewName = quViewName == null ? "Search" : quViewName;
            List<SurveyQuestion> results = _context.SurveyQuestion.Search(
                notId: quNotId,
                surveyId: quSurveyId,
                pageSize: quPageSize,
                index: quIndex,
                order: quOrder,
                question: quQuestion,
                surveyLabel: quSurveyLabel,
                active:quActive
                );

            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, quViewName, results);
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