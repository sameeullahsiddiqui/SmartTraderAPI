using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Models
{
    public class Portfolio
    {
        public Guid PortfolioId { get; set; }
        public string PortfolioName { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? RecommenationDate { get; set; }

        public string SymbolName { get; set; }

        public int SymbolId { get; set; }
        public virtual Symbol Symbol { get; set; }
        public string TradeType { get; set; }

        public int Quantity { get; set; }
        public DateTime? BuyDate { get; set; }
        public decimal? BuyPrice { get; set; }
        public decimal? BuyCommission { get; set; }
        public decimal? TargetPrice { get; set; }
        public decimal? StopLossPrice { get; set; }
        public decimal? BreakEvenPrice { get; set; }

        public decimal? BuyOpen { get; set; }
        public decimal? BuyHigh { get; set; }
        public decimal? BuyLow { get; set; }
        public decimal? BuyClose { get; set; }

        public decimal? BuyDayReturn { get; set; }
        public decimal Buy_RSI_14 { get; set; }
        public decimal? BuyNiftyReturn { get; set; }
        public string BuyComment { get; set; }

        public decimal? BuyGrade { get; set; }
        public DateTime? BuyExecutionTime { get; set; }
        public decimal AllowedRiskOnBuyDay { get; set; }

        public DateTime? SellDate { get; set; }
        public decimal? SellPrice { get; set; }
        public decimal? SellCommission { get; set; }

        public decimal? SellOpen { get; set; }
        public decimal? SellHigh { get; set; }
        public decimal? SellLow { get; set; }
        public decimal? SellClose { get; set; }
        public decimal? SellDayReturn { get; set; }
        public decimal? Sell_RSI_14 { get; set; }
        public string SellComment { get; set; }
        public decimal? SellNiftyReturn { get; set; }
        public decimal? SellGrade { get; set; }
        public DateTime? SellExecutionTime { get; set; }

        public decimal? TradeGrade { get; set; }
        public string Status { get; set; }
        public decimal? CurrentProfit { get; set; }
        public decimal? ProfitPercent { get; set; }
        public decimal? HoldingProfit { get; set; }

        public int TradeDays { get; set; }
        public string TradeBuyDay { get; set; }
        public string TradeSellDay { get; set; }

        public Portfolio DeepCopy()
        {
            Portfolio othercopy = (Portfolio)this.MemberwiseClone();
            othercopy.PortfolioId = Guid.NewGuid();

            return othercopy;
        }
    }
}
