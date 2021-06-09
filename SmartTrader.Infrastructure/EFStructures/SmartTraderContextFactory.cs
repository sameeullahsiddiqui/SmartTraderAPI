using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace   SmartTrader.Infrastructure.EFStructures {
public class SmartTraderContextFactory : IDesignTimeDbContextFactory<SmartTraderContext>
{
    public SmartTraderContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SmartTraderContext>();
            optionsBuilder.UseSqlServer("Server=LAPTOP-IGUMURQO;Database=StockPortal;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new SmartTraderContext(optionsBuilder.Options);
    }
}
}