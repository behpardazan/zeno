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
    
    public partial class ShopCustomField
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ShopCustomField()
        {
            this.ShopCustomItem = new HashSet<ShopCustomItem>();
            this.ShopCustomValue = new HashSet<ShopCustomValue>();
        }
    
        public int ID { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public bool IsRequired { get; set; }
    
        public virtual Code Code { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShopCustomItem> ShopCustomItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShopCustomValue> ShopCustomValue { get; set; }
    }
}
