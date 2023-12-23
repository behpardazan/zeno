using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductBrandLanguage : BaseRepository<ProductBrandLanguage>
    {
        private DatabaseEntities _context;
        public Entity_ProductBrandLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductBrandLanguage> GetAllByProductBrandId(int typeId)
        {
            return _context.ProductBrandLanguage.Where(p => p.ProductBrandId == typeId).ToList();
        }

        public void DeleteByProductBrandId(int typeId)
        {
            List<ProductBrandLanguage> list = GetAllByProductBrandId(typeId);
            
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
            Save();
        }
    }
}
