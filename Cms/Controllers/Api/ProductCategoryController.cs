using DataLayer.Api;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class ProductCategoryController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
               
        [HttpGet]
        public ApiResult Get(int TypeId)
        {
            var list = _context.ProductCategory.Search(typeId: TypeId, pageSize: 100);
            ApiResult result = ApiResponse.CreateSuccessResult(list.ToApi());
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
