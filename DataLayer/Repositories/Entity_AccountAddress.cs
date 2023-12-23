using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AccountAddress : BaseRepository<AccountAddress>
    {
        private DatabaseEntities _context;
        public Entity_AccountAddress(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<AccountAddress> GetAllByAccount(int accountId)
        {
            return _context.AccountAddress.Where(p =>
            p.AccountId == accountId
            ).OrderByDescending(p=>p.ID).ToList();
        }
    }
}
