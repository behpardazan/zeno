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
    public class Entity_LadderPayment : BaseRepository<LadderPayment>
    {
        private DatabaseEntities _context;
        public Entity_LadderPayment(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public string CreateExternal(PaymentWebsite paymentWebsite, string merchant, int amount, string currency, string orderId, string sign)
        {
            var merchantId = _context.Merchant.FirstOrDefault(s => s.Bank.Label.ToLower() == merchant.ToLower());
            if (merchantId == null)
            {
                return null;
            }
            var currencyId = _context.Code.FirstOrDefault(s => s.Label.ToLower() == currency.ToLower());
            if (currencyId == null)
            {
                return null;
            }
            Code statusEntity = _context.Code.FirstOrDefault(p => p.Label == "PAYMENT_STATUS_INSERTED");
            PaymentSubject paymentSubject = _context.PaymentSubject.FirstOrDefault(s => s.Label == "EXTERNAL");
            var payment = new LadderPayment()
            {
                Datetime = DateTime.Now,
                Amount = amount,
                CurrencyTypeId = currencyId.ID,
                MerchantId = merchantId.ID,
                PaymentWebsiteId = paymentWebsite.ID,
                //SubjectId = paymentSubject.ID,
                IpAddress = paymentWebsite.Ip,
                StatusId = statusEntity.ID,
                ExternalInfo5 = Guid.NewGuid().ToString(),
                ExternalInfo6 = orderId,
                ExternalInfo7 = sign,
            };
            Insert(payment);
            return payment.ExternalInfo5;
        }
        public LadderPayment GetByTokenAndSign(string token, string sign)
        {
            return _context.LadderPayment.FirstOrDefault(s => s.ExternalInfo5 == token && s.ExternalInfo7 == sign);

        }
        public LadderPayment GetByReferenceNumber(string refNumber)
        {
            return _context.LadderPayment.FirstOrDefault(p => p.RefNumber == refNumber);
        }
        public void DoLadderPaymentServices(UnitOfWork mainContext, LadderPayment payment)
        {
            var laddersetting = _context.LadderSetting.Where(s => s.ID == payment.LadderSetting.ID).FirstOrDefault();
            try
            {

                StringBuilder str_1 = new StringBuilder();
                StringBuilder str = new StringBuilder();

                string token_1 = laddersetting.Name.ToString() /*orderId.ToPersian()*/;
                string token_2 = laddersetting.LadderDays.ToString();
                string token_3 = "";
                str.AppendLine("آگهی شما در سایت نردبان شد");
                mainContext.Sms.SaveNewSms(payment.Account.Mobile, Enum_SmsType.PAYMENT_SUCCESSLADDER, str.ToString(), token_1, token_2, token_3);
                Save();
            }
            catch (Exception) { }

            try
            {

                StringBuilder str_2 = new StringBuilder();
                string token_1 = laddersetting.LadderDays.ToString()/*orderId.ToPersian()*/;
                string token_2 = laddersetting.LadderPrice.GetCurrencyFormat().ToPersian();
                string token_3 = "";
                str_2.AppendLine(" ارتقای جدید آگهی" + token_1 + /*" مبلغ " + token_2 +*/ " در سایت ثبت شد");
                List<SiteUser> listUser = mainContext.SiteUser.GetAllByUserRole(Enum_UserRole.ADMIN);
                foreach (SiteUser item in listUser.Where(p => p.Mobile != null))
                {
                    //if (shop.ShopUser.Any(p => p.UserId == item.ID))
                    //{
                    mainContext.Sms.SaveNewSms(item.Mobile, Enum_SmsType.PAYMENT_SUCCESS_ADMINLADDER, str_2.ToString(), token_1, token_2, token_3);
                    //}
                }
            }
            catch (Exception) { }


        }

        public LadderPayment CreatePayment(int? accountId, double price, int merchantId, int LadderSettingId,
            string description = null, string payment_status = "PAYMENT_STATUS_STARTED", bool? isMobile = false, int? productId = null)
        {
            Code statusEntity = _context.Code.FirstOrDefault(p => p.Label == payment_status);
            LadderPayment payment = new LadderPayment()
            {
                LedderSettingId = LadderSettingId,
                StatusId = statusEntity.ID,
                ProductId = productId,
                //AccountId=accountId,
                MerchantId = merchantId,
                //SubjectId = subjectId,
                Amount = price,
                IpAddress = BaseSecurity.GetClientIPAddress(),
                Datetime = DateTime.Now,
                Description = description,
                IsMoblie = isMobile
            };

            if (accountId != null)
                payment.AccountId = accountId.GetValueOrDefault();
            //if (userId != null)
            //    payment.SiteUserId = userId.GetValueOrDefault();
            Insert(payment);
            Save();
            return payment;
        }

        public int SearchCount(
              int? accountId = null,
              int? merchantId = null,
              string refId = null,
              int[] statusId = null,
              List<string> statusStr = null,
              int? fromorderid = null,
              int? toorderid = null,
              DateTime? fromDatetime = null,
              DateTime? toDatetime = null,
              bool? clear = null
             )
        {
            var MyQuery = GetSearchQuery(
                accountId: accountId,
                merchantId: merchantId,
                fromDatetime: fromDatetime,
                fromorderid: fromorderid,
                refId: refId,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDatetime,
                toorderid: toorderid,
                clear: clear
             );
            return _context.LadderPayment.Count(MyQuery);
        }

        public List<LadderPayment> Search(
            int? index = 1,
            int? pageSize = null,
            int? accountId = null,

            int? merchantId = null,
            string refId = null,
            Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
            int[] statusId = null,
            List<string> statusStr = null,
            int? fromorderid = null,
            int? toorderid = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,

            bool? clear = null
            )
        {
            var MyQuery = GetSearchQuery(
                accountId: accountId,
                merchantId: merchantId,
                fromDatetime: fromDatetime,
                fromorderid: fromorderid,
                refId: refId,
                status: status,
                statusId: statusId,
                statusStr: statusStr,
                toDatetime: toDatetime,
                toorderid: toorderid,
                clear: clear
                );

            pageSize = pageSize == null ? 10 : pageSize;
            index = index == null ? 1 : index;
            int skipValue = pageSize.Value * (index.Value - 1);
            int pageValue = pageSize.Value;

            List<LadderPayment> results = _context
                .LadderPayment
                .OrderByDescending(p => p.ID)
                .Where(MyQuery)
                .Skip(skipValue)
                .Take(pageValue)
                .ToList();

            return results;
        }


        private Expression<Func<LadderPayment, bool>> GetSearchQuery(
            int? accountId = null,
            string refId = null,
            Enum_Code status = Enum_Code.ORDER_STATUS_NONE,
            int[] statusId = null,
            List<string> statusStr = null,
            int? fromorderid = null,
            int? toorderid = null,
            int? merchantId = null,
            DateTime? fromDatetime = null,
            DateTime? toDatetime = null,
            bool? clear = null
            )
        {

            List<LadderPayment> results = new List<LadderPayment>();
            var MyQuery = PredicateBuilder.True<LadderPayment>();

            if (accountId != null)
                MyQuery = MyQuery.And(p => p.AccountId == accountId);


            if (string.IsNullOrEmpty(refId) == false)
                MyQuery = MyQuery.And(p => p.RefNumber == refId);
            if (status != Enum_Code.ORDER_STATUS_NONE)
            {

                MyQuery = MyQuery.And(p => p.Code.Label == status.ToString());
            }
            if (statusId != null && statusId.Any())
            {
                var QueryStr = PredicateBuilder.False<LadderPayment>();
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
                var QueryStr = PredicateBuilder.False<LadderPayment>();
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

