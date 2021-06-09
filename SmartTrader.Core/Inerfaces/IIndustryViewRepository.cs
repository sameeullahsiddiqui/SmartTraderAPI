using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartTrader.Core.Inerfaces
{
    public interface IIndustryViewRepository : IRepository<IndustryView>
    {
        IEnumerable<IndustryView> GetByDate(DateTime date);
        
        IEnumerable<IndustryView> GetByIndustryName(string industryName);
    }
}
