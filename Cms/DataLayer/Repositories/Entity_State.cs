using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_State : BaseRepository<State>
    {
        private DatabaseEntities _context;
        public Entity_State(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<State> Search(
            int? countryId = null,
            int? notId = null,
            int? index = null,
            int? pageSize = null,
            string name = null,
            bool isActiveShipping=false,
            int storeId=0)
        {
            var MyQuery = PredicateBuilder.True<State>();
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;

            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (countryId != null && countryId != 0)
                MyQuery = MyQuery.And(p => p.CountryId == countryId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (isActiveShipping == true)
            {
                var storeStateList = _context.ShippingCity.Where(s => s.StoreId == storeId).ToList();
                foreach(var item in storeStateList)
                {
                    MyQuery = MyQuery.And(p => p.ID!=item.StateId);
                }
                
            }
            return _context
                .State
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
    }
}
