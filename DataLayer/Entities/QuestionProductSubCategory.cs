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
    
    public partial class QuestionProductSubCategory
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
    
        public virtual ProductSubCategory ProductSubCategory { get; set; }
    }
}
