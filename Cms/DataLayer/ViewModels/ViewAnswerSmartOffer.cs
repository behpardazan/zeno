using DataLayer.Entities;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewAnswerSmartOffer
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewQuestionSmartOffer QuestionSmartOffer { get; set; }
        public ViewPicture Picture { get; set; }
        public string Text { get; set; }
        public bool? Active { get; set; }
        public int? ShowNumber { get; set; }
        public ViewAnswerSmartOffer() { }

        public ViewAnswerSmartOffer(AnswerSmartOffer answerSmartOffer)
        {
            this.Id = answerSmartOffer.ID;
        
            this.Text = answerSmartOffer.Text;
            this.QuestionSmartOffer = new ViewQuestionSmartOffer(answerSmartOffer.QuestionSmartOffer);
            this.Picture = new ViewPicture(answerSmartOffer.Picture);
            this.ShowNumber = answerSmartOffer.ShowNumber;
            this.Active = answerSmartOffer.Active;
        }

        public ViewAnswerSmartOffer(AnswerSmartOffer answerSmartOffer, int index, string MaxZero)
        {
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Id = answerSmartOffer.ID;
            this.Text = answerSmartOffer.Text;
            this.QuestionSmartOffer = new ViewQuestionSmartOffer(answerSmartOffer.QuestionSmartOffer);
            this.Picture = new ViewPicture(answerSmartOffer.Picture);
            this.ShowNumber = answerSmartOffer.ShowNumber;
            this.Active = answerSmartOffer.Active;
        }
    }
}
