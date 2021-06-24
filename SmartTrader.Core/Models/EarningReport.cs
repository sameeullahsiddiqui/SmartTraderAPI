using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class EarningReport
    {
        public int EarningReportId { get; set; }
        public double YoYNetProfit_Q4 { get; set; }
        public double YoYNetProfit_Q3 { get; set; }
        public double YoYNetProfit_Q2 { get; set; }
        public double YoYNetProfit_Q1 { get; set; }
        public double YoYGrossProfit_Q4 { get; set; }
        public double YoYGrossProfit_Q3 { get; set; }
        public double YoYGrossProfit_Q2 { get; set; }
        public double YoYGrossProfit_Q1 { get; set; }
        public double YoYSales_Q4 { get; set; }
        public double YoYSales_Q3 { get; set; }
        public double YoYSales_Q2 { get; set; }
        public double YoYSales_Q1 { get; set; }

        public double QoQNetProfit_Q4 { get; set; }
        public double QoQNetProfit_Q3 { get; set; }
        public double QoQNetProfit_Q2 { get; set; }
        public double QoQNetProfit_Q1 { get; set; }
        public double QoQGrossProfit_Q4 { get; set; }
        public double QoQGrossProfit_Q3 { get; set; }
        public double QoQGrossProfit_Q2 { get; set; }
        public double QoQGrossProfit_Q1 { get; set; }
        public double QoQSales_Q4 { get; set; }
        public double QoQSales_Q3 { get; set; }
        public double QoQSales_Q2 { get; set; }
        public double QoQSales_Q1 { get; set; }
        public DateTime Date { get; set; }
        public string Company { get; set; }
        public int Year { get; set; }
        public string CurrentQuarter { get; set; }
        public decimal EarningDayPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal ChangeSinceReport { get; set; }
        public DateTime? Q1Date { get; set; }
        public DateTime? Q2Date { get; set; }
        public DateTime? Q3Date { get; set; }
        public DateTime? Q4Date { get; set; }
        public bool isMissingCompany { get; set; }
    }
}
