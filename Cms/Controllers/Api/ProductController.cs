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
    public class ProductController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();

        [HttpGet]
        public ApiResult Get(int id)
        {
            Product product = _context.Product.GetById(id);
            return ApiResponse.CreateSuccessResult(product.ToApi());
        }

        [HttpGet]
        public ApiResult Get(int? notId = null,
            int? index = null,
            int? pageSize = null,
            string shopId = null,
            string typeId = null,
            string categoryId = null,
            string subCategoryId = null,
            string brandId = null,
            string name = null,
            string custom = null,
            int? pricefrom = null,
            int? priceto = null,
            bool? isService=null,
            bool activeLocation = false,
            bool exteraResult = false,
            string codeValue=null,
            Enum_ProductOrder order = Enum_ProductOrder.MORE_VISIT,
            bool? isPublish = null,
             bool? isAdvertising = null)
       {
            ApiResult result = ApiProduct.Search(
                _context: _context, 
                notId: notId, 
                index: index, 
                pageSize: pageSize, 
                shopId: shopId, 
                typeId: typeId, 
                categoryId: categoryId,
                brandId: brandId,
                custom: custom,
                name: name,
                order: order,
                pricefrom: pricefrom,
                priceto: priceto,
                subCategoryId: subCategoryId ,
                isService: isService,
                activeLocation: activeLocation,
                exteraResult:exteraResult,
                codeValue: codeValue,
                isPublish: isPublish,
                isAdvertising: isAdvertising
                );
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
