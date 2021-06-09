using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Inerfaces
{
    public interface IStockPriceRepository : IRepository<StockPrice>
    {
        IEnumerable<StockPrice> GetByName(string stockName, DateTime date);
    }
}
