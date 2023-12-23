using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductCustomField : BaseRepository<ProductCustomField>
    {
        private DatabaseEntities _context;
        public Entity_ProductCustomField(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductCustomField> Search(int? typeId = null, int? pageSize = null, int? index = null, int? categoryId = null, int ?subcategoryId = null, int? brandId = null)
        {
            List<ProductCustomField> results = new List<ProductCustomField>();
            var MyQuery = PredicateBuilder.True<ProductCustomField>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            typeId = typeId == 0 ? null : typeId;
            brandId = brandId == 0 ? null : brandId;
            categoryId = categoryId == 0 ? null : categoryId;
            subcategoryId = subcategoryId == 0 ? null : subcategoryId;

            MyQuery = MyQuery.And(p => 

                ((typeId != null) && p.ProductTypeId == typeId) ||
                ((categoryId != null) && p.ProductCategoryId == categoryId) ||
                ((subcategoryId != null) && p.ProductSubCategoryId == subcategoryId) ||
                ((brandId != null) && p.ProductBrandId == brandId) || 

                (
                    p.ProductTypeId == null &&
                    p.ProductCategoryId == null &&
                    p.ProductSubCategoryId == null &&
                    p.ProductBrandId == null
                ));

            results = _context
                .ProductCustomField
                .OrderBy(p => p.ShowNumber)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<ProductCustomField> GetAllForSync()
        {
            return _context.ProductCustomField.Where(p =>
                p.SyncName != "" &&
                p.SyncName != null)
            .OrderBy(p => p.ShowNumber)
            .ToList();
        }
    }
}
