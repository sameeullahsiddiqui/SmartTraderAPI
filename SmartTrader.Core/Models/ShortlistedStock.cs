using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class ShortlistedStock
    {
        public int ShortlistedStockId { get; set; }
        public string SymbolName { get; set; }
        public DateTime ShortlistDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public decimal Price { get; set; }
        public decimal Percentage { get; set; }
        public decimal Monthly { get; set; }
        public decimal Weekly { get; set; }
        public decimal DelRatio { get; set; }
        public string StrategyName { get; set; }
        public bool IsActive { get; set; }
    }
}
