using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api
{
    public class ApiProduct : ApiResponse
    {
        public static ApiResult Search(
            UnitOfWork _context,
            int? notId = null,
            int? index = null, 
            int? pageSize = null, 
            string shopId = null, 
            string typeId = null, 
            string categoryId = null,
            string colorId = null,
            string sizeId = null,
            string subCategoryId = null,
            string brandId = null,
            string name = null,
            string custom = null,
            int? pricefrom = null,
            int? priceto = null,
            bool? extra = true,
            string discount = null,
            Enum_ProductOrder order = Enum_ProductOrder.NONE,
            bool? isService = null,
             string latitude = null,
            string longitude = null,
            int distance = 0,
            bool exteraResult=false,
            int ? notProductId=null,
             string option = null,
            bool activeLocation = true, string stock = "ALL",
              string storeId = null,
              string codeValue=null,
              bool? isAdvertising = null, 
              bool? isPublish = true
            )
        {
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            List<Product> list = _context.Product.Search(
                notId: notId,
                shopId: shopId,
                typeId: typeId,
                categoryId: categoryId,
                brandId: brandId,
                subCategoryId: subCategoryId,
                index: index.Value,
                pageSize: pageSize.Value,
                name: name,

                custom: custom,
                pricefrom: pricefrom,
                priceto: priceto,
                order: order,
                discount: discount,
                isService:isService,
                latitude:latitude,
                longitude:longitude,
                distance:distance,
                colorId: colorId,
                sizeId: sizeId,
                option: option,
                prstock: stock,
                storeId: storeId,
                activeLocation:activeLocation,
                codeValue: codeValue,
                isAdvertising: isAdvertising,
                isPublish:isPublish);
            if(exteraResult)
            {
                var exteraModel = new ViewModels.ViewApiExtera();
                var toApi = list.ToApi(extra);
                exteraModel.List = toApi;
                var detail= _context.Product.SearchDetail(
                notId: notId,
                shopId: shopId,
                typeId: typeId,
                categoryId: categoryId,
                brandId: brandId,
                subCategoryId: subCategoryId,
                name: name,
                custom: custom,
                pricefrom: pricefrom,
                priceto: priceto,
                discount: discount,
                isService: isService,
                storeId:storeId,
                  prstock: stock,
                option:option,
                latitude: latitude,
                longitude: longitude,
                distance: distance,
                colorId: colorId,
                sizeId: sizeId,
                activeLocation:activeLocation,
                codeValue: codeValue,
                isAdvertising:isAdvertising,
                isPublish:isPublish);
                exteraModel.Count = detail.Count;
                exteraModel.MinPrice = detail.MinPrice;
                exteraModel.MaxPrice = detail.MaxPrice;
                return ApiResponse.CreateSuccessResult(exteraModel);
            }
            return ApiResponse.CreateSuccessResult(list.ToApi(extra));
        }



    }
}
