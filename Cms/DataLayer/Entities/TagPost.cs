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
    
    public partial class TagPost
    {
        public int Id { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> TagId { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}