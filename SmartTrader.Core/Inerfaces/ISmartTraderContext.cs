using Microsoft.EntityFrameworkCore;
using SmartTrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTrader.Core.Inerfaces
{
    public interface ISmartTraderContext
    {
         DbSet<SectorAnalysis> SectorAnalysis { get; set; }
         DbSet<SectorStockView> SectorStockViews { get; set; }
         DbSet<IndustryView> IndustryViews { get; set; }
         DbSet<Delivery> Deliverys { get; set; }
         DbSet<StockPrice> StockPrices { get; set; }
         DbSet<Symbol> Symbols { get; set; }
         DbSet<WatchList> WatchLists { get; set; }
         DbSet<EarningReport> EarningReports { get; set; }
         DbSet<Trade> Trades { get; set; }
         DbSet<Portfolio> Portfolios { get; set; }
         DbSet<BackgroundJob> BackgroundJobs { get; set; }
         DbSet<FileCategory> FileCategories { get; set; }
         DbSet<SuperstarPortfolio> SuperstarPortfolios { get; set; }

        int SaveChanges();
    }
}
