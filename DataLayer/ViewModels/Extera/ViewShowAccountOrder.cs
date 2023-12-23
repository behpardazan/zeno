using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShowAccountOrder
    {
        public int TotalCount { get; set; }
        public double TotalBasePrice { get; set; }
        public double TotalPrice { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalDiscountPrice { get; set; }
        public double TotalRebatePrice { get; set; }
        public double TotalSendPrice { get; set; }
        public bool IsParent { get; set; }
        public List<ViewAccountOrder> List { get; set; }
    }
}
