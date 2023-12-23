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
    
    public partial class Newsletter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Newsletter()
        {
            this.NewsletterItem = new HashSet<NewsletterItem>();
        }
    
        public int ID { get; set; }
        public Nullable<int> WebsiteId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public System.DateTime Datetime { get; set; }
        public bool Active { get; set; }
    
        public virtual Website Website { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsletterItem> NewsletterItem { get; set; }
    }
}