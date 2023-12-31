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
    
    public partial class AnswerSmartOffer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AnswerSmartOffer()
        {
            this.AnswerSmartOfferLanguage = new HashSet<AnswerSmartOfferLanguage>();
        }
    
        public int ID { get; set; }
        public int QuestionId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public string Text { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public bool Active { get; set; }
    
        public virtual Picture Picture { get; set; }
        public virtual QuestionSmartOffer QuestionSmartOffer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnswerSmartOfferLanguage> AnswerSmartOfferLanguage { get; set; }
    }
}
