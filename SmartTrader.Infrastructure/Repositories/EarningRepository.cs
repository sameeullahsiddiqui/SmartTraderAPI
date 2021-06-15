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
    public class EarningReportRepository : BaseRepository<EarningReport>, IEarningReportRepository
    {
        public EarningReportRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<EarningReport> GetByDate(DateTime date)
        {
            var result = Find(x => x.Date == date);
            return result;
        }

        public IEnumerable<EarningReport> GetByCompanyName(string company, int year)
        {
            var result = Find(x => x.Company == company && x.Year == year);
            return result;
        }
    }
}
