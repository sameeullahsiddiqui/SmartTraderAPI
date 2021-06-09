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
    public class PortfolioRepository : BaseRepository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<Portfolio> GetByStatus(string status)
        {
            var result = Find(x => x.Status == status);
            return result;
        }
    }
}
