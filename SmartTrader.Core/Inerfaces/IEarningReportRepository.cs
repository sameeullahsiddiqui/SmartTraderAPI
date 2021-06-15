using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartTrader.Core.Inerfaces
{
    public interface IEarningReportRepository : IRepository<EarningReport>
    {
        IEnumerable<EarningReport> GetByDate(DateTime date);

        IEnumerable<EarningReport> GetByCompanyName(string company, int year);
    }
}
