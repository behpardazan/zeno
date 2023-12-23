using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionPostLanguage : BaseRepository<QuestionPostLanguage>
    {

        private DatabaseEntities _context;
        public Entity_QuestionPostLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionPostLanguage> GetAllByQuestionId(int questionId)
        {
            return _context.QuestionPostLanguage.Where(p =>
                p.QuestionId == questionId
            ).ToList();
        }

        public void DeleteByQuestionId(int postId)
        {
            List<QuestionPostLanguage> list = GetAllByQuestionId(postId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }




}
