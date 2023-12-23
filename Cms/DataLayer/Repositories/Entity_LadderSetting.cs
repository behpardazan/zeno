using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_LadderSetting : BaseRepository<LadderSetting>
    {
        private DatabaseEntities _context;
        public Entity_LadderSetting(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<LadderSetting> GetAllActive()
        {
            return _context.LadderSetting.Where(p => p.Active == true ).ToList();
        }
    }
}
