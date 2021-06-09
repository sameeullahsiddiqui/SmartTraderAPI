﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartTrader.Infrastructure.EFStructures;

namespace SmartTrader.Infrastructure.Migrations
{
    [DbContext(typeof(SmartTraderContext))]
    [Migration("20210602012650_watchlists")]
    partial class watchlists
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartTrader.Core.Models.Delivery", b =>
                {
                    b.Property<decimal>("AvgVolume_30")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DelRatio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DeliveryQty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instrument")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Last")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Monthly")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sector")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SymbolName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalTradedQty")
                        .HasColumnType("int");

                    b.Property<decimal>("VolRatio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Weekly")
                        .HasColumnType("decimal(18,2)");

                    b.ToView("Delivery");
                });

            modelBuilder.Entity("SmartTrader.Core.Models.IndustryView", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gain")
                        .HasColumnType("int");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Loss")
                        .HasColumnType("int");

                    b.Property<int>("Nutral")
                        .HasColumnType("int");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.ToView("IndustryView");
                });

            modelBuilder.Entity("SmartTrader.Core.Models.SectorStockView", b =>
                {
                    b.Property<decimal>("AvgVolume_30")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Close")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MarketCap")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Monthly")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sector")
                        .HasColumnType("Varchar(500)");

                    b.Property<int>("TotalTradedQty")
                        .HasColumnType("int");

                    b.Property<decimal>("Weekly")
                        .HasColumnType("decimal(18,2)");

                    b.ToView("SectorStockView");
                });

            modelBuilder.Entity("SmartTrader.Core.Models.SectorView", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gain")
                        .HasColumnType("int");

                    b.Property<int>("Loss")
                        .HasColumnType("int");

                    b.Property<int>("Nutral")
                        .HasColumnType("int");

                    b.Property<string>("Sector")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.ToView("SectorView");
                });

            modelBuilder.Entity("SmartTrader.Core.Models.StockPrice", b =>
                {
                    b.Property<Guid>("StockPriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("AvgDelivery_30")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AvgVolume_30")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Close")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DelRatio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DeliveryQty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsFlaged")
                        .HasColumnType("bit");

                    b.Property<decimal>("Last")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Monthly")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Open")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrevClose")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Series")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SymbolId")
                        .HasColumnType("int");

                    b.Property<string>("SymbolName")
                        .HasColumnType("Varchar(500)");

                    b.Property<int>("TotalTradedQty")
                        .HasColumnType("int");

                    b.Property<decimal>("VolRatio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Weekly")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("StockPriceId");

                    b.HasIndex("SymbolId");

                    b.ToTable("AppStockPrices");
                });

            modelBuilder.Entity("SmartTrader.Core.Models.Symbol", b =>
                {
                    b.Property<int>("SymbolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("CashByMarketCap")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CashEndOfLastYear")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Debt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<float>("Facevalue")
                        .HasColumnType("real");

                    b.Property<bool?>("Flag")
                        .HasColumnType("bit");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instrument")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("InstrumentToken")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Interest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IntradaySelected")
                        .HasColumnType("bit");

                    b.Property<string>("Isin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Margin")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MarketCap")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("NetProfit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Sales")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Sector")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Ticker")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAssets")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TradeReceivables")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool?>("isNifty100")
                        .HasColumnType("bit");

                    b.Property<bool?>("isNifty200")
                        .HasColumnType("bit");

                    b.Property<bool?>("isNifty50")
                        .HasColumnType("bit");

                    b.Property<bool?>("isNiftyNext50")
                        .HasColumnType("bit");

                    b.Property<bool>("isShariaComplience")
                        .HasColumnType("bit");

                    b.HasKey("SymbolId");

                    b.ToTable("AppSymbols");
                });

            modelBuilder.Entity("SmartTrader.Core.Models.WatchList", b =>
                {
                    b.Property<int>("WatchListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("Varchar(500)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ReasonToWatch")
                        .HasColumnType("Varchar(1000)");

                    b.Property<string>("Status")
                        .HasColumnType("Varchar(100)");

                    b.Property<string>("Symbol")
                        .HasColumnType("Varchar(500)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("WatchListId");

                    b.ToTable("WatchList");
                });

            modelBuilder.Entity("SmartTrader.Core.Models.StockPrice", b =>
                {
                    b.HasOne("SmartTrader.Core.Models.Symbol", "Symbol")
                        .WithMany("StockPrices")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Symbol");
                });

            modelBuilder.Entity("SmartTrader.Core.Models.Symbol", b =>
                {
                    b.Navigation("StockPrices");
                });
#pragma warning restore 612, 618
        }
    }
}
