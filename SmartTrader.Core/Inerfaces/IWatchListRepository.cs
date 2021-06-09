using SmartTrader.Core.Models;
using System.Collections.Generic;

namespace SmartTrader.Core.Inerfaces
{
    public interface IWatchListRepository : IRepository<WatchList>
    {
        IEnumerable<WatchList> GetAll(bool activeOnly);
    }
}
