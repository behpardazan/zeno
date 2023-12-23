using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionProductLanguage : BaseRepository<QuestionProductLanguage>
    {

        private DatabaseEntities _context;
        public Entity_QuestionProductLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionProductLanguage> GetAllByQuestionId(int questionId)
        {
            return _context.QuestionProductLanguage.Where(p =>
                p.QuestionId == questionId
            ).ToList();
        }

        public void DeleteByQuestionId(int postId)
        {
            List<QuestionProductLanguage> list = GetAllByQuestionId(postId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }




}
