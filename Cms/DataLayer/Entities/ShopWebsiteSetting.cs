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
    
    public partial class ShopWebsiteSetting
    {
        public int ID { get; set; }
        public int WebsiteId { get; set; }
        public bool HasShop { get; set; }
        public bool HasProductType { get; set; }
        public bool HasProductCategory { get; set; }
        public bool HasProductSubCategory { get; set; }
        public bool HasBrand { get; set; }
        public bool HasGarranty { get; set; }
        public bool HasColor { get; set; }
        public bool HasSize { get; set; }
        public bool HasStatus { get; set; }
        public bool HasLastPrice { get; set; }
        public bool HasPrice { get; set; }
        public bool HasMinOrder { get; set; }
        public bool HasCustomFields { get; set; }
        public bool HasPicture { get; set; }
        public bool HasDescription { get; set; }
        public bool HasSummary { get; set; }
        public bool HasShowNumber { get; set; }
        public bool HasShowHomePage { get; set; }
        public bool HasDocument { get; set; }
        public Nullable<int> MaxShopProductCount { get; set; }
        public bool HasQuantity { get; set; }
        public bool HasQuantityColor { get; set; }
        public bool HasUnit { get; set; }
        public bool HasCodeValue { get; set; }
        public bool HasActive { get; set; }
        public bool HasStore { get; set; }
        public bool HasProductOption { get; set; }
        public bool HasSeo { get; set; }
        public bool HasRequierdState { get; set; }
        public bool HasRequierdCity { get; set; }
        public bool HasUserName { get; set; }
        public string MobileRegix { get; set; }
        public string ZipCodeRegix { get; set; }
        public string PhoneRegix { get; set; }
        public string EmailRegix { get; set; }
        public string PasswordRegix { get; set; }
        public double UnitPrice { get; set; }
        public bool HasUnitPrice { get; set; }
        public bool HasService { get; set; }
        public bool IsLoginWithNationalCode { get; set; }
        public string NationalCodeRegix { get; set; }
        public bool ProductMaxOrderCount { get; set; }
        public string MapKey { get; set; }
        public bool HasPriceRange { get; set; }
        public bool HasCompany { get; set; }
        public bool HasSendByPost { get; set; }
        public bool HasCopyProduct { get; set; }
        public Nullable<bool> HasCountPostPrice { get; set; }
        public bool HasDate { get; set; }
        public bool ErrorLowCount { get; set; }
        public bool HasCreditShoping { get; set; }
        public Nullable<bool> IsPersianChar { get; set; }
        public bool HasRequierdZipCode { get; set; }
        public Nullable<bool> ShowProductInPost { get; set; }
        public bool HasMultiCategory { get; set; }
    
        public virtual Website Website { get; set; }
    }
}