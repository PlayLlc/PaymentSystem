using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.Registration.Persistence.Sql.Persistence;

internal class RegistrationDbContextOptionsBuilder : IDesignTimeDbContextFactory<RegistrationDbContext>
{
    #region Instance Members

    public RegistrationDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        string connectionString = configuration.GetConnectionString("Identity");
        DbContextOptionsBuilder<RegistrationDbContext> builder = new();
        builder.UseSqlServer(connectionString);

        return new RegistrationDbContext(builder.Options);
    }

    #endregion
}