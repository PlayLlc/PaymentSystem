using Microsoft.EntityFrameworkCore;

using Play.Loyalty.Persistence.Sql.Persistence;

namespace Play.Loyalty.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString(LoyaltyDbContext.DatabaseName);

        builder.Services.AddDbContext<LoyaltyDbContext>(options =>
        {
            options.UseSqlServer(connectionString!);
        });

        return builder;
    }

    #endregion
}