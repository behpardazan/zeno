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
    
    public partial class WebsiteFormAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WebsiteFormAccount()
        {
            this.WebsiteFormValue = new HashSet<WebsiteFormValue>();
        }
    
        public int ID { get; set; }
        public int FormId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public string FullName { get; set; }
        public System.DateTime Datetime { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual WebsiteForm WebsiteForm { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebsiteFormValue> WebsiteFormValue { get; set; }
    }
}
