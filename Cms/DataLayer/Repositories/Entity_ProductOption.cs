using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductOption : BaseRepository<ProductOption>
    {
        private DatabaseEntities _context;
        public Entity_ProductOption(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductOption> Search(int? typeId = null, int? pageSize = null, int? index = null)
        {
            List<ProductOption> results = new List<ProductOption>();
            var MyQuery = PredicateBuilder.True<ProductOption>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            typeId = typeId == 0 ? null : typeId;
            MyQuery = MyQuery.And(p =>
                ((typeId != null) && p.ProductTypeId == typeId) ||
                 ((typeId == null) && p.ProductTypeId > 0) ||

                (p.ProductTypeId == null)
                );

            results = _context
                .ProductOption
                .OrderBy(p=>p.ProductTypeId).ThenBy(p => p.ShowNumber)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
       
    }
}
