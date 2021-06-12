using System;

namespace SmartTrader.Core.Models
{
    public class SuperstarPortfolio
    {
        public int SuperstarPortfolioId { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public string InvestorName { get; set; }
        public string ReasonToWatch { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public DateTime UpdateTime { get; set; }
        public double CurrentPrice { get; set; }
        public double ChangeSinceAdded { get; set; }
        public int Days { get; set; }
    }
}
