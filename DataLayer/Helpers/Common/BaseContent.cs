using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace DataLayer.Base
{
    public static class BaseContent
    {
        private static ViewWebsiteDetail _viewWebsiteDetail;
        public static ViewWebsiteDetail WebsiteDetail
        {
            get
            {
                if (_viewWebsiteDetail == null)
                {
                    UnitOfWork _context = new UnitOfWork();
                    var langId = Base.BaseLanguage.GetCurrentLanguageId(_context);
                    var websiteDetail = _context.WebsiteDetail.GetWebsiteDetail(langId);
                    _context.Dispose();
                    _viewWebsiteDetail = new ViewWebsiteDetail(websiteDetail);
                }
                return _viewWebsiteDetail;

            }
        }
    }
}
