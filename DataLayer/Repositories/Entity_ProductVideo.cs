using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DataLayer.Entities;
using System.Linq.Expressions;
using Binbin.Linq;
using DataLayer.ViewModels;

namespace DataLayer.Repositories
{
    public class Entity_ProductVideo : BaseRepository<ProductVideo>
    {
        private DatabaseEntities _context;
       
        public Entity_ProductVideo(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<ProductVideo> GetAllByProductId(int productId)
        {
            return _context.ProductVideo.Where(p =>
                p.ProductId == productId
            ).ToList();
        }
        public void DeleteByProductId(int productId)
        {
            List<ProductVideo> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
