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
    
    public partial class WebsiteDomain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WebsiteDomain()
        {
            this.AlexaRank = new HashSet<AlexaRank>();
        }
    
        public int ID { get; set; }
        public int WebsiteId { get; set; }
        public string Domain { get; set; }
        public string ActivationKey { get; set; }
        public Nullable<int> LanguageId { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsShortUrl { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlexaRank> AlexaRank { get; set; }
        public virtual Language Language { get; set; }
        public virtual Website Website { get; set; }
    }
}
