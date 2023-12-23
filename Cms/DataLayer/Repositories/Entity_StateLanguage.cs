using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_StateLanguage : BaseRepository<StateLanguage>
    {
        private DatabaseEntities _context;
        public Entity_StateLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void DeleteByStateId(int StateID)
        {
            List<StateLanguage> list = GetAllByStateId(StateID);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public List<StateLanguage> GetAllByStateId(int StateID)
        {
            return _context.StateLanguage.Where(p => p.StateId == StateID).ToList();
        }
    }
}
