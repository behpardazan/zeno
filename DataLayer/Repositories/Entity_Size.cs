using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Size : BaseRepository<Size>
    {
        private DatabaseEntities _context;
        public Entity_Size(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Size> GetAllProductTypeId(int ProductTypeId)
        {
            return _context.Size.Where(p =>
                p.ProductTypeId == ProductTypeId ||
                p.ProductTypeId == null
            ).OrderBy(p => p.Name).ToList();
        }

        public List<Size> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? typeId = null,
            string name = null)
        {
            List<Size> results = new List<Size>();
            var MyQuery = PredicateBuilder.True<Size>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (string.IsNullOrEmpty(name) == false)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (typeId != null)
                MyQuery = MyQuery.And(p => p.ProductTypeId == typeId);

            results = _context
                .Size
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
    }
}
