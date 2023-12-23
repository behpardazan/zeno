using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_QuestionProductTypeLanguage : BaseRepository<QuestionProductTypeLanguage>
    {

        private DatabaseEntities _context;
        public Entity_QuestionProductTypeLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<QuestionProductTypeLanguage> GetAllByQuestionId(int questionId)
        {
            return _context.QuestionProductTypeLanguage.Where(p =>
                p.QuestionId == questionId
            ).ToList();
        }

        public void DeleteByQuestionId(int postId)
        {
            List<QuestionProductTypeLanguage> list = GetAllByQuestionId(postId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }




}
