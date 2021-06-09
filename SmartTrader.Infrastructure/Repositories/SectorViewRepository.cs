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
    public class SectorViewRepository : BaseRepository<SectorView>, ISectorViewRepository
    {
        public SectorViewRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<SectorView> GetByDate(DateTime date)
        {
            return Find(x => x.Date == date).Where(x=>x.Total>2);
        }

        public IEnumerable<SectorView> GetBySectorName(string sectorName)
        {
            return Find(x => x.Sector == sectorName).TakeLast(30).OrderBy(x => x.Date);
        }

    }
}
