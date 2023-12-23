using DataLayer.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_PaymentSubscription : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_PaymentSubscription == false)
            {
                Sync_Base.Is_PaymentSubscription = true;
                UnitOfWork _context = new UnitOfWork();
                var listAccount = _context.Account.Where(s => s.ShippingSubscriptionPaymentActive == true).ToList();
                foreach (var item in listAccount)
                {
                    var date = item.ShippingSubscriptionPaymentDate.Value.AddDays(30);
                    if (DateTime.Now > date)
                    {
                        item.ShippingSubscriptionPaymentActive = false;

                        _context.Account.Update(item);
                        _context.Save();
                    }
                }
                _context.Dispose();
                Sync_Base.Is_PaymentSubscription = false;
            }
        }
    }
}
