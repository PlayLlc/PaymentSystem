using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using Play.Loyalty.Persistence.Sql.Persistence;

namespace Play.Loyalty.Persistence.Sql.Persistence;

internal class LoyaltyDbContextOptionsBuilder : IDesignTimeDbContextFactory<LoyaltyDbContext>
{
    #region Constructor

    public LoyaltyDbContextOptionsBuilder()
    { }

    #endregion

    #region Instance Members

    public LoyaltyDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        string connectionString = configuration.GetConnectionString("Loyalty");
        DbContextOptionsBuilder<LoyaltyDbContext> builder = new DbContextOptionsBuilder<LoyaltyDbContext>();
        builder.UseSqlServer(connectionString);

        return new LoyaltyDbContext(builder.Options);
    }

    #endregion
}