using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewLadderPaymentOrder
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewAccount Account { get; set; }
        public ViewCode Status { get; set; }
        public string NextStatus { get; set; }
        public ViewSendType SendType { get; set; }
        public ViewProduct Product { get; set; }
        public ViewLadderSetting LadderSetting { get; set; }

        public double Price { get; set; }
        public DateTime? Datetime { get; set; }

        public ViewLadderPaymentOrder() { }

        public ViewLadderPaymentOrder(LadderPayment payment)
        {
            this.Id = payment.ID;
            this.Account = new ViewAccount(payment.Account);
            this.Product = new ViewProduct(payment.Product);
            this.LadderSetting = new ViewLadderSetting(payment.LadderSetting);
            switch (this.Status.Label)
            {
                case nameof(Enumarables.Enum_Code.ORDER_STATUS_SUCCESS):
                    {
                        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_PROCESS.ToString();
                        break;
                    }
                case nameof(Enumarables.Enum_Code.ORDER_STATUS_PROCESS):
                    {
                        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_POST.ToString();
                        break;
                    }
            }
            this.Status = new ViewCode(payment.Code);
            this.Price = payment.LadderSetting.LadderPrice.Value;
            this.Datetime = payment.Datetime;

        }

        public ViewLadderPaymentOrder(LadderPayment order, int index, string MaxZero)
        {
            this.Id = order.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Account = new ViewAccount(order.Account);
            this.Product = new ViewProduct(order.Product);
            this.Status = new ViewCode(order.Code);
            this.LadderSetting = new ViewLadderSetting(order.LadderSetting);
            switch (this.Status.Label)
            {
                case nameof(Enumarables.Enum_Code.ORDER_STATUS_SUCCESS):
                    {
                        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_PROCESS.ToString();
                        break;
                    }
                case nameof(Enumarables.Enum_Code.ORDER_STATUS_PROCESS):
                    {
                        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_POST.ToString();
                        break;
                    }
            }
            this.Price = order.LadderSetting.LadderPrice.Value;
            this.Datetime = order.Datetime;
            
        }
    }
}
