using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;
using SmartTrader.Infrastructure.EFStructures;
using System.Collections.Generic;
using System.Linq;

namespace SmartTrader.Infrastructure.Repositories
{
    public class SuperstarPortfolioRepository : BaseRepository<SuperstarPortfolio>, ISuperstarPortfolioRepository
    {
        public SuperstarPortfolioRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<SuperstarPortfolio> GetAll(bool activeOnly)
        {
            var result = GetAll();

            if (activeOnly)
                result = result.Where(x => x.Status == "Open").OrderByDescending(x => x.Date);

            return result;
        }
    }
}
