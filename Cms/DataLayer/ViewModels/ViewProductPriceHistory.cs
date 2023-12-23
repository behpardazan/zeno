using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductPriceHistory
    {

        public List<string> Date { get; set; }
        public List<string> Price { get; set; }


        public ViewProductPriceHistory() { }

        //public ViewProductPriceHistory(ProductPiceHistory history)
        //{
        //    if (history != null)
        //    {
        //        this.Date = history.Date.ToPersian();
        //        this.Price = history.Price.GetCurrencyFormat();
        //    }
        //}
    }
}
