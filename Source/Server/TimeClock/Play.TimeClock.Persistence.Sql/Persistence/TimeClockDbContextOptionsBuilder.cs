﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.TimeClock.Persistence.Sql.Persistence;

internal class TimeClockDbContextOptionsBuilder : IDesignTimeDbContextFactory<TimeClockDbContext>
{
    #region Instance Members

    public TimeClockDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        string connectionString = configuration.GetConnectionString(TimeClockDbContext.DatabaseName);
        DbContextOptionsBuilder<TimeClockDbContext> builder = new();
        builder.UseSqlServer(connectionString);

        return new(builder.Options);
    }

    #endregion
}