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
    
    public partial class AgreementPlatform
    {
        public int ID { get; set; }
        public Nullable<int> AgreementId { get; set; }
        public Nullable<int> PlatformId { get; set; }
    
        public virtual Agreement Agreement { get; set; }
        public virtual Code Code { get; set; }
    }
}