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
    
    public partial class SurveyAnswerDetail
    {
        public int ID { get; set; }
        public int AnswerId { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string AnswerText { get; set; }
        public Nullable<bool> AnswerBool { get; set; }
    
        public virtual SurveyAnswer SurveyAnswer { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
        public virtual SurveyQuestionItem SurveyQuestionItem { get; set; }
    }
}