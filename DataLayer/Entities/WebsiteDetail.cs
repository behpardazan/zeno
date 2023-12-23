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
    
    public partial class WebsiteDetail
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string AboutUs { get; set; }
        public string ContactUs { get; set; }
        public string Rules { get; set; }
        public string PurchaseGuide { get; set; }
        public string Abstract { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Instagram { get; set; }
        public string Telegram { get; set; }
        public string Whatsapp { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Address2 { get; set; }
        public Nullable<int> LanguageId { get; set; }
        public string Waze { get; set; }
        public string Linkedin { get; set; }
        public string Aparat { get; set; }
        public string Summary { get; set; }
        public string DateMap { get; set; }
        public string TitleOtherPage { get; set; }
    
        public virtual Language Language { get; set; }
    }
}