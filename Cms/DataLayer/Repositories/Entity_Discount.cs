using Binbin.Linq;
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
    public class Entity_Discount : BaseRepository<Discount>
    {
        private DatabaseEntities _context;
        public Entity_Discount(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Discount> GetAllByDiscountGroupId(int GroupId)
        {
            return _context.Discount.Where(p =>
                p.GroupId == GroupId
            ).ToList();
        }

        public List<Discount> GetAllByProduct(Product product)
        {
            DateTime now = DateTime.Now;
            string type_shop = Enum_Code.DISCOUNT_TYPE_SHOP.ToString();
            string type_producttype = Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString();
            string type_productcategory = Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString();
            string type_productsubcategory = Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString();
            string type_product = Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString();
            string type_store = Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString();

            return _context.Discount.Where(p =>
                ((p.Code.Label == type_product && p.ProductId== product.ID)||
                    (p.Code.Label == type_store && p.Product.StoreProduct.Any()) ||
                    (p.Code.Label == type_shop && p.ShopId==product.ShopId) ||
                    (p.Code.Label == type_producttype && p.ProductTypeId == product.ProductTypeId) ||
                    (p.Code.Label == type_productcategory && p.ProductCategoryId == product.ProductCategoryId) ||
                    (p.Code.Label == type_productsubcategory && p.ProductSubCategoryId == product.ProductSubCategoryId)
                ) &&
                (p.Active == true) && (p.Store.Active == true) && (p.Store.Approve == true) &&
                (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now))
            ).ToList();
        }
        public List<Discount> GetAllByStoreId(StoreProductQuantity storeProductQuantity)
        {
            DateTime now = DateTime.Now;
            string type_shop = Enum_Code.DISCOUNT_TYPE_SHOP.ToString();
            string type_producttype = Enum_Code.DISCOUNT_TYPE_PRODUCTTYPE.ToString();
            string type_productcategory = Enum_Code.DISCOUNT_TYPE_CATEGORY.ToString();
            string type_productsubcategory = Enum_Code.DISCOUNT_TYPE_SUBCATEGORY.ToString();
            string type_product = Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString();
            string type_varienty = Enum_Code.DISCOUNT_TYPE_VARIENTY.ToString();
            return _context.Discount.Where(p =>
            (p.StoreId == storeProductQuantity.StoreProduct.StoreId)
            &&
                (
                 (p.Code.Label == type_varienty && p.StoreProductQuantityId==storeProductQuantity.ID) ||
                 (p.Code.Label == type_product && p.ProductId == storeProductQuantity.StoreProduct.ProductId) ||
                    (p.Code.Label == type_producttype && p.ProductTypeId == storeProductQuantity.StoreProduct.Product.ProductTypeId) ||
                    (p.Code.Label == type_productcategory && p.ProductCategoryId == storeProductQuantity.StoreProduct.Product.ProductCategoryId) ||
                    (p.Code.Label == type_productsubcategory && p.ProductSubCategoryId == storeProductQuantity.StoreProduct.Product.ProductSubCategoryId) ||
                     (p.Code.Label == type_shop )

                    ) &&
                (p.Active == true) &&
                (p.StartDatetime == null || (p.StartDatetime != null && p.StartDatetime <= now)) &&
                (p.EndDatetime == null || (p.EndDatetime != null && p.EndDatetime >= now))
            ).ToList();
        }
        public List<Discount> GetAllByProductId(int productId)
        {
            string type_product = Enum_Code.DISCOUNT_TYPE_PRODUCT.ToString();
            return _context.Discount.Where(p =>
                p.ProductId == productId &&
                p.Code.Label == type_product
            ).ToList();
        }

        public List<Discount> Search(
            Enum_Code type = Enum_Code.DISCOUNT_TYPE_NONE,
            string label = null,
            int? notId = null,
            int? index = null,
            int? pageSize = null,
            Boolean? active = null,
            int? storeId = null)
        {
            index = index == null ? 1 : index;
            pageSize = pageSize == null ? 10 : pageSize;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            var MyQuery = GetSearchQuery(type: type, label: label, notId: notId, active: active, storeId: storeId);

            return _context
                .Discount
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
        }

        public int SearchCount(
            Enum_Code type = Enum_Code.DISCOUNT_TYPE_NONE,
            string label = null,
            int? notId = null,
            Boolean? active = null,
            int? storeId = null)
        {
            var MyQuery = GetSearchQuery(type: type, label: label, notId: notId, active: active, storeId: storeId);
            return _context.Discount.Count(MyQuery);
        }

        private Expression<Func<Discount, bool>> GetSearchQuery(
     Enum_Code type = Enum_Code.DISCOUNT_TYPE_NONE,
            string label = null,
            int? notId = null,
            Boolean? active = null,
            int? storeId = null)
        {
            var MyQuery = PredicateBuilder.True<Discount>();
            string typeString = type.ToString();

            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (label != null)
                MyQuery = MyQuery.And(p => p.DiscountGroup.Label == label);

            if (active != null)
            {
                MyQuery = MyQuery.And(p => p.DiscountGroup.Active == active);

            }
            if (type != Enum_Code.DISCOUNT_TYPE_NONE)
                MyQuery = MyQuery.And(p => p.Code.Label == typeString);

            if (storeId != null)
                MyQuery = MyQuery.And(p => p.StoreId == storeId);
            return MyQuery;
        }
        public void DeleteByProductId(int productId)
        {
            List<Discount> list = GetAllByProductId(productId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
