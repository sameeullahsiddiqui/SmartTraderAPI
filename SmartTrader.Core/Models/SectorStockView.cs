using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class SectorStockView
    {
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public decimal Close { get; set; }
        public decimal Percentage { get; set; }
        public decimal Monthly { get; set; }
        public decimal Weekly { get; set; }
        public decimal MarketCap { get; set; }
        public int TotalTradedQty { get; set; }
        public decimal AvgVolume_30 { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Reason { get; set; }
    }
}
