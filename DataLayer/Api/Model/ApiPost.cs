using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api
{
    public class ApiPost : ApiResponse
    {
        public static ApiResult Search(
            UnitOfWork _context,
            int? index = null, 
            int? pageSize = null,
            int? typeId = null,
            string category = null,          
            string name = null,
            Enum_PostOrder order=Enum_PostOrder.NEW)
        {
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            List<Post> list = _context.Post.Search(

                categoryLabel: category,
                index: index.Value,
                pageSize: pageSize.Value,
                name: name,
                order:order,
                typeId:typeId
                );

            return ApiResponse.CreateSuccessResult(list.ToApi());
        }



    }
}
