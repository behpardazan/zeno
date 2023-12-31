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
    
    public partial class Template
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Template()
        {
            this.Shop = new HashSet<Shop>();
            this.Website = new HashSet<Website>();
        }
    
        public int ID { get; set; }
        public int TypeId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public Nullable<int> Version { get; set; }
        public string Author { get; set; }
    
        public virtual Code Code { get; set; }
        public virtual Picture Picture { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop> Shop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Website> Website { get; set; }
    }
}
