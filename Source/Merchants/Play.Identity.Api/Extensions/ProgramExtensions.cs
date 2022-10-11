using Duende.IdentityServer;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;

using IdentityModel;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Play.Identity.Api.Identity.Configuration;
using Play.Identity.Api.Identity.Entities;
using Play.Identity.Api.Identity.Persistence;
using Play.Identity.Api.Identity.Services;
using Play.Identity.Api.Services;

namespace Play.Identity.Api.Extensions
{
    internal static class ProgramExtensions
    {
        #region Instance Members

        internal static WebApplicationBuilder ConfigureIdentityServer(this WebApplicationBuilder builder)
        {
            string? identityConnectionString = builder.Configuration.GetConnectionString("Identity");
            builder.Services.AddDbContext<UserIdentityDbContext>(options =>
            {
                options.UseSqlServer(identityConnectionString);
            });
            builder.Services.AddIdentity<UserIdentity, Role>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = true;

                    // PCI-DSS: Lock the user account after not more than 6 logon attempts
                    options.Lockout.MaxFailedAccessAttempts = 4;

                    // PCI-DSS Set the lockout period to a minimum of 30 minutes or until the administrator enables the User Id
                    options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 0, 45);

                    // PCI-DSS Restricted Data - Ensure troubleshooting does not expose authentication or sensitive data
                    // options.Stores.ProtectPersonalData = true; // HACK: Implement Service

                    // PCI-DSS Passwords must be at least 7 characters, Contain numeric and alphabetic characters, Unique when updated
                    options.Password.RequiredLength = 8;

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
                .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                .AddInMemoryApiScopes(IdentityConfig.GetApiScopes())
                .AddInMemoryClients(IdentityConfig.GetClients(builder.Configuration))
                .AddTestUsers(IdentityConfig.GetTestUsers(builder.Configuration));

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
            builder.Services.AddTransient<IUserRegistrationService, UserRegistrationService>();
            builder.Services.AddTransient<IBuildLoginViewModel, LoginViewModelBuilder>();
            builder.Services.AddScoped<IProfileService, ProfileService<UserIdentity>>();

            return builder;
        }

        #endregion
    }
}