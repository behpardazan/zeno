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
    
    public partial class EmailAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmailAddress()
        {
            this.Email1 = new HashSet<Email>();
            this.EmailInbox = new HashSet<EmailInbox>();
            this.EmailSetting = new HashSet<EmailSetting>();
        }
    
        public int ID { get; set; }
        public int EmailHostId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public System.DateTime CreateDatetime { get; set; }
        public System.DateTime UpdateDatetime { get; set; }
        public string Color { get; set; }
        public bool AutoSync { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Email> Email1 { get; set; }
        public virtual SiteUser SiteUser { get; set; }
        public virtual EmailHost EmailHost { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailInbox> EmailInbox { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailSetting> EmailSetting { get; set; }
    }
}