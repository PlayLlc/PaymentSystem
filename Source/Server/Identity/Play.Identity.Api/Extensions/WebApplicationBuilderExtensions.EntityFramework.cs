using Microsoft.EntityFrameworkCore;

using Play.Identity.Persistence.Sql.Persistence;

namespace Play.Identity.Api.Extensions;
/*
 * var mapperConfig = new MapperConfiguration(cfg =>
{
cfg.AddProfile(new AccountMappingProfile());
});
var mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);
 */

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