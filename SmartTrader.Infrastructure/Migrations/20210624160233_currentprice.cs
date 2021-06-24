using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartTrader.Infrastructure.Migrations
{
    public partial class currentprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "AppEarningReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "AppEarningReports");
        }
    }
}
