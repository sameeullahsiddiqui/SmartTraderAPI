using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class EarningReport
    {
        public int EarningReportId { get; set; }
        public double YoYNetProfit_Q1 { get; set; }
        public double YoYGrossProfit_Q1 { get; set; }
        public double YoYSales_Q1 { get; set; }

        public double QoQNetProfit_Q1 { get; set; }
        public double QoQGrossProfit_Q1 { get; set; }
        public double QoQSales_Q1 { get; set; }
        public DateTime Date { get; set; }
        public string Company { get; set; }
        public int Year { get; set; }
        public string CurrentQuarter { get; set; }
        public decimal EarningDayPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal ChangeSinceReport { get; set; }
        public bool isMissingCompany { get; set; }
    }
}
