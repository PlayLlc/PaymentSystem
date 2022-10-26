using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer;

using IdentityModel;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Persistence;
using Play.Identity.Api.Identity;

namespace Play.Identity.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureIdentityServer(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<UserIdentity, RoleIdentity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = true;

                // PCI-DSS: Lock the user account after not more than 6 logon attempts
                options.Lockout.MaxFailedAccessAttempts = 6;

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

        // TODO: used for configuration data such as clients, resources, and scopes. https://docs.identityserver.io/en/3.1.0/quickstarts/4_entityframework.html
        // ConfigureDbContext
        // TODO: used for temporary operational data such as authorization codes, and refresh tokens
        // PersistedGrantDbContext 

        // string migrationAssembly = typeof(Program).Assembly.GetName().Name!;
        //.AddConfigurationStore(options =>
        //{
        //    options.ConfigureDbContext = dbContextOptions =>
        //        dbContextOptions.UseSqlServer(identityConnectionString, sql => sql.MigrationsAssembly(migrationAssembly));
        //})

        // PersistedGrantDbContext

        //.AddOperationalStore(options =>
        //{
        //    options.ConfigureDbContext = dbContextOptions =>
        //        dbContextOptions.UseSqlServer(identityConnectionString, sql => sql.MigrationsAssembly(migrationAssembly));

        //    // this enables automatic token cleanup. this is optional.
        //    options.EnableTokenCleanup = true;
        //    options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
        //}); 

        builder.Services.AddAuthentication()
            .AddGoogle("Google", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            })
            .AddMicrosoftAccount("Microsoft", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
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

    #endregion
}