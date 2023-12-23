using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewQuestionSmartOffer
    {
        public int Id { get; set; }
        public string RowId { get; set; }

        public string Text { get; set; }
        public int? ShowNumber { get; set; }
        public bool? Active { get; set; }
        public List<ViewAnswerSmartOffer> AnswerSmartOffer { get; set; }


        public ViewQuestionSmartOffer() { }

        public ViewQuestionSmartOffer(QuestionSmartOffer questionSmartOffer, int index, string MaxZero)
        {
            if (questionSmartOffer != null)
            {
                this.Id = questionSmartOffer.ID;
                List<ViewAnswerSmartOffer> list = new List<ViewAnswerSmartOffer>();
                foreach(var item in questionSmartOffer.AnswerSmartOffer)
                {
                    list.Add(new ViewAnswerSmartOffer(item));
                }
                this.AnswerSmartOffer = list;
                this.Active = questionSmartOffer.Active;
                this.Text = questionSmartOffer.Text;
                this.ShowNumber = questionSmartOffer.ShowNumber;

            }
        }
        public ViewQuestionSmartOffer(QuestionSmartOffer questionSmartOffer)
        {
            if (questionSmartOffer != null)
            {
                this.Id = questionSmartOffer.ID;

                this.Active = questionSmartOffer.Active;
                this.Text = questionSmartOffer.Text;
                this.ShowNumber = questionSmartOffer.ShowNumber;

            }
        }
    }
}
