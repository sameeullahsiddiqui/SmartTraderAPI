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
    public class EarningRepository : BaseRepository<EarningReport>, IEarningRepository
    {
        public EarningRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<EarningReport> GetByName(string company, int year)
        {
            var result = Find(x => x.Company == company && x.Year == year);
            return result;
        }
    }
}
