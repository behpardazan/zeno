using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_CityLanguage : BaseRepository<CityLanguage>
    {
        private DatabaseEntities _context;
        public Entity_CityLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public void DeleteByCityId(int cityID)
        {
            List<CityLanguage> list = GetAllByCityId(cityID);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }

        public List<CityLanguage> GetAllByCityId(int cityID)
        {
            return _context.CityLanguage.Where(p => p.CityId == cityID).ToList();
        }
    }
}
