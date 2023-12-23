using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewEmail
    {
        public string Body { get; set; }
        public string[] UserEmails { get; set; }

        public ViewEmail() { }
    }
}
