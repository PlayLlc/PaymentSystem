using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Play.Identity.Api.Identity.Persistence;

internal class UserIdentityDbContextFactory : IDesignTimeDbContextFactory<UserIdentityDbContext>
{
    #region Instance Members

    public UserIdentityDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<UserIdentityDbContext> builder = new DbContextOptionsBuilder<UserIdentityDbContext>();
        string? envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        IConfigurationRoot? configuration = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{envName}.json", false)
            .Build();

        string? connectionString = configuration.GetConnectionString("Identity");

        builder.UseSqlServer(connectionString);

        return new UserIdentityDbContext(builder.Options);
    }

    #endregion
}