﻿using Microsoft.EntityFrameworkCore;

namespace Play.Registration.Api.Extensions;
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