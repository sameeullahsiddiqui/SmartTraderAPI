using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartTrader.Core.Inerfaces
{
    public interface ISectorAnalysisRepository : IRepository<SectorAnalysis>
    {
        IEnumerable<SectorAnalysis> GetByDate(DateTime date);
        
        IEnumerable<SectorAnalysis> GetBySectorName(string sectorName);
    }
}
