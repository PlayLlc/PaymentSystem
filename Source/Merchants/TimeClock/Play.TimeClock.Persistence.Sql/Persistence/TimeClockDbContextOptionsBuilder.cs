using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using Play.Loyalty.Persistence.Sql.Persistence;

namespace Play.Loyalty.Persistence.Sql.Persistence;

internal class TimeClockDbContextOptionsBuilder : IDesignTimeDbContextFactory<TimeClockDbContext>
{
    #region Constructor

    public TimeClockDbContextOptionsBuilder()
    { }

    #endregion

    #region Instance Members

    public TimeClockDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        string connectionString = configuration.GetConnectionString(TimeClockDbContext.DatabaseName);
        DbContextOptionsBuilder<TimeClockDbContext> builder = new DbContextOptionsBuilder<TimeClockDbContext>();
        builder.UseSqlServer(connectionString);

        return new TimeClockDbContext(builder.Options);
    }

    #endregion
}