using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartTrader.Infrastructure.Migrations
{
    public partial class ShortlistedStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortlistedStock",
                columns: table => new
                {
                    ShortlistedStockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SymbolName = table.Column<string>(type: "Varchar(500)", nullable: true),
                    ShortlistDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Monthly = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weekly = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DelRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StrategyName = table.Column<string>(type: "Varchar(500)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortlistedStock", x => x.ShortlistedStockId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortlistedStock");
        }
    }
}
