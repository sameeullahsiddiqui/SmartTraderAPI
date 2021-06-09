using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartTrader.Infrastructure.MigrationHelpers
{
    public static class ViewsHelper
    {
        public static void CreateDeliveryView(MigrationBuilder builder)
        {
            builder.Sql(@" CREATE VIEW Delivery AS
                            Select Date,SymbolName,Industry,Sector,VolRatio,DelRatio,Percentage,Weekly,Monthly, Last, TotalTradedQty,
                                  AvgVolume_30,Reason,DeliveryQty, symbols.Instrument
                            From AppStockPrices(nolock) prices 
                                inner join AppSymbols symbols on symbols.Code = prices.SymbolName
                            Where 1=1
                            and Sector not in ('Trading','Media','Funds','Financial','Ciggerettes','Bank','Alcohol','Breweries & Distilleries')");
        }

        public static void DropDeliveryView(MigrationBuilder builder)
        {
            builder.Sql("drop view Delivery");
        }


        public static void CreateIndustryView(MigrationBuilder builder)
        {
            builder.Sql(@" CREATE VIEW IndustryView AS
                            Select Date as Date,Industry,
                                SUM (CASE WHEN percentage > 0 THEN 1 ELSE 0 END) AS Gain,
                                SUM (CASE WHEN percentage < 0 THEN 1 ELSE 0 END) AS Loss,
                                SUM (CASE WHEN percentage = 0 THEN 1 ELSE 0 END) AS Nutral,
                                count(1) as Total
                            from AppStockPrices(nolock) prices
                            inner join AppSymbols symbols on symbols.Code = prices.SymbolName
                           group by Date,Industry");
        }

        public static void DropIndustryView(MigrationBuilder builder)
        {
            builder.Sql("drop view IndustryView");
        }

        public static void CreateSectorView(MigrationBuilder builder)
        {
            builder.Sql(@" CREATE VIEW SectorView AS
                            Select Date,Sector,
                                SUM (CASE WHEN percentage > 0 THEN 1 ELSE 0 END) AS Gain,
                                SUM (CASE WHEN percentage < 0 THEN 1 ELSE 0 END) AS Loss,
                                SUM (CASE WHEN percentage = 0 THEN 1 ELSE 0 END) AS Nutral,
                                count(1) as Total
                            from AppStockPrices(nolock) prices
                            inner join AppSymbols symbols on symbols.Code = prices.SymbolName
                            where Sector not in ('Trading','Media','Funds','Financial','Ciggerettes','Bank','Alcohol','Breweries & Distilleries')
                           group by Date,Sector");
        }

        public static void DropSectorView(MigrationBuilder builder)
        {
            builder.Sql("drop view SectorView");
        }

        public static void CreateSectorStockView(MigrationBuilder builder)
        {
            builder.Sql(@" CREATE VIEW SectorStockView AS
                            Select Date,Code,[Close],Percentage, Weekly, Monthly, MarketCap, TotalTradedQty, 
                                AvgVolume_30,Sector,Industry,Reason from AppStockPrices prices
                            Inner join AppSymbols symbols on symbols.Code = prices.SymbolName");
        }

        public static void DropSectorStockView(MigrationBuilder builder)
        {
            builder.Sql("drop view SectorStockView");
        }

    }
}
