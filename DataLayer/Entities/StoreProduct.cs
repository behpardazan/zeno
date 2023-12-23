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
    
    public partial class StoreProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StoreProduct()
        {
            this.StoreProductQuantity = new HashSet<StoreProductQuantity>();
        }
    
        public int ID { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int StatusId { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual Code Code { get; set; }
        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreProductQuantity> StoreProductQuantity { get; set; }
    }
}
