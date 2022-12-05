using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Play.Payroll.Persistence.Sql.Persistence;

internal class PayrollDbContextOptionsBuilder : IDesignTimeDbContextFactory<PayrollDbContext>
{
    #region Instance Members

    public PayrollDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        string connectionString = configuration.GetConnectionString(PayrollDbContext.DatabaseName);
        DbContextOptionsBuilder<PayrollDbContext> builder = new();
        builder.UseSqlServer(connectionString);

        return new PayrollDbContext(builder.Options);
    }

    #endregion
}