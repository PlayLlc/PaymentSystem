using Duende.IdentityServer.Configuration;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Api.Data;
using Play.Accounts.Api.Services;
using Play.Merchants.Onboarding.Domain.Aggregates;

using ApplicationUser = Play.Accounts.Api.Services.ApplicationUser;

namespace Play.Accounts.Api.Extensions
{
    internal static class WebApplicationBuilderExtensions
    {
        #region Instance Members

        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            string migrationsAssembly = typeof(Program).Assembly!.GetName().Name!;

            // HACK: GRAB FROM JSON
            string connectionString = "";

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //builder.Services.AddIdentityServices(builder.Configuration.GetConnectionString("Play.IdentityStore"));

            builder.Services.AddIdentityServer(options =>
            {
                // options.Endpoints.

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                //options.EmitStaticAudienceClaim = true;
            });

            //IdentityRole 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = true;

                // PCI-DSS: Lock the user account after not more than 6 logon attempts
                options.Lockout.MaxFailedAccessAttempts = 4;

                // PCI-DSS Set the lockout period to a minimum of 30 minutes or until the administrator enables the User Id
                options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 0, 45);

                // PCI-DSS Restricted Data - Ensure troubleshooting does not expose authentication or sensitive data
                options.Stores.ProtectPersonalData = true;

                // PCI-DSS Passwords must be at least 7 characters, Contain numeric and alphabetic characters, Unique when updated
                options.Password.RequiredLength = 8;

                // PCI-DSS Unique user ids
                options.User.RequireUniqueEmail = true;

                // TODO: User Passwords must be unique when updated. Require last 4 passwords to be unique
            });

            //identityServerBuilder
            //    .AddOperationalStore(options => options.ConfigureDbContext = builder =>
            //        builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
            //    .AddConfigurationStore(options => options.ConfigureDbContext = builder =>
            //        builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            //identityServerBuilder.AddAspNetIdentity<IdentityUser>();

            return builder.Build();
        }

        #endregion
    }
}