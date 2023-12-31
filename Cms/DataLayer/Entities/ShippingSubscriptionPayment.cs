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
    
    public partial class ShippingSubscriptionPayment
    {
        public int ID { get; set; }
        public Nullable<int> SiteUserId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public int StatusId { get; set; }
        public int MerchantId { get; set; }
        public int SubjectId { get; set; }
        public string RefNumber { get; set; }
        public string ExternalInfo1 { get; set; }
        public string ExternalInfo2 { get; set; }
        public string ExternalInfo3 { get; set; }
        public string ExternalInfo4 { get; set; }
        public double Amount { get; set; }
        public string IpAddress { get; set; }
        public System.DateTime Datetime { get; set; }
        public string Description { get; set; }
        public string Sign { get; set; }
        public Nullable<int> PaymentWebsiteId { get; set; }
        public Nullable<int> CurrencyTypeId { get; set; }
        public string ExternalInfo5 { get; set; }
        public string ExternalInfo6 { get; set; }
        public string ExternalInfo7 { get; set; }
        public Nullable<bool> IsMobile { get; set; }
        public Nullable<int> AccountAddressId { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Code Code { get; set; }
        public virtual Code Code1 { get; set; }
        public virtual Merchant Merchant { get; set; }
        public virtual PaymentSubject PaymentSubject { get; set; }
        public virtual PaymentWebsite PaymentWebsite { get; set; }
        public virtual SiteUser SiteUser { get; set; }
        public virtual AccountAddress AccountAddress { get; set; }
    }
}
