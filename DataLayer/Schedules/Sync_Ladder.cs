using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_Ladder : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_Ladder_Syncing == false)
            {
                Sync_Base.Is_Ladder_Syncing = true;
                try
                {
                    UnitOfWork _context = new UnitOfWork();
                    SyncLog log = new SyncLog()
                    {
                        Name = "Ladder",
                        StartDatetime = DateTime.Now
                    };
                   
                    List<Product> listProduct = _context.Product.Where(p => p.LadderActive==true).ToList();
                    foreach (var item  in listProduct)
                    {
                        if (item.LadderDateExpire <= DateTime.Now)
                        {
                            item.LadderActive = false;
                            item.LadderDate = null;
                            item.LadderDateExpire = null;
                            _context.Product.Update(item);
                            _context.Save();
                        }
                          
                    }
                }
                catch (Exception ex) {
                    string msg = ex.Message;
                }
                Sync_Base.Is_Ladder_Syncing = false;
            }
        }
    }
}