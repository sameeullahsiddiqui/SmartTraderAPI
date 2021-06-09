using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Inerfaces
{
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        IEnumerable<Delivery> GetByDate(DateTime date, bool withDelivery);
    }
}
