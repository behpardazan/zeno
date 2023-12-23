using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_SurveyAnswer : BaseRepository<SurveyAnswer>
    {
        private DatabaseEntities _context;
        public Entity_SurveyAnswer(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<SurveyAnswer> Search(
            int? surveyId=null,
            int? pageSize = 10,
            int? index = 1,
            string name = null)
        {
            var MyQuery = PredicateBuilder.True<SurveyAnswer>();
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            if (!string.IsNullOrEmpty(name))
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (surveyId != null)
                MyQuery = MyQuery.And(p => p.SurveyId == surveyId);

            var a = _context
                 .SurveyAnswer
                 .Where(MyQuery)
                 .OrderByDescending(p => p.ID)
                 .Skip(skipValue)
                 .Take(pageValue)
                 .ToList();
            return a;
        }

    }
}
