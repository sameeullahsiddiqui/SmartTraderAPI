using SmartTrader.Core.Models;
using System.Collections.Generic;

namespace SmartTrader.Core.Inerfaces
{
    public interface ISuperstarPortfolioRepository : IRepository<SuperstarPortfolio>
    {
        IEnumerable<SuperstarPortfolio> GetAll(bool activeOnly);
    }
}
