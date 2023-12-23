using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionProductType : BaseRepository<QuestionProductType>
    {
        private DatabaseEntities _context;
        public Entity_QuestionProductType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionProductType> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? ProductTypeId = null)
        {
            List<QuestionProductType> results = new List<QuestionProductType>();
            var MyQuery = PredicateBuilder.True<QuestionProductType>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
           
            if (ProductTypeId != null)
                MyQuery = MyQuery.And(p => p.ProductTypeId == ProductTypeId);

            results = _context
                .QuestionProductType
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<QuestionProductType> GetAllProductTypeId(int ProductTypeId)
        {
            return _context.QuestionProductType.Where(p => 
                p.ProductTypeId == ProductTypeId ||
                p.ProductTypeId == null
            )
            .OrderBy(p => p.ID)
            .ToList();
        }
    }
}
