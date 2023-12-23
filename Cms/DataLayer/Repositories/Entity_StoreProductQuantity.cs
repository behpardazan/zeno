using Binbin.Linq;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_StoreProductQuantity : BaseRepository<StoreProductQuantity>
    {
        private DatabaseEntities _context;
        public Entity_StoreProductQuantity(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void AddOrEdit(StoreProductQuantity storeProductQuantity, UnitOfWork unitOfWork)
        {
            UnitOfWork unit = new UnitOfWork();
            StoreProductQuantity spq = _context.StoreProductQuantity.FirstOrDefault(s => s.StoreProductId == storeProductQuantity.StoreProductId && s.ProductQuantityId == storeProductQuantity.ProductQuantityId);

            if (spq != null)
            {
                spq.Count = storeProductQuantity.Count;
                spq.Price = Base.BaseStore.GetFromPriceRange(storeProductQuantity:storeProductQuantity);
                spq.Warranty = storeProductQuantity.Warranty;
                Update(spq);
                Save();
            }
            else
            {
                spq = storeProductQuantity;
                Insert(spq);
                Save();
                spq = unit.StoreProductQuantity.FirstOrDefault(s => s.ID == spq.ID);
            }

            var discount = DataLayer.Base.BaseStore.GetDiscountPrice(quantity: spq,count:1);
            if (discount != spq.DiscountValue)
            {
                spq.DiscountValue = discount;
                Update(spq);
                Save();
            }
            Base.BaseStore.UpdateProductQuantity(spq.ProductQuantity.ProductId, unitOfWork);
        }
        public List<StoreProductQuantity> Search(int productId, int? colorId = null, int? sizeId = null, bool available = true)
        {
            var storeProductQuantities = _context.StoreProductQuantity.Where(s =>
            s.ProductQuantity.ProductId == productId
            && s.ProductQuantity.SizeId == sizeId
            && s.ProductQuantity.ColorId == colorId
            && s.StoreProduct.Code.Label == DataLayer.Enumarables.Enum_Code.STOREPRODUCT_REQUEST_APPROVED.ToString()
            && s.StoreProduct.Store.Active
            && s.StoreProduct.Store.Approve);
            return storeProductQuantities.ToList();
        }
        public List<StoreProductQuantity> GetForStore(int storeId)
        {
            var model = _context.StoreProductQuantity.Where(s => s.StoreProduct.StoreId == storeId).OrderBy(s => s.StoreProduct.Product.Name).ToList();
            return model;
        }
        public IQueryable<StoreProductQuantity> GetActives(int productId)
        {
            var model = _context.StoreProductQuantity.Where(s => s.StoreProduct.ProductId == productId && s.Count > 0 && s.Price > 0 && /*s.StoreProduct.Code.Label == DataLayer.Enumarables.Enum_Code.STOREPRODUCT_REQUEST_APPROVED.ToString() &&*/ s.StoreProduct.Store.Active && s.StoreProduct.Store.Approve);
            return model;
        }
    }
}
