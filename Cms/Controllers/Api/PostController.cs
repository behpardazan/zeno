using DataLayer.Api;
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
    public class PostController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
               
        [HttpGet]
        public ApiResult Get(
            int? index = null,
            int? pageSize = null,
            int? typeId = null,
            string name = null,
            string category=null,
            Enum_PostOrder order = Enum_PostOrder.NEW)
        {
            ApiResult result = ApiPost.Search(
                _context: _context, 
                index: index, 
                pageSize: pageSize, 
                name: name,
                category:category,
                typeId:typeId,
                order: order);
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
