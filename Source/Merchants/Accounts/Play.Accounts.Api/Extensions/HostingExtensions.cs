using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Api.Data;

using Serilog;

namespace Play.Accounts.Api.Extensions
{
    internal static class HostingExtensions
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

            IIdentityServerBuilder identityServerBuilder = builder.Services.AddIdentityServer(options =>
            {
                options.KeyManagement.Enabled = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            });

            identityServerBuilder
                .AddOperationalStore(options => options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(options => options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            identityServerBuilder.AddAspNetIdentity<IdentityUser>();

            return builder.Build();
        }

        /// <exception cref="DbUpdateException"></exception>
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            InitializeDbTestData(app);
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.UseIdentityServer();
            app.Run();

            return app;
        }

        /// <exception cref="DbUpdateException"></exception>
        private static void InitializeDbTestData(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

            ConfigurationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!context.Clients.Any())
            {
                foreach (Client client in Clients.Get())
                    context.Clients.Add(client.ToEntity());
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (IdentityResource resource in Resources.GetIdentityResources())
                    context.IdentityResources.Add(resource.ToEntity());
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (ApiScope scope in Resources.GetApiScopes())
                    context.ApiScopes.Add(scope.ToEntity());
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (ApiResource resource in Resources.GetApiResources())
                    context.ApiResources.Add(resource.ToEntity());
                context.SaveChanges();
            }

            UserManager<IdentityUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (!userManager.Users.Any())
                foreach (TestUser testUser in Users.Get())
                {
                    IdentityUser identityUser = new IdentityUser(testUser.Username) {Id = testUser.SubjectId};

                    userManager.CreateAsync(identityUser, "Password123!").Wait();
                    userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList()).Wait();
                }
        }

        #endregion
    }
}