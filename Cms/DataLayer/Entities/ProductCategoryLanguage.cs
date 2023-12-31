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
    
    public partial class ProductCategoryLanguage
    {
        public int ID { get; set; }
        public int ProductCategoryId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string DescriptionMeta { get; set; }
        public string TitlePage { get; set; }
        public string H1 { get; set; }
        public string Description { get; set; }
        public string MetaDescription { get; set; }
        public string RobotMeta { get; set; }
        public string Canonical { get; set; }
        public string Body { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
