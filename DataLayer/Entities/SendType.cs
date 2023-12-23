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
    
    public partial class SendType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SendType()
        {
            this.AccountOrder = new HashSet<AccountOrder>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public Nullable<double> BasePrice { get; set; }
        public Nullable<double> PerProductPrice { get; set; }
        public Nullable<double> MaxPrice { get; set; }
        public Nullable<double> MinPriceForFree { get; set; }
        public int MaxDays { get; set; }
        public Nullable<int> StoreId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountOrder> AccountOrder { get; set; }
        public virtual Store Store { get; set; }
    }
}