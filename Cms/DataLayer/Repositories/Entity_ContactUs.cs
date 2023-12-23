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
    public class Entity_ContactUs : BaseRepository<ContactUs>
    {
        DatabaseEntities _context;



        public Entity_ContactUs(DatabaseEntities context) : base(context)
        {
            _context = context;
        }



        public List<ContactUs> Search(
            int? pageSize = 20,
            int? index = 1,
            string name = null)
        {
            var MyQuery = PredicateBuilder.True<ContactUs>();
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            if (!string.IsNullOrEmpty(name))
                MyQuery = MyQuery.And(p => p.Title.Contains(name) || p.Name.Contains(name) || p.Phone.Contains(name) || p.Email.Contains(name));


            var a = _context
                 .ContactUs
                 .Where(MyQuery)
                 .OrderByDescending(p => p.ID)
                 .Skip(skipValue)
                 .Take(pageValue)
                 .ToList();
            return a;
        }
    }
}
