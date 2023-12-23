using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Repositories
{
    public class Entity_SmartOffer : BaseRepository<SmartOfferVisit>
    {
        private DatabaseEntities _context;
        public Entity_SmartOffer(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void IncreaseView(int? accountId = null)
        {
            HttpContext user_context = HttpContext.Current;
            var Request = user_context.Request;
            var ip = BaseSecurity.GetClientIPAddress();
            var temp = _context.SmartOfferVisit.FirstOrDefault(s => s.IP == ip || s.AccountId == accountId);
            if (temp == null)
            {
                SmartOfferVisit view = new SmartOfferVisit()
                {

                    DateTime = DateTime.Now,
                    IP = BaseSecurity.GetClientIPAddress(),
                    AccountId = accountId,
                };
                Insert(view);
                Save();
            }
           
        }

    }
}
