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
        DbContextOptionsBuilder<InventoryDbContext> builder = new();
        builder.UseSqlServer(connectionString);

        return new(builder.Options);
    }

    #endregion
}