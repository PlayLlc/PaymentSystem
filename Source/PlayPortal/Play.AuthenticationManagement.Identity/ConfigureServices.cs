using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Play.AuthenticationManagement.Identity.Data.DataSeed;
using Play.AuthenticationManagement.Identity.Models;
using Play.AuthenticationManagement.Identity.Services;

namespace Play.AuthenticationManagement.Identity;

public static class ConfigureServices
{
    public static async Task<IServiceCollection> AddIdentityServices(this IServiceCollection services, string identityConnectionString)
    {
        services.AddDbContext<Data.DbContext>(options =>
            {
                options.UseSqlServer(identityConnectionString);
            });

        services.AddIdentity<User, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                //options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<Data.DbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 7;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredUniqueChars = 1;

            options.User.RequireUniqueEmail = true;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 6;
        });

        services.AddTransient<IIdentityService, IdentityService>();

        await SeedDefaultIdentityData(services);

        return services;
    }

    private static async Task SeedDefaultIdentityData(IServiceCollection services)
    {
        var seeder = new IdentityDbContextSeedData(services.BuildServiceProvider().GetService<Data.DbContext>()!);

        await seeder.SeedDefaultRoles();

        await seeder.SeedDefaultAdmin();
    }
}
