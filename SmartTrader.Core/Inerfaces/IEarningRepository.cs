using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Inerfaces
{
    public interface IEarningRepository : IRepository<EarningReport>
    {
        IEnumerable<EarningReport> GetByName(string stockName, int year);
    }
}
