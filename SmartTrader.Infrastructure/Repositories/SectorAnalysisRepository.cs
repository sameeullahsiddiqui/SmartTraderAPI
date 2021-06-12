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
    public class SectorAnalysisRepository : BaseRepository<SectorAnalysis>, ISectorAnalysisRepository
    {
        public SectorAnalysisRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<SectorAnalysis> GetByDate(DateTime date)
        {
            return Find(x => x.Date == date);
        }

        public IEnumerable<SectorAnalysis> GetBySectorName(string sectorName)
        {
            return Find(x => x.Sector == sectorName).OrderBy(x => x.Date).TakeLast(100);
        }

    }
}
