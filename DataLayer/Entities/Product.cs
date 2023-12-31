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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.AccountBasket = new HashSet<AccountBasket>();
            this.AccountOrderProduct = new HashSet<AccountOrderProduct>();
            this.Discount = new HashSet<Discount>();
            this.ProductBarcode = new HashSet<ProductBarcode>();
            this.ProductColor = new HashSet<ProductColor>();
            this.ProductComment = new HashSet<ProductComment>();
            this.ProductCustomValue = new HashSet<ProductCustomValue>();
            this.ProductLanguage = new HashSet<ProductLanguage>();
            this.ProductLike = new HashSet<ProductLike>();
            this.ProductNotify = new HashSet<ProductNotify>();
            this.ProductPicture = new HashSet<ProductPicture>();
            this.ProductQuantity = new HashSet<ProductQuantity>();
            this.ProductSize = new HashSet<ProductSize>();
            this.ProductVisit = new HashSet<ProductVisit>();
            this.Review = new HashSet<Review>();
            this.ShopResellerCollectionProduct = new HashSet<ShopResellerCollectionProduct>();
            this.ShopResellerProduct = new HashSet<ShopResellerProduct>();
            this.StoreProduct = new HashSet<StoreProduct>();
            this.ProductPiceHistory = new HashSet<ProductPiceHistory>();
            this.ProductVideo = new HashSet<ProductVideo>();
            this.LadderPayment = new HashSet<LadderPayment>();
            this.Post = new HashSet<Post>();
            this.QuestionProduct = new HashSet<QuestionProduct>();
            this.ProductsSubCategory = new HashSet<ProductsSubCategory>();
            this.ProductsType = new HashSet<ProductsType>();
            this.ProductsCategory = new HashSet<ProductsCategory>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ShopId { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductSubCategoryId { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> DocId { get; set; }
        public string SyncId { get; set; }
        public string Name { get; set; }
        public string CodeValue { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> MinOrder { get; set; }
        public Nullable<double> LastPrice { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> VisitCount { get; set; }
        public bool ShowHomePage { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }
        public bool Active { get; set; }
        public double MinPrice { get; set; }
        public bool IsService { get; set; }
        public double DiscountValue { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<int> ProductOptionId { get; set; }
        public Nullable<int> SellCount { get; set; }
        public string H1 { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Img { get; set; }
        public double Weight { get; set; }
        public bool Deleted { get; set; }
        public Nullable<int> MaxOrderCount { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<bool> IsCopy { get; set; }
        public Nullable<int> AudioDemoFileId { get; set; }
        public Nullable<int> AudioFileId { get; set; }
        public Nullable<int> MusicFileId { get; set; }
        public Nullable<int> MusicDemoFileId { get; set; }
        public Nullable<int> PdfDemoFileId { get; set; }
        public Nullable<int> PdfFileId { get; set; }
        public string ViedoLink { get; set; }
        public Nullable<bool> NoActiveSearch { get; set; }
        public Nullable<int> ZipFileId { get; set; }
        public Nullable<int> VideoFileId { get; set; }
        public Nullable<int> VideoDemoFileId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> ErrorLowCount { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CustomURL { get; set; }
        public string Sco_id { get; set; }
        public Nullable<bool> IsAdvertising { get; set; }
        public Nullable<bool> LadderActive { get; set; }
        public Nullable<System.DateTime> LadderDate { get; set; }
        public string Phone { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string Address { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<bool> IsPublish { get; set; }
        public Nullable<System.DateTime> LadderDateExpire { get; set; }
        public Nullable<System.DateTime> SpecialProduct { get; set; }
        public Nullable<bool> IsSpecialProduct { get; set; }
        public string MetaDescription { get; set; }
        public string RobotMeta { get; set; }
        public string Canonical { get; set; }
        public string Schema { get; set; }
        public bool NoFollow { get; set; }
        public bool NoIndex { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountBasket> AccountBasket { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountOrderProduct> AccountOrderProduct { get; set; }
        public virtual Code Code { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Discount> Discount { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual WebsiteDocument WebsiteDocument { get; set; }
        public virtual ProductBrand ProductBrand { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductCompany ProductCompany { get; set; }
        public virtual ProductOption ProductOption { get; set; }
        public virtual ProductSubCategory ProductSubCategory { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual SiteUser SiteUser { get; set; }
        public virtual Unit Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductBarcode> ProductBarcode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductColor> ProductColor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductComment> ProductComment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCustomValue> ProductCustomValue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductLanguage> ProductLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductLike> ProductLike { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductNotify> ProductNotify { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPicture> ProductPicture { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductQuantity> ProductQuantity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSize> ProductSize { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductVisit> ProductVisit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Review { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShopResellerCollectionProduct> ShopResellerCollectionProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShopResellerProduct> ShopResellerProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreProduct> StoreProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPiceHistory> ProductPiceHistory { get; set; }
        public virtual File File { get; set; }
        public virtual File File1 { get; set; }
        public virtual File File2 { get; set; }
        public virtual File File3 { get; set; }
        public virtual File File4 { get; set; }
        public virtual File File5 { get; set; }
        public virtual File File6 { get; set; }
        public virtual ProductType ProductType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductVideo> ProductVideo { get; set; }
        public virtual Account Account { get; set; }
        public virtual City City { get; set; }
        public virtual State State { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LadderPayment> LadderPayment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Post { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionProduct> QuestionProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsSubCategory> ProductsSubCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsType> ProductsType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsCategory> ProductsCategory { get; set; }
    }
}
