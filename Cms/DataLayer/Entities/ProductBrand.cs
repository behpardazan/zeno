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
    
    public partial class ProductBrand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductBrand()
        {
            this.Discount = new HashSet<Discount>();
            this.Product = new HashSet<Product>();
            this.ProductBrandUser = new HashSet<ProductBrandUser>();
            this.ProductCustomField = new HashSet<ProductCustomField>();
            this.ProductTypeBrand = new HashSet<ProductTypeBrand>();
            this.Rebate = new HashSet<Rebate>();
            this.ShopResellerCollectionProduct = new HashSet<ShopResellerCollectionProduct>();
            this.ShopResellerProduct = new HashSet<ShopResellerProduct>();
            this.ProductBrandLanguage = new HashSet<ProductBrandLanguage>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string SyncId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }
        public bool IsPublic { get; set; }
        public string ThemeColor { get; set; }
        public string Description { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string H1 { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Discount> Discount { get; set; }
        public virtual Picture Picture { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductBrandUser> ProductBrandUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCustomField> ProductCustomField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTypeBrand> ProductTypeBrand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rebate> Rebate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShopResellerCollectionProduct> ShopResellerCollectionProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShopResellerProduct> ShopResellerProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductBrandLanguage> ProductBrandLanguage { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}
