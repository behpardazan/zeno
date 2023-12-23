using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewAccountOrderProduct
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> ResellerId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public System.DateTime Datetime { get; set; }
        public Nullable<double> ProductPrice { get; set; }
        public Nullable<double> ProductDiscount { get; set; }
        public Nullable<int> ColorId { get; set; }
        public Nullable<int> SizeId { get; set; }
        public string SyncId { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }

        public ViewProduct Product { get; set; }
        public ViewColor Color { get; set; }
        public ViewSize Size { get; set; }
        public ViewProductOptionItem ProductOptionItem { get; set; }

        public ViewAccountOrderProduct() { }

        public ViewAccountOrderProduct(AccountOrderProduct orderProduct)
        {
            this.Id = orderProduct.ID;
            this.OrderId = orderProduct.OrderId;
            this.ProductId = orderProduct.ProductId;
            this.ResellerId = orderProduct.ResellerId;
            this.Count = orderProduct.Count;
            this.Price = orderProduct.Price;
            this.Datetime = orderProduct.Datetime;
            this.ProductPrice = orderProduct.ProductPrice;
            this.ProductDiscount = orderProduct.ProductDiscount;
            this.ColorId = orderProduct.ColorId;
            this.SizeId = orderProduct.SizeId;
            this.SyncId = orderProduct.SyncId;
            this.SyncDatetime = orderProduct.SyncDatetime;

            this.Product = new ViewProduct(orderProduct.Product);
            this.Color = orderProduct.Color != null ? new ViewColor(orderProduct.Color, false) : null;
            this.Size = orderProduct.Size != null ? new ViewSize(orderProduct.Size, false) : null;
            this.ProductOptionItem = orderProduct.ProductOptionItem != null ? new ViewProductOptionItem(orderProduct.ProductOptionItem, false) : null;

        }

    }
}
