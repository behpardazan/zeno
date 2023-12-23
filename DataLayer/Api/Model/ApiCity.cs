using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Api.Model
{
    public class ApiCity : ApiResponse
    {
        public static ApiResult Get(UnitOfWork _context, int? countryId = null, int? stateId = null, int? notId = null, int? index = null, int? pageSize = null, string name = null)
        {
            List<City> list = _context.City.Search(countryId: countryId, stateId: stateId, notId: notId, index: index, pageSize: pageSize, name: name);
            return CreateSuccessResult(list);
        }
    }
}
