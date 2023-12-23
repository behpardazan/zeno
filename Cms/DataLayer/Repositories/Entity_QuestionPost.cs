using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionPost : BaseRepository<QuestionPost>
    {
        private DatabaseEntities _context;
        public Entity_QuestionPost(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionPost> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? PostId = null)
        {
            List<QuestionPost> results = new List<QuestionPost>();
            var MyQuery = PredicateBuilder.True<QuestionPost>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
           
            if (PostId != null)
                MyQuery = MyQuery.And(p => p.PostId == PostId);

            results = _context
                .QuestionPost
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<QuestionPost> GetAllPostId(int PostId)
        {
            return _context.QuestionPost.Where(p => 
                p.PostId == PostId ||
                p.PostId == null
            )
            .OrderBy(p => p.ID)
            .ToList();
        }
    }
}
