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
    
    public partial class ProductTypeBrand
    {
        public int ID { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }
    
        public virtual ProductBrand ProductBrand { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}