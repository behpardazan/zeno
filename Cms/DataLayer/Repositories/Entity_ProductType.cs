﻿using Binbin.Linq;
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
    public class Entity_ProductType : BaseRepository<ProductType>
    {
        private DatabaseEntities _context;
        public Entity_ProductType(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public ProductType GetByCustomLabel(string customLabel)
        {
            return FirstOrDefault(p => p.CustomLabel.Trim().ToLower().Replace(" ", "-") == customLabel.Trim().ToLower().Replace(" ", "-"));
        }
        public ProductType GetBySyncId(string SyncId)
        {
            return _context.ProductType.FirstOrDefault(p => p.SyncId == SyncId);
        }

        public List<ProductType> Search(
            string Id = null,
            int? shopId = null,
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            string name = null,
            bool? isService = null,
            bool? isActive = null,
            string customLabel=null
            )
        {
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            var MyQuery = GetSearchQuery(Id: Id, shopId: shopId, notId: notId, name: name, isService: isService, isActive: isActive, customLabel: customLabel);
            List<ProductType> results = _context
                .ProductType
                .OrderBy(p => p.Priority).ThenBy(x => x.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();
            return results;
        }

        public List<ProductType> GetAllHasSyncId()
        {
            return _context.ProductType.Where(p =>
                p.SyncId != null &&
                p.SyncId != ""
            ).ToList();
        }

        public int SearchCount(string Id = null, int? shopId = null, int? notId = null, string name = null, bool? isService = null)
        {
            var MyQuery = GetSearchQuery(Id: Id, shopId: shopId, notId: notId, name: name, isService: isService);
            return _context.ProductType.Count(MyQuery);
        }

        public List<ProductType> GetAllByUserId(int UserId)
        {
            List<ShopUser> shopUsers = _context.ShopUser.Where(p =>
                p.UserId == UserId
            ).ToList();

            List<ProductType> list = new List<ProductType>();

            foreach (ShopUser shopUser in shopUsers)
            {
                foreach (ShopProductType item in shopUser.Shop.ShopProductType)
                {
                    if (list.Any(p => p.ID == item.ProductTypeId) == false)
                        list.Add(item.ProductType);
                }
            }

            return list;
        }

        private Expression<Func<ProductType, bool>> GetSearchQuery(string Id = null, int? shopId = null, int? notId = null, string name = null, bool? isService = null, bool? isActive = null , string customLabel = null)
        {
            var MyQuery = PredicateBuilder.True<ProductType>();

            if (Id != null)
            {
                var QueryStr = PredicateBuilder.False<ProductType>();
                string[] Split = Id.Split('-');
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
                MyQuery = MyQuery.And(p => p.ShopProductType.Any(s => s.ShopId == shopId));
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (name != null)
                MyQuery = MyQuery.And(p => p.Name.Contains(name));
            if (isService != null)
                MyQuery = MyQuery.And(p => p.IsService == isService);
            if (isActive != null)
                MyQuery = MyQuery.And(p => p.Active == isActive);
            if (customLabel != null)
                MyQuery = MyQuery.And(p => p.CustomLabel == customLabel);

            return MyQuery;
        }
        public List<ProductType> GetStoreProductTypes(int StoreId)
        {
            var model = _context.StoreProduct.Where(s => s.StoreId == StoreId && s.Product.ProductType != null).Select(s => s.Product.ProductType).Distinct().ToList();
            return model;
        }
    }
}
