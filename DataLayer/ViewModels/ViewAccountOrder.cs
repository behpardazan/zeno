using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewAccountOrder
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewAccount Account { get; set; }
        public ViewPaymentType PaymentType { get; set; }
        public ViewAccountAddress AccountAddress { get; set; }
        public ViewCode Status { get; set; }
        public string NextStatus { get; set; }

        public ViewRebate Rebate { get; set; }
        public ViewSendType SendType { get; set; }
        public double? SendPrice { get; set; }
        public double ProductsPrice { get; set; }
        public double Discount { get; set; }
        public double? PaymentPrice { get; set; }
        public Nullable<double> DiscountPrice { get; set; }

        public double Price { get; set; }
        public int ProductCount { get; set; }
        public int CommentCount { get; set; }
        public DateTime Datetime { get; set; }
        public int? StoreId { get; set; }
        public int? SendTypeStore { get; set; }

        public string StoreName { get; set; }
        public int? BenefitPercent { get; set; }
        public int ProductsCount { get; set; }
        public int ProductsTotalCount { get; set; }
        public bool? IsCreditShoping { get; set; }


        public List<ViewAccountOrderProduct> AccountOrderProducts { get; set; }
        public List<ViewAccountOrder> SubAccountOrders { get; set; }

        public ViewAccountOrder() { }

        public ViewAccountOrder(AccountOrder order, AccountAddress address = null)
        {
            this.Id = order.ID;
            this.IsCreditShoping = order.IsCreditShoping;

            this.Account = new ViewAccount(order.Account, address);
            this.PaymentType = new ViewPaymentType(order.PaymentType);
            this.AccountAddress = new ViewAccountAddress(order.AccountAddress);
            this.Status = new ViewCode(order.Code);
            this.Rebate = order.RebateId != null ? new ViewRebate(order.Rebate) : null;
            this.SendType = order.SendTypeId != null ? new ViewSendType(order.SendType) : null;
            this.Price = order.Price;
            this.DiscountPrice = order.DiscountPrice;

            this.ProductsPrice = order.ProductsPrice;
            this.Datetime = order.Datetime;
            this.StoreId = order.StoreId;
            this.SendTypeStore = order.SendTypeStore;
            this.SendPrice = order.SendPrice;
            try
            {
                if (order.ParentId.HasValue)
                {
                    this.PaymentPrice = order.AccountOrder2.Price;
                }
                else
                {
                    this.PaymentPrice = order.Price;

                }
            }
            catch
            {

            }
            this.StoreName = order.StoreId.HasValue ? order.Store.Name : "";
            this.AccountOrderProducts = new List<ViewAccountOrderProduct>();
            foreach (var item in order.AccountOrderProduct)
            {
                AccountOrderProducts.Add(new ViewAccountOrderProduct(item));
                this.ProductsCount += 1;
                this.ProductsTotalCount += item.Count;
            }
            this.SubAccountOrders = new List<ViewAccountOrder>();

            if (order.AccountOrder1.Any())
            {
                foreach (var item in order.AccountOrder1)
                {
                    SubAccountOrders.Add(new ViewAccountOrder(item));
                    this.ProductsCount += item.AccountOrderProduct.Count();
                    this.ProductsTotalCount += item.AccountOrderProduct.Sum(s => s.Count);
                }
            }

        }

        public ViewAccountOrder(AccountOrder order, int index, string MaxZero)
        {
            this.Id = order.ID;

            try
            {
                int ProductCount = 0;

                foreach (AccountOrderProduct item in order.AccountOrderProduct)
                {
                    ProductCount = ProductCount + item.Count;
                }
                this.ProductCount = ProductCount;
            }
            catch
            {
                this.ProductCount = 0;
            }
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Account = new ViewAccount(order.Account);
            this.PaymentType = new ViewPaymentType(order.PaymentType);
            this.AccountAddress = new ViewAccountAddress(order.AccountAddress);
            this.Status = new ViewCode(order.Code);
            this.StoreId = order.StoreId;
            this.StoreName = order.StoreId.HasValue ? order.Store.Name : "";
            this.SendTypeStore = order.SendTypeStore;
            try
            {
                if (order.ParentId.HasValue)
                {
                    this.PaymentPrice = order.AccountOrder2.Price;
                    this.IsCreditShoping = order.AccountOrder2.IsCreditShoping;
                }
                else
                {
                    this.IsCreditShoping = order.IsCreditShoping;
                    this.PaymentPrice = order.Price;

                }

            }
            catch
            {

            }
            switch (this.Status.Label)
            {
                case nameof(Enumarables.Enum_Code.ORDER_STATUS_SUCCESS):
                    {
                        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_POST.ToString();
                        break;
                    }
                    //case nameof(Enumarables.Enum_Code.ORDER_STATUS_PROCESS):
                    //    {
                    //        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_POST.ToString();
                    //        break;
                    //    }
            }
            this.Price = order.Price;
            this.ProductsPrice = order.ProductsPrice;
            this.SendPrice = order.SendPrice;
            this.Datetime = order.Datetime;
            this.DiscountPrice = order.DiscountPrice;

            this.CommentCount = order.AccountOrderComment.Count;

            this.BenefitPercent = order.BenefitPercent;
        }
    }
}
