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
    
    public partial class TelegramCommand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TelegramCommand()
        {
            this.TelegramKeyBoardItem = new HashSet<TelegramKeyBoardItem>();
        }
    
        public int ID { get; set; }
        public int BotId { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<int> PictureId { get; set; }
        public Nullable<int> KeyboardId { get; set; }
        public string Command { get; set; }
        public string Name { get; set; }
        public string TextMessage { get; set; }
        public string ParseMode { get; set; }
        public bool DisableWebPagePreview { get; set; }
        public bool DisableNotification { get; set; }
    
        public virtual Code Code { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual TelegramBot TelegramBot { get; set; }
        public virtual TelegramKeyBoard TelegramKeyBoard { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TelegramKeyBoardItem> TelegramKeyBoardItem { get; set; }
    }
}