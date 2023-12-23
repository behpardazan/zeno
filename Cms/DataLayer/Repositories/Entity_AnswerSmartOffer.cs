using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AnswerSmartOffer : BaseRepository<AnswerSmartOffer>
    {
        private DatabaseEntities _context;
        public Entity_AnswerSmartOffer(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<AnswerSmartOffer> Search(int? questionId = null, int? pageSize = null, int? index = null)
        {
            List<AnswerSmartOffer> results = new List<AnswerSmartOffer>();
            var MyQuery = PredicateBuilder.True<AnswerSmartOffer>();
            pageSize = pageSize == null ? 50 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            questionId = questionId == 0 ? null : questionId;
           

            MyQuery = MyQuery.And(p =>(questionId != null) && p.QuestionId == questionId );

            results = _context
                .AnswerSmartOffer
                .OrderBy(p => p.ShowNumber)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

    }
}
