using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Survey : BaseRepository<Survey>
    {
        DatabaseEntities _context;

        public Entity_Survey(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Survey> Search(
            int? notId = 0,
            int? pageSize = 10,
            int? index = 1,
            string name = null

            )
        {
            var MyQuery = PredicateBuilder.True<Survey>();
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));

            return _context
                .Survey
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }
    }
}
