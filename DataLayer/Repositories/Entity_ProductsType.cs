using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductsType : BaseRepository<ProductsType>
    {
        private DatabaseEntities _context;
        public Entity_ProductsType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<ProductsType> GetAllByProductId(int productId)
        {
            return _context.ProductsType.Where(p =>
                p.ProductId == productId
            ).ToList();
        }

        public void DeleteByProductId(int productId)
        {
            List<ProductsType> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
