using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrader.Core.Models {

    public class SectorAnalysis {

        public int SectorAnalysisId { get; set; }

        public DateTime Date { get; set; }

        public string Sector { get; set; }

        public int Gainers { get; set; }

        public int Loser { get; set; }

        public int Nutral { get; set; }

        public int Total { get; set; }

        public int Score { get; set; }

        public int? Day1Gainer { get; set; }
        public int? Day2Gainer { get; set; }
        public int? Day3Gainer { get; set; }
        public int? Day4Gainer { get; set; }
        public int? Day5Gainer { get; set; }

        public string Day0Color { get; set; }
        public string Day1Color { get; set; }
        public string Day2Color { get; set; }
        public string Day3Color { get; set; }
        public string Day4Color { get; set; }
        public string Day5Color { get; set; }

        public double GainerRatio { get; set; }

        //[NotMapped]
        //public double GainerRatio  { get { return  Math.Round((double)(Gainers / Total) * 100,2); } }
    }

}