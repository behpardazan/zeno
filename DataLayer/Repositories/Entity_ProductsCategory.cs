using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductsCategory : BaseRepository<ProductsCategory>
    {
        private DatabaseEntities _context;
        public Entity_ProductsCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<ProductsCategory> GetAllByProductId(int productId)
        {
            return _context.ProductsCategory.Where(p =>
                p.ProductId == productId
            ).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductsCategory> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
