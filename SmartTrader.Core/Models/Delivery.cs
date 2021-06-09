using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class Delivery
    {
        public DateTime Date { get; set; }
        public string SymbolName { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }
        public decimal VolRatio { get; set; }
        public decimal DelRatio { get; set; }
        public decimal Percentage { get; set; }
        public decimal Weekly { get; set; }
        public decimal Monthly { get; set; }
        public decimal Last { get; set; }
        public int TotalTradedQty { get; set; }
        public decimal AvgVolume_30 { get; set; }
        public string Reason { get; set; }
        public decimal DeliveryQty { get; set; }
        public string Instrument { get; set; }
    }
}
