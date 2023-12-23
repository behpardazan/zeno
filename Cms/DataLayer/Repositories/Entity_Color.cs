using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Color : BaseRepository<Color>
    {
        private DatabaseEntities _context;
        public Entity_Color(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Color> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? typeId = null,
            string name = null)
        {
            List<Color> results = new List<Color>();
            var MyQuery = PredicateBuilder.True<Color>();
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
                .Color
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<Color> GetAllProductTypeId(int ProductTypeId)
        {
            return _context.Color.Where(p => 
                p.ProductTypeId == ProductTypeId ||
                p.ProductTypeId == null
            )
            .OrderBy(p => p.HexValue)
            .ThenBy(p => p.Name)
            .ToList();
        }
    }
}
