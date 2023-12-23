using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class LavinApi
    {
        
        public class GetToken { 
        
            public string token { get;set; }
            public GetToken() {
            
            }
        }

        public class GetUser
        {

            public int statusCode { get; set; }
            public string ID { get; set; }

            public GetUser()
            {

            }
        }
        public class CreateUser
        {

            public int statusCode { get; set; }
            public int user_id { get; set; }

            public CreateUser()
            {

            }
        }
        public class CreateLinces
        {

            public int statusCode { get; set; }
            public int license_id { get; set; }

            public CreateLinces()
            {

            }
        }
    }
}
