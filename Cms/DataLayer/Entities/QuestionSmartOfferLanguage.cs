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
    
    public partial class QuestionSmartOfferLanguage
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public Nullable<int> LanguageId { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual QuestionSmartOffer QuestionSmartOffer { get; set; }
    }
}
