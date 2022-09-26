using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.Merchants.Persistence.Sql.Sql;

internal class MerchantPortalDbContextFactory : IDesignTimeDbContextFactory<MerchantPortalDbContext>
{
    #region Instance Members

    /// <summary>
    ///     Used to generate migrations
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public MerchantPortalDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        var builder = new DbContextOptionsBuilder<MerchantPortalDbContext>();

        var connectionString = configuration.GetConnectionString("sql");

        builder.UseSqlServer(connectionString);

        return new MerchantPortalDbContext(builder.Options);
    }

    #endregion
}