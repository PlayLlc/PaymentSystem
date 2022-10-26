using AutoMapper;

using Play.Accounts.Persistence.Sql.Mapping;

namespace Play.Identity.Api.Extensions
{
    public static partial class WebApplicationBuilderExtensions
    {
        #region Instance Members

        internal static WebApplicationBuilder ConfigureAutoMapper(this WebApplicationBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccountMappingProfile());
            });
            var mapper = mapperConfig.CreateMapper();

            builder.Services.AddSingleton(mapper);

            return builder;
        }

        #endregion
    }
}