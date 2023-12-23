using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_AnswerSmartOfferLanguage : BaseRepository<AnswerSmartOfferLanguage>
    {
        private DatabaseEntities _context;
        public Entity_AnswerSmartOfferLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<AnswerSmartOfferLanguage> GetAllByAnswerSmartOfferId(int answerId)
        {
            return _context.AnswerSmartOfferLanguage.Where(p => p.AnswerId == answerId).ToList();
        }

        public void DeleteByAnswerSmartOfferId(int typeId)
        {
            List<AnswerSmartOfferLanguage> list = GetAllByAnswerSmartOfferId(typeId);
            
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
            Save();
        }
    }
}
