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
    public class SymbolRepository : BaseRepository<Symbol>, ISymbolRepository
    {
        public SymbolRepository(SmartTraderContext context) : base(context)
        { }

    }
}
