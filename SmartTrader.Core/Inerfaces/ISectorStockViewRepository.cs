using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartTrader.Core.Inerfaces
{
    public interface ISectorStockViewRepository : IRepository<SectorStockView>
    {
        IEnumerable<SectorStockView> GetByDate(DateTime date);

        IEnumerable<SectorStockView> GetBySectorName(DateTime date, string sectorName, int gainer);

        IEnumerable<SectorStockView> GetByIndustryName(DateTime date, string industryName, int gainer);
    }
}
