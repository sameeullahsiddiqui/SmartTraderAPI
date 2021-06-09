using Microsoft.EntityFrameworkCore.Migrations;
using SmartTrader.Infrastructure.MigrationHelpers;

namespace SmartTrader.Infrastructure.Migrations
{
    public partial class AddSectorViews2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ViewsHelper.CreateIndustryView(migrationBuilder);
            ViewsHelper.CreateSectorView(migrationBuilder);
            ViewsHelper.CreateSectorStockView(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ViewsHelper.DropIndustryView(migrationBuilder);
            ViewsHelper.DropSectorView(migrationBuilder);
            ViewsHelper.DropSectorStockView(migrationBuilder);
        }
    }
}
