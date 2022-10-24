using Microsoft.EntityFrameworkCore;

using Play.Accounts.Persistence.Sql.Persistence;

namespace Play.Identity.Api.Extensions
{
    public static partial class WebApplicationBuilderExtensions
    {
        #region Instance Members

        internal static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
        {
            string? identityConnectionString = builder.Configuration.GetConnectionString("Identity");

            builder.Services.AddDbContext<UserIdentityDbContext>(options =>
            {
                options.UseSqlServer(identityConnectionString);
            });

            return builder;
        }

        #endregion
    }
}