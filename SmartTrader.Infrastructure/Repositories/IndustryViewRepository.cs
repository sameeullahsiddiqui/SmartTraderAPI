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
    public class IndustryViewRepository : BaseRepository<IndustryView>, IIndustryViewRepository
    {
        public IndustryViewRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<IndustryView> GetByDate(DateTime date)
        {
            return Find(x => x.Date == date);
        }

        public IEnumerable<IndustryView> GetByIndustryName(string industryName)
        {
            return Find(x => x.Industry == industryName).TakeLast(200).OrderByDescending(x => x.Date);
        }

    }
}
