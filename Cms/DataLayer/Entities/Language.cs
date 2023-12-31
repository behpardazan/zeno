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
    
    public partial class Language
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Language()
        {
            this.BannerLanguage = new HashSet<BannerLanguage>();
            this.CategoryLanguage = new HashSet<CategoryLanguage>();
            this.CityLanguage = new HashSet<CityLanguage>();
            this.ColorLanguage = new HashSet<ColorLanguage>();
            this.MenuLanguage = new HashSet<MenuLanguage>();
            this.PostLanguage = new HashSet<PostLanguage>();
            this.ProductCategoryLanguage = new HashSet<ProductCategoryLanguage>();
            this.ProductCustomFieldLanguage = new HashSet<ProductCustomFieldLanguage>();
            this.ProductCustomItemLanguage = new HashSet<ProductCustomItemLanguage>();
            this.ProductLanguage = new HashSet<ProductLanguage>();
            this.ProductSubCategoryLanguage = new HashSet<ProductSubCategoryLanguage>();
            this.ProductTypeLanguage = new HashSet<ProductTypeLanguage>();
            this.ShopResellerLanguage = new HashSet<ShopResellerLanguage>();
            this.SizeLanguage = new HashSet<SizeLanguage>();
            this.SliderLanguage = new HashSet<SliderLanguage>();
            this.SurveyLanguage = new HashSet<SurveyLanguage>();
            this.SurveyQuestionItemLanguage = new HashSet<SurveyQuestionItemLanguage>();
            this.SurveyQuestionLanguage = new HashSet<SurveyQuestionLanguage>();
            this.WebsiteDetail = new HashSet<WebsiteDetail>();
            this.WebsiteDomain = new HashSet<WebsiteDomain>();
            this.WebsiteLanguage = new HashSet<WebsiteLanguage>();
            this.ProductBrandLanguage = new HashSet<ProductBrandLanguage>();
            this.StateLanguage = new HashSet<StateLanguage>();
            this.GalleryLanguage = new HashSet<GalleryLanguage>();
            this.AnswerSmartOfferLanguage = new HashSet<AnswerSmartOfferLanguage>();
            this.QuestionSmartOfferLanguage = new HashSet<QuestionSmartOfferLanguage>();
            this.QuestionProductLanguage = new HashSet<QuestionProductLanguage>();
            this.QuestionPostLanguage = new HashSet<QuestionPostLanguage>();
            this.QuestionProductTypeLanguage = new HashSet<QuestionProductTypeLanguage>();
        }
    
        public int ID { get; set; }
        public Nullable<int> DirectionId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Culture { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BannerLanguage> BannerLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoryLanguage> CategoryLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CityLanguage> CityLanguage { get; set; }
        public virtual Code Code { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ColorLanguage> ColorLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuLanguage> MenuLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostLanguage> PostLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCategoryLanguage> ProductCategoryLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCustomFieldLanguage> ProductCustomFieldLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductCustomItemLanguage> ProductCustomItemLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductLanguage> ProductLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSubCategoryLanguage> ProductSubCategoryLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTypeLanguage> ProductTypeLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShopResellerLanguage> ShopResellerLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SizeLanguage> SizeLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SliderLanguage> SliderLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyLanguage> SurveyLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyQuestionItemLanguage> SurveyQuestionItemLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyQuestionLanguage> SurveyQuestionLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebsiteDetail> WebsiteDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebsiteDomain> WebsiteDomain { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WebsiteLanguage> WebsiteLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductBrandLanguage> ProductBrandLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StateLanguage> StateLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GalleryLanguage> GalleryLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnswerSmartOfferLanguage> AnswerSmartOfferLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionSmartOfferLanguage> QuestionSmartOfferLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionProductLanguage> QuestionProductLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionPostLanguage> QuestionPostLanguage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionProductTypeLanguage> QuestionProductTypeLanguage { get; set; }
    }
}
