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
    
    public partial class ProductNotify
    {
        public int ID { get; set; }
        public int AccountId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> ColorId { get; set; }
        public Nullable<int> SizeId { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSms { get; set; }
        public System.DateTime Datetime { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Color Color { get; set; }
        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
    }
}