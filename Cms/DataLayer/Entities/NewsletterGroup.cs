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
    
    public partial class NewsletterGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NewsletterGroup()
        {
            this.NewsletterItem = new HashSet<NewsletterItem>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsletterItem> NewsletterItem { get; set; }
    }
}