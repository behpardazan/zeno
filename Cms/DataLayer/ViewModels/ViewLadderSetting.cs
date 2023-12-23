using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewLadderSetting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Day { get; set; }
        public double? Price { get; set; }

        public ViewLadderSetting() { }

        public ViewLadderSetting(LadderSetting ladder)
        {
            this.Id = ladder.ID;
            this.Name = ladder.Name;
            this.Description = ladder.Description;
            this.Day = ladder.LadderDays;
            this.Price = ladder.LadderPrice;
        }
    }
}
