using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;
using SmartTrader.Infrastructure.EFStructures;
using System.Collections.Generic;
using System.Linq;

namespace SmartTrader.Infrastructure.Repositories
{
    public class WatchListRepository : BaseRepository<WatchList>, IWatchListRepository
    {
        public WatchListRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<WatchList> GetAll(bool activeOnly)
        {
            var result = GetAll();

            if (activeOnly)
                result = result.Where(x => x.Status == "Open").OrderByDescending(x => x.Date);

            return result;
        }
    }
}
