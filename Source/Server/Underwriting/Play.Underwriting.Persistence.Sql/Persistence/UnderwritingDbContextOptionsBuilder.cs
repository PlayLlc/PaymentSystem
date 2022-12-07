using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.Underwriting.Persistence.Sql.Persistence;

internal class UnderwritingDbContextOptionsBuilder : IDesignTimeDbContextFactory<UnderwritingDbContext>
{
    #region Instance Members

    public UnderwritingDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        string connectionString = configuration.GetConnectionString("Underwriting");
        DbContextOptionsBuilder<UnderwritingDbContext> builder = new();
        builder.UseSqlServer(connectionString);

        return new(builder.Options);
    }

    #endregion
}