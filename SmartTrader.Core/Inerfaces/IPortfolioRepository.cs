using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Inerfaces
{
    public interface IPortfolioRepository : IRepository<Portfolio>
    {
        IEnumerable<Portfolio> GetByStatus(string status);
    }
}
