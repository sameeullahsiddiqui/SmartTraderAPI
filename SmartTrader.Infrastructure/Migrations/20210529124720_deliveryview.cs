using Microsoft.EntityFrameworkCore.Migrations;
using SmartTrader.Infrastructure.MigrationHelpers;

namespace SmartTrader.Infrastructure.Migrations
{
    public partial class deliveryview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ViewsHelper.CreateDeliveryView(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ViewsHelper.DropDeliveryView(migrationBuilder);
        }
    }
}
