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
    
    public partial class OnlineOrder
    {
        public int ID { get; set; }
        public Nullable<int> NationalCode { get; set; }
        public string subject { get; set; }
        public Nullable<int> Phone { get; set; }
        public string Body { get; set; }
        public Nullable<int> FileId { get; set; }
        public Nullable<int> StatusOrder { get; set; }
        public string RefrenceCode { get; set; }
        public Nullable<System.DateTime> DateTimeOrder { get; set; }
        public Nullable<double> Price { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string PlaceBirth { get; set; }
        public Nullable<System.DateTime> BirthDay { get; set; }
        public Nullable<int> PassportFileID { get; set; }
    
        public virtual Code Code { get; set; }
        public virtual WebsiteDocument WebsiteDocument { get; set; }
        public virtual WebsiteDocument WebsiteDocument1 { get; set; }
        public virtual WebsiteDocument WebsiteDocument2 { get; set; }
    }
}
