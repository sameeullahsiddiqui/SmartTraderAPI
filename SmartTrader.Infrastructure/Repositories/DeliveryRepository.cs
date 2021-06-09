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
    public class DeliveryRepository : BaseRepository<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(SmartTraderContext context) : base(context)
        { }

        public IEnumerable<Delivery> GetByDate(DateTime date, bool withDelivery)
        {
            var result = Find(x => x.Date == date);
            if (withDelivery)
                result = result.Where(x => x.DelRatio > 2).OrderByDescending(x => x.DelRatio);

            return result;
        }
    }
}
