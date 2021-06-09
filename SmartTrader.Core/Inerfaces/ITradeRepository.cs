using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Inerfaces
{
    public interface ITradeRepository : IRepository<Trade>
    {
        IEnumerable<Trade> GetByDate(DateTime date);
        IEnumerable<Trade> GetBySymbol(string symbol);
    }
}
