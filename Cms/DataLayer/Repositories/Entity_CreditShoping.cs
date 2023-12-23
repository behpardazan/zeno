using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_CreditShoping : BaseRepository<CreditShoping>
    {
        private DatabaseEntities _context;
        public Entity_CreditShoping(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public int SearchCount(
       int? notId = null,
          int? accountId = null,
        
          bool? active = null,
        
          int? id = null
       )
        {
            var MyQuery = GetSearchQuery(
                notId: notId,
                accountId: accountId,

                active: active,
               
                id: id
             );
            return _context.CreditShoping.Count(MyQuery);
        }
        private Expression<Func<CreditShoping, bool>> GetSearchQuery(
         int? notId = null,
            int? accountId = null,
           
            bool? active = null,
          
            int? id = null
        )
        {
            List<CreditShoping> results = new List<CreditShoping>();
            var MyQuery = PredicateBuilder.True<CreditShoping>();
            if (id != null)
                MyQuery = MyQuery.And(p => p.ID == id);
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (accountId != null)
                MyQuery = MyQuery.And(p => p.AccountId == accountId);

            if (active != null)
                MyQuery = MyQuery.And(p => (p.Active != null ? p.Active : false) == active);
            
            return MyQuery;
        }
        public List<CreditShoping> Search(
            int? notId = null,
            int? accountId = null,
          
            int? index = 1,
            int? pageSize = null,
            bool? active = null,
           
            int? id = null)
        {
            List<CreditShoping> results = new List<CreditShoping>();
            var MyQuery = PredicateBuilder.True<CreditShoping>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (id != null)
                MyQuery = MyQuery.And(p => p.ID == id);
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (accountId != null)
                MyQuery = MyQuery.And(p => p.AccountId == accountId);
           
            if (active != null)
                MyQuery = MyQuery.And(p => (p.Active != null ? p.Active : false) == active);

           

            results = _context
                .CreditShoping
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
    }
}
