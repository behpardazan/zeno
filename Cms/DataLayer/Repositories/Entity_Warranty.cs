using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Warranty : BaseRepository<Warranty>
    {
        private DatabaseEntities _context;
        public Entity_Warranty(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<Warranty> Search(int index = 1, int pageSize = 10,string serial="")
        {
            var MyQuery = GetSearchQuery(serial);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;


            return _context.Warranty.OrderByDescending(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }

        public int SearchCount()
        {
            var MyQuery = GetSearchQuery();
            return _context.Warranty.Count(MyQuery);
        }

        private Expression<Func<Warranty, bool>> GetSearchQuery(string serial = "")
        {
            var MyQuery = PredicateBuilder.True<Warranty>();

            serial = serial.ToEnglishDigit();
            if (serial != null && serial != "")
                MyQuery = MyQuery.And(p => p.Serial == serial);
            return MyQuery;
        }
    }
}
