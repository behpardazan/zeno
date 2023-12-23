using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ProductSubCategory : BaseRepository<ProductSubCategory>
    {
        private DatabaseEntities _context;
        public Entity_ProductSubCategory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<ProductSubCategory> GetAllHasSyncId()
        {
            return _context.ProductSubCategory.Where(p =>
                p.SyncId != null &&
                p.SyncId != ""
            ).ToList();
        }

        public ProductSubCategory GetBySyncId(string SyncId)
        {
            return _context.ProductSubCategory.FirstOrDefault(p => p.SyncId == SyncId);
        }

        public List<ProductSubCategory> Search(string Id = null, int? notId = 0, string typeId = null, string categoryId = null, int? pageSize = 10, int? index = 1, string name = null)
        {
            var MyQuery = GetSearchQuery(Id: Id, notId: notId, typeId: typeId, categoryId: categoryId, name: name);
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            
            return _context
                .ProductSubCategory
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }

        public int SearchCount(string pbId = null, int? notId = 0, string typeId = null, string categoryId = null, string name = null)
        {
            var MyQuery = GetSearchQuery(Id: pbId, notId: notId, typeId: typeId, categoryId: categoryId, name: name);
            return _context.ProductSubCategory.Count(MyQuery);
        }

        private Expression<Func<ProductSubCategory, bool>> GetSearchQuery(string Id = null, int? notId = 0, string typeId = null, string categoryId = null, string name = null)
        {
            var MyQuery = PredicateBuilder.True<ProductSubCategory>();
            if (Id != null && Id != "0")
            {
                var QueryStr = PredicateBuilder.False<ProductSubCategory>();
                foreach (int item in SplitIds(Id))
                {
                    QueryStr = QueryStr.Or(p => p.ID == item);
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (typeId != null && typeId != "0")
            {
                var QueryStr = PredicateBuilder.False<ProductSubCategory>();
                foreach (int item in SplitIds(typeId))
                {
                    QueryStr = QueryStr.Or(p => p.ProductCategory.TypeId == item);
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (categoryId != null && categoryId != "0")
            {
                var QueryStr = PredicateBuilder.False<ProductSubCategory>();
                foreach (int item in SplitIds(categoryId))
                {
                    QueryStr = QueryStr.Or(p => p.CategoryId == item);
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            MyQuery.And(p => p.Deleted != true);

            return MyQuery;
        }
        public List<ProductSubCategory> GetStoreProductSubCategories(int StoreId)
        {
            var model = _context.StoreProduct.Where(s => s.StoreId == StoreId && s.Product.ProductSubCategory != null).Select(s => s.Product.ProductSubCategory).Distinct().ToList();
            return model;
        }
        
    }
}
