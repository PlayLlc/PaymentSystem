using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Play.Identity.Domain.Services;
using Play.Identity.Persistence.Sql.Entities;
using Play.Identity.Persistence.Sql.Persistence;

namespace Play.Identity.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static async Task SeedDb(this WebApplicationBuilder builder)
    {
        ServiceProvider serviceBuilder = builder.Services.BuildServiceProvider();
        UserIdentityDbSeeder seeder =
            new UserIdentityDbSeeder(serviceBuilder.GetService<UserIdentityDbContext>()!, serviceBuilder.GetService<IHashPasswords>()!);

        await seeder.Seed(serviceBuilder.GetService<UserManager<UserIdentity>>()!,
                new RoleStore<RoleIdentity>(serviceBuilder.GetService<UserIdentityDbContext>()))
            .ConfigureAwait(false);
    }

    #endregion
}