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
    
    public partial class WebsiteFormField
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WebsiteFormField()
        {
            this.WebsiteFormValue = new HashSet<WebsiteFormValue>();
        }
    
        public int ID { get; set; }
        public int FormId { get; set; }
        public Nullable<int> TypeId { get; set; }
        public string Name { get; set; }
        public Nullable<int> MaxCount { get; set; }
        public string ColumnName { get; set; }
        public int ShowNumber { get; set; }
        public Nullable<bool> Requier { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
    
        public virtual Code Code { get; set; }
        public virtual WebsiteForm WebsiteForm { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebsiteFormValue> WebsiteFormValue { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
