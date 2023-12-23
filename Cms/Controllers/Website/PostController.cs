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
    public class PostController : Controller
    {
        UnitOfWork _context = new UnitOfWork();
        public ActionResult Index(int? poid, string poidname = "")
        {
            ViewPostModel model = new ViewPostModel();
            Post post = _context.Post.GetById(poid);
            //if (!string.IsNullOrEmpty(post.URL))
            //{
            //    if (post.URL.StandardUrl() != poidname)
            //    {
            //        return RedirectPermanent(post.GetLink());
            //    }
            //}
           string currentUrl = Request.Url.ToString()/*.Replace("http:", "https:")*/.ToLower();
            if (currentUrl != post.GetLinkWithUrl())
            {

                HttpContext.Response.Status = "301 Moved Permanently";
                HttpContext.Response.AddHeader("Location", post.GetLinkWithUrl());
            }

            post.VisitCount = post.VisitCount + 1;
            model.Post = post;
            model.RelatedPost = _context.Post.Where(s => s.CategoryId == post.CategoryId && s.ID!=poid).Take(3).OrderByDescending(s => s.CreateDateTime).ToList();
            _context.Post.Update(post);
            _context.Save();
            ViewBag.CurrentAccount = _context.Account.GetCurrentAccount();

            int prpageSize = 10;
            int prindex = 1;
            List<Product> results = new List<Product>();
            foreach (var item in model.Post.TagPost)
            {

                foreach (var tagsub in item.Tag.TagSubcategory)
                {
                    var searchQuery = _context.Product.GetSearchQuery(subCategoryId: tagsub.SubCategoryId.ToString());
                    var products = _context.Product.Search(
             pageSize: prpageSize,
             index: prindex,
            query: searchQuery);

                    foreach (var product in products)
                    {
                        results.Add(product);
                    }
                }

            }
            model.RelatedProduct = results;
            return PartialView(BaseController.GetView(this), model);
        }
        public ActionResult ManagersState(string poidlabel = "Managers",string poviewName = "")
        {
            ViewPostModel model = new ViewPostModel();
            List<Post> results = _context.Post.Search(
              categoryLabel: poidlabel
              );
            var listState = _context.State.Where(s => s.Post.Any()).ToList();
            //var listState = _context.Post.Search(
            //    categoryLabel: poidlabel,
            //    pageSize:1000,
            //    index:1
            //    ).Select(s => s.State).Distinct().ToList();
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, listState);

        }
        public ActionResult Managers(string poidlabel = "Managers", int? stateId = null, int? poIndex = null, int? poSize = null)
        {
            ViewPostModel model = new ViewPostModel();
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 10;
            List<Post> results = _context.Post.Search(
              categoryLabel: poidlabel,
              index: poIndex.Value,
              pageSize: poSize.Value,
              stateId: stateId

              );
            var listState = _context.Post.Search(
                categoryLabel: poidlabel,
                index: 1,
                pageSize: 500

                ).Select(s => s.State).Distinct();
            ViewBag.StateId = new SelectList(listState, "ID", "Name").ToList();
            ViewSearchPost search = new ViewSearchPost()
            {
                TotalCount = _context.Post.SearchCount(categoryLabel: poidlabel, stateId: stateId),
                Category = _context.Category.FirstOrDefault(p => p.Label == poidlabel),
                PageIndex = poIndex.Value,
                PageSize = poSize.Value,
                Results = results
            };
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poidlabel, search);

        }
        public ActionResult FillCity(int? stateId)
        {
            return new JsonResult()
            {
                Data = _context.City.Search(stateId: stateId).ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult FillState()
        {
            return new JsonResult()
            {
                Data = _context.State.GetAll().ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult SearchCategory(string poName="",bool ? active=true,int ? stateId=null,int? cityId=null, string poidlabel = null, string poviewName = null, int? poIndex = null, int? poSize = null, Enum_PostOrder poorder = Enum_PostOrder.NONE)
        {
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 10;
            poviewName = poviewName == null ? poidlabel : poviewName;
            ViewBag.StateId = new SelectList(_context.State.GetAll(), "ID", "Name");
            ViewBag.CityId = new SelectList(new List<City>(), "ID", "Name");

            List<Post> results = _context.Post.Search(
                categoryLabel: poidlabel,
                stateId:stateId,
                index: poIndex.Value,
                pageSize: poSize.Value,
                order: poorder,
                cityId: cityId,
                name: poName,
                active: active

                );
            ViewSearchPost search = new ViewSearchPost()
            {
                TotalCount = _context.Post.SearchCount(categoryLabel: poidlabel),
                Category = _context.Category.FirstOrDefault(p => p.Label == poidlabel),
                PageIndex = poIndex.Value,
                PageSize = poSize.Value,
                Results = results
            };
            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, search);
        }

        public ActionResult Search(
            int? websiteId = null,
            string pocategory = null,
            int? notId = null,
            int? pocategoryId = null,
            int? poIndex = null,
            int? poSize = null,
            int? poTypeId = null,
            string pokeyword = null,
            string poviewName = null,
            string url=null,
            Enum_PostOrder poorder = Enum_PostOrder.NONE,
            Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
            string name = null
            )

        {

            poviewName = poviewName == null ? "Search" : poviewName;
            List<Post> results = SearchContent(
                websiteId, pocategory, notId, pocategoryId,
                poIndex, poSize, poTypeId, pokeyword, poviewName,
                url, poorder, pooutput, name);


            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, results);
        }
        [OutputCache(Duration = 1800, VaryByParam = "*")]

        public ActionResult SearchPartial(
            int? websiteId = null,
            string pocategory = null,
            int? notId = null,
            int? pocategoryId = null,
            int? poIndex = null,
            int? poSize = null,
            int? poTypeId = null,
            string pokeyword = null,
            string poviewName = null,
            string url=null,
            Enum_PostOrder poorder = Enum_PostOrder.NONE,
            Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
            string name = null
            )

        {

            poviewName = poviewName == null ? "Search" : poviewName;
            List<Post> results = SearchContent(
                websiteId, pocategory, notId, pocategoryId,
                poIndex, poSize, poTypeId, pokeyword, poviewName,
                url, poorder, pooutput, name);


            return BaseController.GetView(this, Enum_ResultType.RESULT_TYPE_VIEWNAME, poviewName, results);
        }   
        public List<Post> SearchContent(
            int? websiteId = null,
            string pocategory = null,
            int? notId = null,
            int? pocategoryId = null,
            int? poIndex = null,
            int? poSize = null,
            int? poTypeId = null,
            string pokeyword = null,
            string poviewName = null,
            string url=null,
            Enum_PostOrder poorder = Enum_PostOrder.NONE,
            Enum_PostOutput pooutput = Enum_PostOutput.ENTITY,
            string name = null
            )

        {
            poIndex = poIndex != null ? poIndex.Value : 1;
            poSize = poSize != null ? poSize.Value : 40;
            var currentLang = BaseLanguage.GetCurrentLanguage();
            List<Post> results = _context.Post.Search(
                websiteId: websiteId,
                categoryId: pocategoryId,
                categoryLabel: pocategory,
                keyword: pokeyword,
                index: poIndex.Value,
                pageSize: poSize.Value,
                order: poorder,
                name: name,
                notId: notId,
                typeId:poTypeId,
                lang: currentLang,
                url: url
                );


            return results;
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