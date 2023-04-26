using Microsoft.AspNetCore.Identity;

namespace Play.Registration.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static async Task SeedDb(this WebApplicationBuilder builder)
    {
        ServiceProvider serviceBuilder = builder.Services.BuildServiceProvider();

        UserIdentityDbSeeder seeder = new(serviceBuilder.GetService<UserIdentityDbContext>()!, serviceBuilder.GetService<IHashPasswords>()!);

        await seeder.Seed(serviceBuilder.GetService<UserManager<UserIdentity>>()!,
                new RoleStore<RoleIdentity>(serviceBuilder.GetService<UserIdentityDbContext>()))
            .ConfigureAwait(false);
    }

    #endregion
}