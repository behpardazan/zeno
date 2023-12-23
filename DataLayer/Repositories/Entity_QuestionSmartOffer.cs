using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionSmartOffer : BaseRepository<QuestionSmartOffer>
    {
        private DatabaseEntities _context;
        public Entity_QuestionSmartOffer(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<QuestionSmartOffer> Search( int? pageSize = null, int? index = null,bool ? active=null)
        {
            List<QuestionSmartOffer> results = new List<QuestionSmartOffer>();
            var MyQuery = PredicateBuilder.True<QuestionSmartOffer>();
            pageSize = pageSize == null ? 50 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            if (active != null)
            {
                MyQuery = MyQuery.And(p => p.Active == active);

            }

            results = _context
                .QuestionSmartOffer
                .OrderBy(p => p.ShowNumber)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
    }
}
