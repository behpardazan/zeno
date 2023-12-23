using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using RestSharp;
using Newtonsoft.Json;
using Binbin.Linq;
using System.Linq.Expressions;

namespace DataLayer.Repositories
{
    public class Entity_ShippingSubscriptionPayment : BaseRepository<ShippingSubscriptionPayment>
    {
        private DatabaseEntities _context;
        public Entity_ShippingSubscriptionPayment(DatabaseEntities context) : base(context)
        {
            _context = context;
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
            return _context.ShippingSubscriptionPayment.Count(MyQuery);
        }

        public List<ShippingSubscriptionPayment> Search(
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

            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            List<ShippingSubscriptionPayment> results = _context
                .ShippingSubscriptionPayment
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }
        private Expression<Func<ShippingSubscriptionPayment, bool>> GetSearchQuery(
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
            List<ShippingSubscriptionPayment> results = new List<ShippingSubscriptionPayment>();
            var MyQuery = PredicateBuilder.True<ShippingSubscriptionPayment>();

            if (accountId != null)
                MyQuery = MyQuery.And(p => p.AccountId == accountId);

           
            if (string.IsNullOrEmpty(refId) == false)
            {
                refId = refId.ToEnglishDigit();
                MyQuery = MyQuery.And(p => p.RefNumber == refId);

            }
            if (status != Enum_Code.ORDER_STATUS_NONE)
            {

                MyQuery = MyQuery.And(p => p.Code.Label == status.ToString());
            }
            if (statusId != null && statusId.Any())
            {
                var QueryStr = PredicateBuilder.False<ShippingSubscriptionPayment>();
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
                var QueryStr = PredicateBuilder.False<ShippingSubscriptionPayment>();
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

           
            
            if (merchantId != null && merchantId != 0)
                MyQuery = MyQuery.And(p => p.MerchantId == merchantId);

           
            if (string.IsNullOrEmpty(customer) == false)
            {
                var nameQuery = PredicateBuilder.True<ShippingSubscriptionPayment>();
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
            
            return MyQuery;
        }

    }
}

