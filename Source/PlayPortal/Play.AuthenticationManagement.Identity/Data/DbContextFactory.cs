using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.AuthenticationManagement.Identity.Data;

internal class DbContextFactory : IDesignTimeDbContextFactory<DbContext>
{
    public DbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

        var builder = new DbContextOptionsBuilder<DbContext>();

        var connectionString = configuration.GetConnectionString("Play.UserStore");

        builder.UseSqlServer(connectionString);

        return new DbContext(builder.Options);
    }
}
