namespace Play.Registration.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureAutoMapper(this WebApplicationBuilder builder)
    {
        MapperConfiguration mapperConfig = new(cfg =>
        {
            cfg.AddProfile(new AccountMappingProfile());
        });
        IMapper? mapper = mapperConfig.CreateMapper();

        builder.Services.AddSingleton(mapper);

        return builder;
    }

    #endregion
}