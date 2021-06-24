using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartTrader.Infrastructure.Migrations
{
    public partial class earningv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ChangeSinceReport",
                table: "AppEarningReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EarningDayPrice",
                table: "AppEarningReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Q1Date",
                table: "AppEarningReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Q2Date",
                table: "AppEarningReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Q3Date",
                table: "AppEarningReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Q4Date",
                table: "AppEarningReports",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeSinceReport",
                table: "AppEarningReports");

            migrationBuilder.DropColumn(
                name: "EarningDayPrice",
                table: "AppEarningReports");

            migrationBuilder.DropColumn(
                name: "Q1Date",
                table: "AppEarningReports");

            migrationBuilder.DropColumn(
                name: "Q2Date",
                table: "AppEarningReports");

            migrationBuilder.DropColumn(
                name: "Q3Date",
                table: "AppEarningReports");

            migrationBuilder.DropColumn(
                name: "Q4Date",
                table: "AppEarningReports");
        }
    }
}
