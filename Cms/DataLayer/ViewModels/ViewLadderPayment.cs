using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.ViewModels
{
    public class ViewLadderPayment
    {
        public int Id { get; set; }
        public int  LadderSettingId { get; set; }
        public ViewAccount Account { get; set; }
        public ViewMerchant Merchant { get; set; }
        public ViewPaymentSubject PaymentSubject { get; set; }
        public ViewCode Status { get; set; }
        public double Price { get; set; }
        public DateTime ? Date { get; set; }
        public string RefNumber { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int ? ProductId { get; set; }
        public string Sign { get; set; }
        public string ProductName { get; set; }
        public int?  LadderDay { get; set; }

        public int RemainingDays { get; set; }
        public ViewLadderPayment()
        {
        }
            public ViewLadderPayment(LadderPayment payment)
        {
            this.Id = payment.ID;
            this.Date = payment.Datetime;
            this.LadderSettingId = payment.LedderSettingId.Value;
            this.Account = new ViewAccount(payment.Account);
            this.Merchant = new ViewMerchant(payment.Merchant,1,"1");
            this.Status = new ViewCode(payment.Code);
            this.RefNumber = payment.RefNumber;
            this.ProductName = payment.Product.Name;
            if (payment.Datetime != null)
            {
                var EndDate = payment.Datetime.Value.AddDays(payment.LadderSetting.LadderDays.Value);
                var StartDate = DateTime.Now;
                var dif = (EndDate - StartDate).TotalDays;
                var days = (EndDate - StartDate).Days;
                this.RemainingDays = days;
                this.LadderDay = payment.LadderSetting.LadderDays;
            }
        }
    }
}
