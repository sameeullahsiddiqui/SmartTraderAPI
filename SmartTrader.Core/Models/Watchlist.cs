using System;

namespace SmartTrader.Core.Models
{
    public class WatchList
    {
        public int WatchListId { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ReasonToWatch { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public DateTime UpdateTime { get; set; }
        public double CurrentPrice { get; set; }
        public double ChangeSinceAdded { get; set; }
        public int Days { get; set; }
    }
}
