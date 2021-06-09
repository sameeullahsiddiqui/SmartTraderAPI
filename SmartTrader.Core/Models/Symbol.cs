using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class Symbol
    {

        public int SymbolId { get; set; }
        public string Code { get; set; }
        public string Ticker { get; set; }
        public string Instrument { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public float Facevalue { get; set; }
        public string Isin { get; set; }
        public string Sector { get; set; }
        public string Currency { get; set; }
        public bool IntradaySelected { get; set; }
        public bool? Flag { get; set; }

        public bool? isNifty50 { get; set; }
        public bool? isNiftyNext50 { get; set; }
        public bool? isNifty100 { get; set; }
        public bool? isNifty200 { get; set; }

        public bool isShariaComplience { get; set; }
        public string Industry { get; set; }
        public decimal Debt { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal Interest { get; set; }
        public decimal NetProfit { get; set; }
        public decimal CashByMarketCap { get; set; }

        public decimal Margin { get; set; }

        public ICollection<StockPrice> StockPrices { get; set; } = new List<StockPrice>();
        public decimal MarketCap { get; set; }
        public decimal TradeReceivables { get; set; }
        public decimal Sales { get; set; }
        public decimal CashEndOfLastYear { get; set; }
        public uint InstrumentToken { get; set; }
    }
}
