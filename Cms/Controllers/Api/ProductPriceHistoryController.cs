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
    public class ProductPriceHistoryController : ApiController
    {
        // GET: City
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(int productId)
        {
            var currentAccount = BaseWebsite.CurrentAccount;
            ViewProductPriceHistory list = new ViewProductPriceHistory();

            list = _context.ProductPiceHistory.Where(x => x.ProductId == productId).ToList().ToApi();

            return ApiResponse.CreateSuccessResult(list);
        }
    }
}