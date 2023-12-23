using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewShippingSubscription
    {
        public int Id { get; set; }
        public ViewState State { get; set; }
        public double Price { get; set; }

        public ViewShippingSubscription() { }

        public ViewShippingSubscription(ShippingSubscription shippingSubscription)
        {
            if (shippingSubscription != null)
            {
                this.Id = shippingSubscription.ID;
                this.Price = shippingSubscription.Price;
                this.State = new ViewState(shippingSubscription.State);
            }
        }
    }
}
