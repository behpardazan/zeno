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
    
    public partial class ProductPicture
    {
        public int ID { get; set; }
        public int ProductId { get; set; }
        public int PictureId { get; set; }
        public Nullable<int> ColorId { get; set; }
        public string SyncId { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }
    
        public virtual Color Color { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual Product Product { get; set; }
    }
}
