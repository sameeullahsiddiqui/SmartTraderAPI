using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;
using SmartTrader.Infrastructure.EFStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartTrader.Infrastructure.Repositories
{
    public class StockPriceRepository : BaseRepository<StockPrice>, IStockPriceRepository
    {
        public StockPriceRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<StockPrice> GetByName(string stockName, DateTime date)
        {
            var filterDate = date.AddMonths(-6).Date;
            return Find(x => x.Date > filterDate && x.SymbolName == stockName).OrderBy(x=>x.Date);
        }
    }
}
