using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewFile
    {
        public int FileId { get; set; }
        public string Type { get; set; }
        
        public ViewFile()
        {

        }
        public ViewFile(File file, int index)
        {
            this.FileId = file.FileId;
            this.Type = file.ContentType;
           
        }
    }
}
