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
    
    public partial class ShopUser
    {
        public int ID { get; set; }
        public int ShopId { get; set; }
        public int UserId { get; set; }
    
        public virtual Shop Shop { get; set; }
        public virtual SiteUser SiteUser { get; set; }
    }
}
