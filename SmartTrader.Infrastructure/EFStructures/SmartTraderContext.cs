using Microsoft.EntityFrameworkCore;
using SmartTrader.Core.Inerfaces;
using SmartTrader.Core.Models;

namespace SmartTrader.Infrastructure.EFStructures
{
    public class SmartTraderContext : DbContext, ISmartTraderContext
    {

        public DbSet<SectorAnalysis> SectorAnalysis { get; set; }
        public DbSet<SectorStockView> SectorStockViews { get; set; }
        public DbSet<IndustryView> IndustryViews { get; set; }
        public DbSet<Delivery> deliverys { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<EarningReport> EarningReports { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<BackgroundJob> BackgroundJobs { get; set; }
        public DbSet<FileCategory> FileCategories { get; set; }

        public DbSet<SuperstarPortfolio> SuperstarPortfolios { get; set; }

        public SmartTraderContext(DbContextOptions<SmartTraderContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SectorAnalysis>(e =>
            {
                e.ToTable("SectorAnalysis");
                e.Property(x => x.Day0Color).HasColumnType("nVarchar(10)");
                e.Property(x => x.Day1Color).HasColumnType("nVarchar(10)");
                e.Property(x => x.Day2Color).HasColumnType("nVarchar(10)");
                e.Property(x => x.Day3Color).HasColumnType("nVarchar(10)");
                e.Property(x => x.Day4Color).HasColumnType("nVarchar(10)");
                e.Property(x => x.Day5Color).HasColumnType("nVarchar(10)");
                e.Property(c => c.Sector).HasColumnType("nVarchar(100)").HasMaxLength(100);
            });

            builder.Entity<SectorStockView>(e =>
            {
                e.ToView("SectorStockView").HasNoKey();
                e.Property(x => x.Sector).HasColumnType("Varchar(500)");
            });

            builder.Entity<IndustryView>().ToView("IndustryView").HasNoKey();

            builder.Entity<Delivery>().ToView("Delivery").HasNoKey();

            builder.Entity<Symbol>().ToTable("AppSymbols");

            builder.Entity<StockPrice>(e =>
            {
                e.ToTable("AppStockPrices");
                e.Property(x => x.SymbolName).HasColumnType("Varchar(500)");
                e.Property(c => c.Tooltip).HasColumnType("nVarchar(1000)").HasMaxLength(1000);
            });
                
            builder.Entity<WatchList>(e =>
            {
                e.ToTable("WatchList");
                e.Property(x => x.Symbol).HasColumnType("Varchar(500)");
                e.Property(x => x.Description).HasColumnType("Varchar(500)");
                e.Property(x => x.ReasonToWatch).HasColumnType("Varchar(1000)");
                e.Property(x => x.Status).HasColumnType("Varchar(100)");
            });


            builder.Entity<SuperstarPortfolio>(e =>
            {
                e.ToTable("SuperstarPortfolio");
                e.Property(x => x.Symbol).HasColumnType("Varchar(500)");
                e.Property(x => x.InvestorName).HasColumnType("Varchar(500)");
                e.Property(x => x.ReasonToWatch).HasColumnType("Varchar(1000)");
                e.Property(x => x.Status).HasColumnType("Varchar(100)");
            });

            builder.Entity<EarningReport>(e =>
            {
                e.ToTable("AppEarningReports");
                e.Property(x => x.Company).HasColumnType("Varchar(500)");
                e.Property(x => x.CurrentQuarter).HasColumnType("Varchar(100)");
            });

            builder.Entity<Trade>(e =>
            {
                e.ToTable("AppTrades");
                e.Property(x => x.TradeType).HasColumnType("nVarchar(100)");
                e.Property(x => x.ScriptName).HasColumnType("nVarchar(500)");
                e.Property(x => x.PortfolioName).HasColumnType("nVarchar(100)");
            });

            builder.Entity<Portfolio>(e =>
            {
                e.ToTable("AppPortfolios");
                e.Property(x => x.TradeType).HasColumnType("nVarchar(100)");
                e.Property(x => x.BuyComment).HasColumnType("nVarchar(500)");
                e.Property(x => x.SellComment).HasColumnType("nVarchar(500)");
                e.Property(x => x.Status).HasColumnType("nVarchar(100)");
                e.Property(x => x.SymbolName).HasColumnType("nVarchar(500)");
                e.Property(x => x.PortfolioName).HasColumnType("nVarchar(100)");
            });


            builder.Entity<BackgroundJob>(e =>
            {
                e.ToTable("AppBackgroundJobs");
            });

            builder.Entity<FileCategory>(e =>
            {
                e.ToTable("AppFileCategories");
            });


        }

    }

}