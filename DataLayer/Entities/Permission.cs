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
    
    public partial class Permission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Permission()
        {
            this.Permission1 = new HashSet<Permission>();
            this.UserGroupPermission = new HashSet<UserGroupPermission>();
        }
    
        public int ID { get; set; }
        public int ModuleId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
    
        public virtual Module Module { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Permission> Permission1 { get; set; }
        public virtual Permission Permission2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserGroupPermission> UserGroupPermission { get; set; }
    }
}
