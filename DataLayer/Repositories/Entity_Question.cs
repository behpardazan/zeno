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
    public class Entity_Question : BaseRepository<Question>
    {
        private DatabaseEntities _context;
        public Entity_Question(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<Question> Search( int index = 1, int pageSize = 10,bool ?IsPrivate=null,bool ? IsActive=null, string keyword = "",int? AccountId=null)
        {
            var MyQuery = GetSearchQuery(IsPrivate: IsPrivate, IsActive:IsActive, keyword: keyword,AccountId:AccountId);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
          
                return _context.Question.OrderByDescending(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }

        public int SearchCount(bool? IsPrivate = null, bool? IsActive = null, string keyword = "", int? AccountId = null)
        {
            var MyQuery = GetSearchQuery(IsPrivate: IsPrivate, IsActive: IsActive, keyword: keyword, AccountId: AccountId);
            return _context.Question.Count(MyQuery);
        }

        private Expression<Func<Question, bool>> GetSearchQuery(bool? IsPrivate = null, bool? IsActive = null, string keyword = "", int? AccountId = null)
        {
            var MyQuery = PredicateBuilder.True<Question>();
           
            if (IsPrivate != null)
                MyQuery = MyQuery.And(p => p.IsPrivate == IsPrivate);
            if (IsActive != null)
                MyQuery = MyQuery.And(p => p.IsActive == IsActive);
            if (AccountId != null)
                MyQuery = MyQuery.And(p => p.AccountId == AccountId);
            if (keyword != "")
                MyQuery = MyQuery.And(p => p.Answer.Contains(keyword)|| p.QuestionText.Contains(keyword));
            return MyQuery;
        }
    }
}
