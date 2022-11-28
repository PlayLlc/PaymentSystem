using Microsoft.EntityFrameworkCore;

using Play.TimeClock.Persistence.Sql.Persistence;

namespace Play.Loyalty.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString(TimeClockDbContext.DatabaseName);

        builder.Services.AddDbContext<TimeClockDbContext>(options =>
        {
            options.UseSqlServer(connectionString!);
        });

        return builder;
    }

    #endregion
}