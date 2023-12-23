using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_DashboardFiles : BaseRepository<DashboardFiles>
    {
        private DatabaseEntities _context;
        public Entity_DashboardFiles(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
    }
}
