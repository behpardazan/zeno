﻿using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopReseller : BaseRepository<ShopReseller>
    {
        private DatabaseEntities _context;
        public Entity_ShopReseller(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public ShopReseller GetByUserId(int userId)
        {
            return _context.ShopReseller.FirstOrDefault(p => 
                p.UserId == userId
            );
        }

        public List<ShopReseller> Search(
            int? notId = null,
            int? index = 1,
            int? pageSize = null,
            int? shopId = null,
            string name = null)
        {
            List<ShopReseller> results = new List<ShopReseller>();
            var MyQuery = PredicateBuilder.True<ShopReseller>();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;
            
            if (notId != null)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (string.IsNullOrEmpty(name) == false)
            {
                if (name.StartsWith("-"))
                {
                    name = name.Substring(1);
                    MyQuery = MyQuery.And(p => p.Name.StartsWith(name));
                }
                else
                    MyQuery = MyQuery.And(p => p.Name.Contains(name));
            }
            if (shopId != null)
                MyQuery = MyQuery.And(p => p.ShopId == shopId);

            results = _context
                .ShopReseller
                .OrderBy(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }

        public List<ViewReportReseller> GetForReport()
        {
            List<ViewReportReseller> query = _context.AccountOrderProduct
                    .Where(p =>
                        (
                            p.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() ||
                            p.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_PROCESS.ToString() ||
                            p.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_STORE.ToString() ||
                            p.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_POST.ToString()
                        ) &&
                            p.ResellerId != null
                    )
                    .GroupBy(p => p.ShopReseller.Name)
                    .Select(g =>
                        new ViewReportReseller()
                        {
                            Name = g.Key,
                            ProductCount = g.Sum(q => q.Count),
                            SumPrice = g.Sum(q => q.Price),
                            BenefitPercent = 10,
                            AmountOfDept = (g.Sum(q => q.Price) * 10) / 100,
                            SumCheckOut = 0,
                        })
                    .OrderByDescending(p => p.SumPrice)
                    .ToList();
            return query;
        }
    }
}
