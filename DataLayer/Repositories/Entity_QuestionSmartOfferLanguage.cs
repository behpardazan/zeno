using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionSmartOfferLanguage : BaseRepository<QuestionSmartOfferLanguage>
    {
        private DatabaseEntities _context;
        public Entity_QuestionSmartOfferLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionSmartOfferLanguage> GetAllByQuestionSmartOfferId(int questionId)
        {
            return _context.QuestionSmartOfferLanguage.Where(p => p.QuestionId == questionId).ToList();
        }

        public void DeleteByQuestionSmartOfferId(int typeId)
        {
            List<QuestionSmartOfferLanguage> list = GetAllByQuestionSmartOfferId(typeId);
            
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
            Save();
        }
    }
}
