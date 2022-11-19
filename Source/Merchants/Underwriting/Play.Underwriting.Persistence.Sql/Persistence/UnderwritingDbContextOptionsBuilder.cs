using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Play.Underwriting.Persistence.Persistence;

namespace Play.Underwriting.Persistence.Sql.Persistence;

internal class UnderwritingDbContextOptionsBuilder : IDesignTimeDbContextFactory<UnderwritingDbContext>
{
    public UnderwritingDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        string connectionString = configuration.GetConnectionString("Underwriting");
        DbContextOptionsBuilder<UnderwritingDbContext> builder = new DbContextOptionsBuilder<UnderwritingDbContext>();
        builder.UseSqlServer(connectionString);

        return new UnderwritingDbContext(builder.Options);

    }

}
