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
    public class SectorStockViewRepository : BaseRepository<SectorStockView>, ISectorStockViewRepository
    {
        public SectorStockViewRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<SectorStockView> GetByDate(DateTime date)
        {
            return Find(x => x.Date > date).OrderBy(x => x.Date);
        }

        public IEnumerable<SectorStockView> GetBySectorName(DateTime date,string sectorName, int gainer)
        {
            var query = Find(x => x.Date == date && x.Sector == sectorName);
            //if (gainer > 0)
            //    query = query.Where(x => x.Percentage > 0);
            //else if (gainer < 0)
            //    query = query.Where(x => x.Percentage < 0);
            //else if (gainer == 0)
            //    query = query.Where(x => x.Percentage == 0);

            return query.OrderByDescending(x => x.Percentage);
        }

        public IEnumerable<SectorStockView> GetByIndustryName(DateTime date, string industryName, int gainer)
        {
            var query = Find(x => x.Date > date && x.Industry == industryName);
            //if (gainer > 0)
            //    query = query.Where(x => x.Percentage > 0);
            //else if (gainer < 0)
            //    query = query.Where(x => x.Percentage < 0);
            //else if (gainer == 0)
            //    query = query.Where(x => x.Percentage == 0);

            return query.OrderByDescending(x => x.Percentage);
        }

    }
}
