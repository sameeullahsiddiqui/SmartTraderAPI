using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartTrader.Core.Inerfaces
{
    public interface ISectorViewRepository : IRepository<SectorView>
    {
        IEnumerable<SectorView> GetByDate(DateTime date);
        
        IEnumerable<SectorView> GetBySectorName(string sectorName);
    }
}
