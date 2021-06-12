using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartTrader.Infrastructure.Migrations
{
    public partial class sectorAnalysis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SectorAnalysis",
                columns: table => new
                {
                    SectorAnalysisId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sector = table.Column<string>(type: "nVarchar(100)", maxLength: 100, nullable: true),
                    Gainers = table.Column<int>(type: "int", nullable: false),
                    Loser = table.Column<int>(type: "int", nullable: false),
                    Nutral = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Day1Gainer = table.Column<int>(type: "int", nullable: false),
                    Day2Gainer = table.Column<int>(type: "int", nullable: false),
                    Day3Gainer = table.Column<int>(type: "int", nullable: false),
                    Day4Gainer = table.Column<int>(type: "int", nullable: false),
                    Day5Gainer = table.Column<int>(type: "int", nullable: false),
                    Day1Color = table.Column<string>(type: "nVarchar(10)", nullable: true),
                    Day2Color = table.Column<string>(type: "nVarchar(10)", nullable: true),
                    Day3Color = table.Column<string>(type: "nVarchar(10)", nullable: true),
                    Day4Color = table.Column<string>(type: "nVarchar(10)", nullable: true),
                    Day5Color = table.Column<string>(type: "nVarchar(10)", nullable: true),
                    GainerRatio = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorAnalysis", x => x.SectorAnalysisId);
                });

            migrationBuilder.CreateTable(
                name: "SuperstarPortfolio",
                columns: table => new
                {
                    SuperstarPortfolioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "Varchar(500)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvestorName = table.Column<string>(type: "Varchar(500)", nullable: true),
                    ReasonToWatch = table.Column<string>(type: "Varchar(1000)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "Varchar(100)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentPrice = table.Column<double>(type: "float", nullable: false),
                    ChangeSinceAdded = table.Column<double>(type: "float", nullable: false),
                    Days = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperstarPortfolio", x => x.SuperstarPortfolioId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SectorAnalysis");

            migrationBuilder.DropTable(
                name: "SuperstarPortfolio");
        }
    }
}
