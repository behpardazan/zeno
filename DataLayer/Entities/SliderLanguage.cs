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
    
    public partial class SliderLanguage
    {
        public int ID { get; set; }
        public int SliderId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual Slider Slider { get; set; }
    }
}
