using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class Trade
    {
        public Guid TradeId { get; set; }
        public DateTime TradeDate { get; set; }
        public string TradeType { get; set; }
        public int Qty { get; set; }
        public int BalanceQty { get; set; }
        public decimal Price { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string ScriptName { get; set; }
        public string PortfolioName { get; set; }
        public long OrderId { get; set; }
    }

}
