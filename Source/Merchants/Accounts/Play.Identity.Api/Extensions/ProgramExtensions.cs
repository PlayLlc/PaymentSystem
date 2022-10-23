using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;
using Duende.IdentityServer;

using IdentityModel;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Play.Accounts.Application.Services;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Persistence;
using Play.Accounts.Persistence.Sql.Repositories;
using Play.Domain.Repositories;
using Play.Identity.Api.Identity;
using Play.Persistence.Sql;
using Play.Telecom.SendGrid.Email;
using Play.Telecom.SendGrid.Sms;

namespace Play.Identity.Api.Extensions;

internal static class ProgramExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureIdentityServer(this WebApplicationBuilder builder)
    {
        string? identityConnectionString = builder.Configuration.GetConnectionString("Identity");
        string migrationAssembly = typeof(Program).Assembly.GetName().Name!;

        builder.Services.AddDbContext<UserIdentityDbContext>(options =>
        {
            options.UseSqlServer(identityConnectionString);
        });
        builder.Services.AddIdentity<UserIdentity, RoleIdentity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = true;

                // PCI-DSS: Lock the user account after not more than 6 logon attempts
                options.Lockout.MaxFailedAccessAttempts = 4;

                // PCI-DSS Set the lockout period to a minimum of 30 minutes or until the administrator enables the User Id
                options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 30, 0);

                // PCI-DSS Restricted Data - Ensure troubleshooting does not expose authentication or sensitive data
                // options.Stores.ProtectPersonalData = true; // HACK: Implement Service

                // PCI-DSS Passwords must be at least 7 characters, Contain numeric and alphabetic characters, Unique when updated
                options.Password.RequiredLength = 7;

                // PCI-DSS Unique user ids
                options.User.RequireUniqueEmail = true;

                // TODO: User Passwords must be unique when updated. Require last 4 passwords to be unique
            })
            .AddEntityFrameworkStores<UserIdentityDbContext>()
            .AddDefaultTokenProviders();

        //builder.Services.AddLocalApiAuthentication();
        builder.Services.AddIdentityServer(options =>
            {
                options.KeyManagement.Enabled = true;

                // PCI-DSS Log out of user sessions that are idle for 15 minutes or longer
                options.Authentication.CookieLifetime = TimeSpan.FromMinutes(15);
                options.Authentication.CookieSlidingExpiration = true;
                options.Authentication.CoordinateClientLifetimesWithUserSession = true;
                options.ServerSideSessions.UserDisplayNameClaimType = JwtClaimTypes.Name;
                options.ServerSideSessions.RemoveExpiredSessions = true;
                options.ServerSideSessions.RemoveExpiredSessionsFrequency = TimeSpan.FromSeconds(10);
                options.ServerSideSessions.ExpiredSessionsTriggerBackchannelLogout = true;
            })

            // .AddDeveloperSigningCredential() // HACK: we will use this only for dev. for production we need appropriate signing certificates for our tls.
            .AddAspNetIdentity<UserIdentity>()
            .AddProfileService<ProfileService<UserIdentity>>()
            .AddInMemoryIdentityResources(IdentityInMemoryConfig.GetIdentityResources())
            .AddInMemoryApiScopes(IdentityInMemoryConfig.GetApiScopes())
            .AddInMemoryClients(IdentityInMemoryConfig.GetClients(builder.Configuration));

        //.AddConfigurationStore(options =>
        //{
        //    options.ConfigureDbContext = dbContextOptions =>
        //        dbContextOptions.UseSqlServer(identityConnectionString, sql => sql.MigrationsAssembly(migrationAssembly));
        //})
        //.AddOperationalStore(options =>
        //{
        //    options.ConfigureDbContext = dbContextOptions =>
        //        dbContextOptions.UseSqlServer(identityConnectionString, sql => sql.MigrationsAssembly(migrationAssembly));

        //    // this enables automatic token cleanup. this is optional.
        //    options.EnableTokenCleanup = true;
        //    options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
        //});
        ;

        //.AddTestUsers(IdentityConfig.GetTestUsers(builder.Configuration));

        builder.Services.AddAuthentication()
            .AddGoogle("Google", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            })
            .AddFacebook("Facebook", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
            })
            .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.SaveTokens = true;

                options.Authority = "https://demo.duendesoftware.com";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        return builder;
    }

    internal static WebApplicationBuilder ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        //  builder.Services.AddTransient<IRegisterUsers, UserRegistrationService>();
        builder.Services.AddScoped<IBuildLoginViewModel, LoginViewModelBuilder>();

        // Infrastructure Services
        builder.Services.AddScoped<ISendSmsMessages, SmsClient>();
        builder.Services.AddScoped<ISendEmail, EmailClient>();

        // Domain Services
        builder.Services.AddScoped<IEnsureUniqueEmails, UniqueEmailChecker>();
        builder.Services.AddScoped<IHashPasswords, PasswordHasher>();
        builder.Services.AddScoped<IUnderwriteMerchants, MerchantUnderwriter>();
        builder.Services.AddScoped<IVerifyEmailAccounts, EmailAccountVerifier>();
        builder.Services.AddScoped<IVerifyMobilePhones, MobilePhoneVerifier>();

        // Repositories
        builder.Services.AddScoped<IUserRegistrationRepository, UserRegistrationRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRepository<MerchantRegistration, string>, Repository<MerchantRegistration, string>>();
        builder.Services.AddScoped<IRepository<Merchant, string>, Repository<Merchant, string>>();

        // Identity Server
        builder.Services.AddScoped<IProfileService, ProfileService<UserIdentity>>();

        return builder;
    }

    internal static async Task SeedDefaultIdentityData(this WebApplicationBuilder builder)
    {
        ServiceProvider serviceBuilder = builder.Services.BuildServiceProvider();
        UserIdentityDbSeeder seeder = new UserIdentityDbSeeder(serviceBuilder.GetService<UserIdentityDbContext>()!);

        await seeder.Seed(builder.Configuration, serviceBuilder.GetService<UserManager<UserIdentity>>()!,
                new RoleStore<RoleIdentity>(serviceBuilder.GetService<UserIdentityDbContext>()))
            .ConfigureAwait(false);
    }

    #endregion
}