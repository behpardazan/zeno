//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccountBasket
    {
        public int ID { get; set; }
        public Nullable<int> AccountId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> ResellerId { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<int> ProductColorId { get; set; }
        public Nullable<int> ProductSizeId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public System.DateTime Datetime { get; set; }
        public double ProductPrice { get; set; }
        public double ProductDiscount { get; set; }
        public Nullable<double> InstallationPrice { get; set; }
        public Nullable<int> ProductOptionItemId { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Color Color { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductOptionItem ProductOptionItem { get; set; }
        public virtual ShopReseller ShopReseller { get; set; }
        public virtual Size Size { get; set; }
        public virtual Store Store { get; set; }
    }
}