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
    
    public partial class Guarantee
    {
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
    
        public virtual GuaranteeCompany GuaranteeCompany { get; set; }
    }
}
