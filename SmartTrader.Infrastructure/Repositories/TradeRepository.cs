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
    public class TradeRepository : BaseRepository<Trade>, ITradeRepository
    {
        public TradeRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<Trade> GetByDate(DateTime date)
        {
            var result = Find(x => x.TradeDate == date);
            return result;
        }


        public IEnumerable<Trade> GetBySymbol(string symbol)
        {
            var result = Find(x => x.ScriptName == symbol);
            return result;
        }
    }
}
