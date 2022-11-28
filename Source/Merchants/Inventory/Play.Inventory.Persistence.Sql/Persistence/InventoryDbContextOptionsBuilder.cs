using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.Inventory.Persistence.Sql.Persistence;

internal class InventoryDbContextOptionsBuilder : IDesignTimeDbContextFactory<InventoryDbContext>
{
    #region Constructor

    #endregion

    #region Instance Members

    public InventoryDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        string connectionString = configuration.GetConnectionString(InventoryDbContext.DatabaseName);
        DbContextOptionsBuilder<InventoryDbContext> builder = new DbContextOptionsBuilder<InventoryDbContext>();
        builder.UseSqlServer(connectionString);

        return new InventoryDbContext(builder.Options);
    }

    #endregion
}