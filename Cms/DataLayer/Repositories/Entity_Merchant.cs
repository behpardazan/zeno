using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Merchant : BaseRepository<Merchant>
    {
        private DatabaseEntities _context;
        public Entity_Merchant(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Merchant> GetAllActive()
        {
            return _context.Merchant.Where(p => p.Active).ToList();
        }
    }
}
