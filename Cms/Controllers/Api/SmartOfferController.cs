using DataLayer.Api;
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
    public class SmartOfferController : ApiController
    {
        // GET: City
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(string smartOffer = "")
        {
            var searchQuery = _context.Product.GetSearchQuery(
prcustomLabel: "all",
                 smartOffer: smartOffer
                 );
            List<Product> results = _context.Product.Search(
             pageSize: 100000,
             index: 1,
            query: searchQuery);

            return ApiResponse.CreateSuccessResult(results.Count());
        }
    }
}