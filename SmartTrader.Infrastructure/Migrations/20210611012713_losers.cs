using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartTrader.Infrastructure.Migrations
{
    public partial class losers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Losser",
                table: "SectorAnalysis",
                newName: "Loser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Loser",
                table: "SectorAnalysis",
                newName: "Losser");
        }
    }
}
