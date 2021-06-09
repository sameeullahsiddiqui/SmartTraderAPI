using Microsoft.EntityFrameworkCore;
using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Inerfaces
{
    public interface ISmartTraderContext
    {
        DbSet<SectorView> SectorViews { get; set; }
        DbSet<SectorStockView> SectorStockViews { get; set; }
        DbSet<IndustryView> IndustryViews { get; set; }
        DbSet<Delivery> deliverys { get; set; }
        DbSet<StockPrice> StockPrices { get; set; }
        DbSet<Symbol> Symbols { get; set; }
        DbSet<WatchList> WatchLists { get; set; }

        int SaveChanges();
    }
}
