using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrader.Core.Models {

    public class IndustryView {

        public DateTime Date { get; set; }
        public string Industry { get; set; }

        public int Gain { get; set; }

        public int Loss { get; set; }
        public int Nutral { get; set; }

        public int Total { get; set; }

        [NotMapped]
        public double GainerRatio  { get { return  Math.Round((double)(Gain / Total) * 100,2); } }
    }

}