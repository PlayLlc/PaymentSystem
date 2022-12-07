using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.Loyalty.Persistence.Sql.Persistence;

internal class LoyaltyDbContextOptionsBuilder : IDesignTimeDbContextFactory<LoyaltyDbContext>
{
    #region Constructor

    #endregion

    #region Instance Members

    public LoyaltyDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        string connectionString = configuration.GetConnectionString(LoyaltyDbContext.DatabaseName);
        DbContextOptionsBuilder<LoyaltyDbContext> builder = new();
        builder.UseSqlServer(connectionString);

        return new(builder.Options);
    }

    #endregion
}