using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.Identity.Persistence.Sql.Persistence;

internal class UserDbContextOptionsBuilder : IDesignTimeDbContextFactory<UserIdentityDbContext>
{
    #region Constructor

    #endregion

    #region Instance Members

    public UserIdentityDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        string connectionString = configuration.GetConnectionString("Identity");
        DbContextOptionsBuilder<UserIdentityDbContext> builder = new DbContextOptionsBuilder<UserIdentityDbContext>();
        builder.UseSqlServer(connectionString);

        return new UserIdentityDbContext(builder.Options);
    }

    #endregion
}