using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductOptionItem : BaseRepository<ProductOptionItem>
    {
        private DatabaseEntities _context;
        public Entity_ProductOptionItem(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<ProductOptionItem> Search(int productOptionId , int? pageSize = null, int? index = null)
        {
            List<ProductOptionItem> results = new List<ProductOptionItem>();
            var MyQuery = PredicateBuilder.True<ProductOptionItem>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            MyQuery = MyQuery.And(p =>p.OptionId==productOptionId);

            results = _context
                .ProductOptionItem                
                .Where(MyQuery)
                .OrderBy(p=>p.ID)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
        public List<ProductOptionItem> GetAllByProductId(int productId)
        {
            var list = _context.ProductOptionItem.Where(p => p.ProductOption.Product.Any(s => s.ID == productId)).ToList();
            return list;
        }
    }
}
