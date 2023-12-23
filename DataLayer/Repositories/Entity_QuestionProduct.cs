using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionProduct : BaseRepository<QuestionProduct>
    {
        private DatabaseEntities _context;
        public Entity_QuestionProduct(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionProduct> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? productId = null)
        {
            List<QuestionProduct> results = new List<QuestionProduct>();
            var MyQuery = PredicateBuilder.True<QuestionProduct>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
           
            if (productId != null)
                MyQuery = MyQuery.And(p => p.ProductId == productId);

            results = _context
                .QuestionProduct
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<QuestionProduct> GetAllProductId(int ProductId)
        {
            return _context.QuestionProduct.Where(p => 
                p.ProductId == ProductId ||
                p.ProductId == null
            )
            .OrderBy(p => p.ID)
            .ToList();
        }
    }
}
