using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class StockPrice
    {
        public Guid StockPriceId { get; set; }
        public DateTime Date { get; set; }
        public string SymbolName { get; set; }
        public int SymbolId { get; set; }
        public virtual Symbol Symbol { get; set; }
        public string Series { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Last { get; set; }
        public decimal PrevClose { get; set; }
        public decimal Percentage { get; set; }
        public int TotalTradedQty { get; set; }
        public decimal AvgVolume_30 { get; set; }
        public decimal VolRatio { get; set; }
        public decimal DeliveryQty { get; set; }
        public decimal AvgDelivery_30 { get; set; }
        public decimal DelRatio { get; set; }
        public decimal Monthly { get; set; }
        public decimal Weekly { get; set; }
        public string Reason { get; set; }
        public bool IsFlaged { get; set; }
        public string Tooltip { get; set; }

        public decimal Q1High { get; set; }
        public decimal Q2High { get; set; }
        public decimal Q3High { get; set; }
        public decimal Q4High { get; set; }

        public decimal Q1Low { get; set; }
        public decimal Q2Low { get; set; }
        public decimal Q3Low { get; set; }
        public decimal Q4Low { get; set; }

    }
}
