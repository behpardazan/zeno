using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductsSubCategory : BaseRepository<ProductsSubCategory>
    {
        private DatabaseEntities _context;
        public Entity_ProductsSubCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<ProductsSubCategory> GetAllByProductId(int productId)
        {
            return _context.ProductsSubCategory.Where(p =>
                p.ProductId == productId
            ).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductsSubCategory> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
