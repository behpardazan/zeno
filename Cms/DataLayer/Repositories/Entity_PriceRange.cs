using Binbin.Linq;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_PriceRange : BaseRepository<PriceRange>
    {
        DatabaseEntities _context;
        UnitOfWork _Unitcontext = new UnitOfWork();


        public Entity_PriceRange(DatabaseEntities context) : base(context)
        {
            _context = context;
            _context = context;

        }
        public bool AddRange(PriceRange priceRange)
        {
            var list = Where(p => p.StoreProductQuantityId == priceRange.StoreProductQuantityId);
            if (list.Any(s => s.StartCount <= priceRange.StartCount && (!s.EndCount.HasValue) && s.ID != priceRange.ID))
            {
                return false;
            }     
            if (list.Any(s => s.StartCount <= priceRange.StartCount && s.EndCount >= priceRange.StartCount && s.ID!=priceRange.ID))
            {
                return false;
            }     
            if (list.Any(s => s.StartCount > priceRange.StartCount &&  (!priceRange.EndCount.HasValue) && s.ID != priceRange.ID))
            {
                return false;
            }      
            if (list.Any(s => s.StartCount > priceRange.StartCount && s.StartCount <=  priceRange.EndCount && s.ID != priceRange.ID) )
            {
                return false;
            }
            if(priceRange.ID == 0)
            {
                Insert(priceRange);
                Save();
            }
            else
            {
                var range = _Unitcontext.PriceRange.GetById(priceRange.ID);
                range.ID = priceRange.ID;
                range.StartCount = priceRange.StartCount;
                range.EndCount = priceRange.EndCount;
                range.Price = priceRange.Price;
                _Unitcontext.PriceRange.Update(range);
                _Unitcontext.Save();
            }
           
            
            return true;
        }
        public List<PriceRange> GetByStoreproductQuantityId(int id)
        {
            return Where(s => s.StoreProductQuantityId == id).ToList();
        }
    }
}
