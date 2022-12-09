using Microsoft.EntityFrameworkCore;

using Play.Payroll.Persistence.Sql.Persistence;

namespace Play.Payroll.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString(PayrollDbContext.DatabaseName);

        builder.Services.AddDbContext<PayrollDbContext>(options =>
        {
            options.UseSqlServer(connectionString!);
        });

        return builder;
    }

    #endregion
}