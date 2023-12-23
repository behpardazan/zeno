using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountOrder : BaseRepository<AccountOrder>
    {
        private DatabaseEntities _context;
        public Entity_AccountOrder(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void updateorder()
        {
            var orders = Where(s => s.ProductsPrice == 0 && s.ParentId == null).ToList();
            foreach (var item in orders)
            {
                double sum = 0;
                foreach (var o in item.AccountOrder1.ToList())
                {
                    if (o.SendPrice.HasValue)
                        o.ProductsPrice = o.Price - o.SendPrice.Value;
                    else
                        o.ProductsPrice = o.Price;

                    sum += o.ProductsPrice;
                }
                Save();
                item.ProductsPrice = sum;
                Save();
            }
        }
        public override void Insert(AccountOrder tEntity)
        {
            if (tEntity.StoreId != null)
            {
                var store = _context.Store.Find(tEntity.StoreId);
                tEntity.BenefitPercent = store.BenefitPercent;
            }

            _context.AccountOrder.Add(tEntity);
        }


        public int SearchCount(
            int? accountId = null,
            int? storeId = null,
            int[] stores = null,
            int? merchantId = null,
            string refId = null,
            int[] statusId = null,
            List<string> statusStr = null,
            string customer = null,
            string product = null,
            int? fromorderid = null,
            int? toorderid = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,
            string rebate = null,
            bool? clear = null,
            string IsOne = null,
            bool isParent = false
           )
        {
            var MyQuery = GetSearchQuery(
                accountId: accountId,
                storeId: storeId,
                stores: stores,
                merchantId: merchantId,
                customer: customer,
                fromDatetime: fromDatetime,
                fromorderid: fromorderid,
                rebate: rebate,
                refId: refId,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDatetime,
                toorderid: toorderid,
                clear: clear,
                IsOne: IsOne,
                product: product,
                isParent: isParent
             );
            return _context.AccountOrder.Count(MyQuery);
        }

        public List<AccountOrder> Search(
            int? index = 1,
            int? pageSize = null,
            int? accountId = null,
            int? storeId = null,
            int[] stores = null,
            int? merchantId = null,
            string refId = null,
            Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
            int[] statusId = null,
            List<string> statusStr = null,
            string customer = null,
            string product = null,
            int? fromorderid = null,
            int? toorderid = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,
            string rebate = null,
            bool? clear = null,
            string IsOne = null,
            bool isParent = false

            )
        {
            var MyQuery = GetSearchQuery(
                accountId: accountId,
                storeId: storeId,
                stores: stores,
                merchantId: merchantId,
                customer: customer,
                fromDatetime: fromDatetime,
                fromorderid: fromorderid,
                rebate: rebate,
                refId: refId,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDatetime,
                toorderid: toorderid,
                clear: clear,
                IsOne: IsOne,
                product: product,
                status: status,
                isParent: isParent
                );

            pageSize = pageSize == null ? 100000 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            List<AccountOrder> results = _context
                .AccountOrder
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
        public ViewShowAccountOrder Search2(
            int? index = 1,
            int? pageSize = null,
            int? accountId = null,
            int? storeId = null,
            int[] stores = null,
            int? merchantId = null,
            string refId = null,
            Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
            int[] statusId = null,
            List<string> statusStr = null,
            string customer = null,
            string product = null,
            int? fromorderid = null,
            int? toorderid = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,
            string rebate = null,
            bool? clear = null,
            string IsOne = null,
            bool isParent = true

            )
        {
            if (stores != null && stores.Any())
            {
                isParent = false;
            }
            else
            {
                isParent = true;

            }

            var MyQuery = GetSearchQuery(
                accountId: accountId,
                storeId: storeId,
                stores: stores,
                merchantId: merchantId,
                customer: customer,
                fromDatetime: fromDatetime,
                fromorderid: fromorderid,
                rebate: rebate,
                refId: refId,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDatetime,
                toorderid: toorderid,
                clear: clear,
                IsOne: IsOne,
                product: product,
                status: status,
                isParent: isParent
                );
            var result = new ViewShowAccountOrder();
            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            IEnumerable<AccountOrder> orders = _context
                .AccountOrder

                .Where(MyQuery);
            result.TotalCount = orders.Count();
            result.TotalBasePrice = orders.Sum(s => s.ProductsPrice);
            result.TotalPrice = orders.Sum(s => s.Price);
            result.TotalShippingPrice = orders.Where(s => s.SendPrice.HasValue).Sum(s => s.SendPrice.Value);
            result.TotalDiscountPrice = orders.Where(s => s.DiscountPrice.HasValue).Sum(s => s.DiscountPrice.Value);
            result.TotalRebatePrice = orders.Where(s => s.RebatePrice.HasValue).Sum(s => s.RebatePrice.Value);
            result.TotalSendPrice = orders.Where(s => s.SendPrice.HasValue).Sum(s => s.SendPrice.Value);

            result.IsParent = isParent;
            result.List = orders.OrderByDescending(p => p.ID).Skip(skipValue)
            .Take(pageValue)
            .ToList().ToApi();
            return result;
        }
        public IQueryable<AccountOrder> Report(

            int? accountId = null,
            int? storeId = null,
            int[] stores = null,
            int? merchantId = null,
            string refId = null,
            int[] statusId = null,
            List<string> statusStr = null,
            string customer = null,
            string product = null,
            int? fromorderid = null,
            int? toorderid = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,
            string rebate = null,
            bool? clear = null
            )
        {
            var MyQuery = GetSearchQuery(
                accountId: accountId,
                storeId: storeId,
                stores: stores,
                merchantId: merchantId,
                customer: customer,
                fromDatetime: fromDatetime,
                fromorderid: fromorderid,
                product: product,
                rebate: rebate,
                refId: refId,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDatetime,
                toorderid: toorderid,
                clear: clear

                );

            var results = _context
                .AccountOrder
                .Where(MyQuery);

            return results;
        }

        private Expression<Func<AccountOrder, bool>> GetSearchQuery(
            int? accountId = null,
            int? storeId = null,
            int[] stores = null,
            string refId = null,
            Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
            int[] statusId = null,
            List<string> statusStr = null,
            string customer = null,
            string product = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,
            string rebate = null,
            bool? clear = null,
            string IsOne = null,
            bool isParent = false
            )
        {
            List<AccountOrder> results = new List<AccountOrder>();
            var MyQuery = PredicateBuilder.True<AccountOrder>();

            if (accountId != null)
                MyQuery = MyQuery.And(p => p.AccountId == accountId);

            if (storeId != null)
                MyQuery = MyQuery.And(p => p.StoreId == storeId);
            if (isParent == false)
            {
                MyQuery = MyQuery.And(p => p.ParentId != null);
            }
            else
            {
                MyQuery = MyQuery.And(p => p.ParentId == null);
            }

            if (stores != null && stores.Any())
            {
                var QueryStr = PredicateBuilder.False<AccountOrder>();
                foreach (var item in stores)
                {
                    int itemId = item;
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.StoreId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }
            if (string.IsNullOrEmpty(refId) == false)
            {
                refId = refId.ToEnglishDigit();
                MyQuery = MyQuery.And(p => p.AccountOrder2.PaymentProductOrder.Any(s => s.Payment.RefNumber == refId));

            }
            if (status != Enum_Code.ORDER_STATUS_NONE)
            {

                MyQuery = MyQuery.And(p => p.Code.Label == status.ToString());
            }
            if (statusId != null && statusId.Any())
            {
                var QueryStr = PredicateBuilder.False<AccountOrder>();
                foreach (var item in statusId)
                {
                    int itemId = item;
                    if (itemId != 0)
                    {
                        QueryStr = QueryStr.Or(p =>
                            p.StatusId == itemId
                        );
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (statusStr != null)
            {
                var QueryStr = PredicateBuilder.False<AccountOrder>();
                foreach (var item in statusStr)
                {
                    if (string.IsNullOrEmpty(item) == false)
                    {
                        QueryStr = QueryStr.Or(p => p.Code.Label == item);
                    }
                }
                MyQuery = MyQuery.And(QueryStr);
            }

            if (fromorderid != null)
                MyQuery = MyQuery.And(p => p.ID >= fromorderid);

            if (toorderid != null)
                MyQuery = MyQuery.And(p => p.ID <= toorderid);

            if (rebate != null)
                MyQuery = MyQuery.And(p => p.Rebate.CodeValue == rebate);
            if (!string.IsNullOrEmpty(IsOne))
                MyQuery = MyQuery.And(p => p.Account.AccountOrder.Count == 2);

            if (merchantId != null && merchantId != 0)
                MyQuery = MyQuery.And(p => p.PaymentProductOrder.Any(q => q.Payment.MerchantId == merchantId));

            if (string.IsNullOrEmpty(product) == false)
                MyQuery = MyQuery.And(p =>
                    p.AccountOrderProduct.Any(q =>
                        q.Product.Name.Contains(product) ||
                        q.Product.Summary.Contains(product) ||
                        q.Product.ProductType.Name.Contains(product) ||
                        q.Product.ProductCategory.Name.Contains(product) ||
                        q.Product.ProductSubCategory.Name.Contains(product)));

            if (string.IsNullOrEmpty(customer) == false)
            {
                var nameQuery = PredicateBuilder.True<AccountOrder>();
                string[] nameSplit = customer.Split(' ');
                foreach (var item in nameSplit)
                {
                    nameQuery = nameQuery.And(p =>
                        p.Account.FullName.Contains(item) ||
                        p.Account.Mobile.Contains(item) ||
                        p.Account.Email.Contains(item) ||
                        p.Account.NationalCode.Contains(item) ||
                        p.AccountAddress.NameFamily.Contains(item) ||
                        p.AccountAddress.City.Name.Contains(item) ||
                        p.AccountAddress.City.State.Name.Contains(item) ||
                        p.AccountAddress.AddressValue.Contains(item) ||
                        p.AccountAddress.Mobile.Contains(item) ||
                        p.AccountAddress.Phone.Contains(item)
                    );
                }
                MyQuery = MyQuery.And(nameQuery);
            }

            if (fromDatetime != null && fromDatetime != default(DateTime))
                MyQuery = MyQuery.And(p => p.Datetime >= fromDatetime);

            if (toDatetime != null && toDatetime != default(DateTime))
            {
                DateTime toDatetimeTemp = toDatetime.Value.AddDays(1);
                MyQuery = MyQuery.And(p => p.Datetime <= toDatetimeTemp);
            }
            if (clear != null)
            {
                if (clear.Value)
                    MyQuery = MyQuery.And(p => p.ClearingId != null);
                MyQuery = MyQuery.And(p => p.ClearingId == null);


            }
            return MyQuery;
        }

        public void ChangeStatus(int orderId, UnitOfWork _unitOfWork)
        {
            var orderAccount = _context.AccountOrder.Find(orderId);
            switch (orderAccount.Code.Label)
            {
                case nameof(Enumarables.Enum_Code.ORDER_STATUS_SUCCESS):
                    {
                        orderAccount.StatusId = _unitOfWork.Code.GetIdByLabel(Enumarables.Enum_Code.ORDER_STATUS_POST);
                        break;
                    }
                    //case nameof(Enumarables.Enum_Code.ORDER_STATUS_PROCESS):
                    //    {
                    //        orderAccount.StatusId = _unitOfWork.Code.GetIdByLabel(Enumarables.Enum_Code.ORDER_STATUS_POST);
                    //        break;
                    //    }
            }
            Save();
        }
        public void UpdateClearing(List<AccountOrder> accountOrder, int clearingId)
        {
            foreach (var item in accountOrder)
            {
                item.ClearingId = clearingId;
                _context.Entry(item).State = EntityState.Modified;
            }
            Save();
        }
        public double GetOrderPrice(int storeId)
        {
            List<string> orderStatus = new List<string>() { nameof(Enum_Code.ORDER_STATUS_POST) };
            var orders = Search(pageSize: 2000, storeId: storeId, statusStr: orderStatus, clear: false);
            double orderPrice = orders.Sum(s => s.Price) - orders.Sum(s => (s.Price - s.SendPrice.Value) * (((double)s.BenefitPercent.Value) / 100));
            return orderPrice;
        }
    }
}
