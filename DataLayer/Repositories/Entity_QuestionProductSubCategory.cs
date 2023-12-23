using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionProductSubCategory : BaseRepository<QuestionProductSubCategory>
    {
        private DatabaseEntities _context;
        public Entity_QuestionProductSubCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionProductSubCategory> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? SubCategoryId = null)
        {
            List<QuestionProductSubCategory> results = new List<QuestionProductSubCategory>();
            var MyQuery = PredicateBuilder.True<QuestionProductSubCategory>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
           
            if (SubCategoryId != null)
                MyQuery = MyQuery.And(p => p.SubCategoryId == SubCategoryId);

            results = _context
                .QuestionProductSubCategory
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<QuestionProductSubCategory> GetAllSubCategoryId(int SubCategoryId)
        {
            return _context.QuestionProductSubCategory.Where(p => 
                p.SubCategoryId == SubCategoryId ||
                p.SubCategoryId == null
            )
            .OrderBy(p => p.ID)
            .ToList();
        }
    }
}
