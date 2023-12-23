using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionProductCategory : BaseRepository<QuestionProductCategory>
    {
        private DatabaseEntities _context;
        public Entity_QuestionProductCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionProductCategory> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? CategoryId = null)
        {
            List<QuestionProductCategory> results = new List<QuestionProductCategory>();
            var MyQuery = PredicateBuilder.True<QuestionProductCategory>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
           
            if (CategoryId != null)
                MyQuery = MyQuery.And(p => p.CategoryId == CategoryId);

            results = _context
                .QuestionProductCategory
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<QuestionProductCategory> GetAllCategoryId(int CategoryId)
        {
            return _context.QuestionProductCategory.Where(p => 
                p.CategoryId == CategoryId ||
                p.CategoryId == null
            )
            .OrderBy(p => p.ID)
            .ToList();
        }
    }
}
