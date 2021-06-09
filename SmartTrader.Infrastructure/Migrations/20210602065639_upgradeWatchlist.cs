using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartTrader.Infrastructure.Migrations
{
    public partial class upgradeWatchlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ChangeSinceAdded",
                table: "WatchList",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentPrice",
                table: "WatchList",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "WatchList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeSinceAdded",
                table: "WatchList");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "WatchList");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "WatchList");
        }
    }
}
