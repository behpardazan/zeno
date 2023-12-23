using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_City : BaseRepository<City>
    {
        private DatabaseEntities _context;
        public Entity_City(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<City> Search(
            int? countryId = null,
            int? stateId = null,
            int? notId = null,
            int? index = null,
            int? pageSize = null,
            string name = null,
            bool isActiveShipping= false,
            int storeId = 0)
        {
            var MyQuery = PredicateBuilder.True<City>();
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;

            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (countryId != null && countryId != 0)
                MyQuery = MyQuery.And(p => p.State.CountryId == countryId);
            if (stateId != null && stateId != 0)
                MyQuery = MyQuery.And(p => p.StateId == stateId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (isActiveShipping == true)
            {
                var storeStateList = _context.ShippingCity.Where(s => s.StoreId == storeId).ToList();
                foreach (var item in storeStateList)
                {
                    if(item.CityId==null && item.StateId!=null)
                    {
                        MyQuery = MyQuery.And(p => p.ID == item.CityId);
                        break;
                    }
                    else
                    {
                        MyQuery = MyQuery.And(p => p.ID != item.CityId);
                    }
                    
                }

            }
            return _context
                .City
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
    }
}
