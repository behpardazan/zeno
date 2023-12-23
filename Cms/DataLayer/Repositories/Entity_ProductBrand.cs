using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductBrand : BaseRepository<ProductBrand>
    {
        private DatabaseEntities _context;
        public Entity_ProductBrand(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public ProductBrand GetBySyncId(string SyncId)
        {
            return _context.ProductBrand.FirstOrDefault(p => p.SyncId == SyncId);
        }

        public ProductBrand GetByLabel(string label)
        {

            _context.Product.Include("ProductBrand").Where(x => x.ProductCategoryId == 12).ToList();
            return _context.ProductBrand.FirstOrDefault(p => p.Label == label);

        }

        public List<ProductBrand> GetAllHasSyncId()
        {

            return _context.ProductBrand.Where(p =>
                p.SyncId != null &&
                p.SyncId != ""
            ).ToList();
        }

        public List<ProductBrand> Search(
            string pbId = null, 
            int? notId = null, 
            int? shopId = null, 
            int? typeId = null, 
            int? categoryId = null, 
            int? subcategoryId = null, 
            int index = 1, int pageSize = 10, string name = null, int? userId = null, Enum_ProductBrandOrder order = Enum_ProductBrandOrder.NONE)
        {
            var MyQuery = GetSearchQuery(pbId: pbId, notId: notId, shopId: shopId, typeId: typeId, categoryId: categoryId, subcategoryId: subcategoryId, name: name, userId: userId);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            List<ProductBrand> results = new List<ProductBrand>();
            if (order == Enum_ProductBrandOrder.MORE_PRODUCT)
            {
                results = _context
                    .ProductBrand
                    .OrderByDescending(p => p.Product.Count)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else if (order == Enum_ProductBrandOrder.NAME)
            {
                results = _context
                    .ProductBrand
                    .OrderBy(p => p.Name)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }
            else
            {
                results = _context
                    .ProductBrand
                    .OrderBy(p => p.ID)
                    .Where(MyQuery)
                    .Skip(skipValue)
                    .Take(pageValue)
                    .ToList();
            }

            return results;
        }

        public int SearchCount(
            string pbId = null, 
            int? notId = null, 
            int? shopId = null, 
            int? typeId = null, 
            int? categoryId = null,
            int? subcategoryId = null, 
            string name = null, 
            int? userId = null)
        {
            var MyQuery = GetSearchQuery(pbId: pbId, notId: notId, shopId: shopId, typeId: typeId, categoryId: categoryId, subcategoryId: subcategoryId, name: name, userId: userId);
            return _context.ProductBrand.Count(MyQuery);
        }

        private Expression<Func<ProductBrand, bool>> GetSearchQuery(
            string pbId = null, 
            int? notId = null, 
            int? shopId = null, 
            int? typeId = null,
            int? categoryId = null,
            int? subcategoryId = null,
            string name = null, 
            int? userId = null)
        {
            var MyQuery = PredicateBuilder.True<ProductBrand>();
            if (pbId != null)
            {
                var QueryStr = PredicateBuilder.False<ProductBrand>();
                string[] Split = pbId.Split('-');
                foreach (var item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.ID == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (shopId != null)
                MyQuery = MyQuery.And(p => p.ProductType.ShopProductType.Any(s => s.ShopId == shopId));
            if (userId != null)
                MyQuery = MyQuery.And(p => (p.ProductBrandUser.Any(s => s.UserId == userId) && p.IsPublic == false) || p.IsPublic == true);
            if (typeId != null)
                MyQuery = MyQuery.And(p => p.ProductTypeId == typeId || p.ProductTypeBrand.Any(q => q.ProductTypeId == typeId));
            if (categoryId != null)
                MyQuery = MyQuery.And(p => p.Product.Any(s => s.ProductCategoryId == categoryId));
            if (subcategoryId != null)
                MyQuery = MyQuery.And(p => p.Product.Any(s => s.ProductSubCategoryId == subcategoryId));
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            return MyQuery;
        }
    }
}
