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
    
    public partial class SurveyQuestionItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SurveyQuestionItem()
        {
            this.SurveyAnswerDetail = new HashSet<SurveyAnswerDetail>();
            this.SurveyQuestionItemLanguage = new HashSet<SurveyQuestionItemLanguage>();
        }
    
        public int ID { get; set; }
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> ShowNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyAnswerDetail> SurveyAnswerDetail { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SurveyQuestionItemLanguage> SurveyQuestionItemLanguage { get; set; }
    }
}
