using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Identity.Api.Client;
using Play.Restful.Clients;
using Play.TimeClock.Application.Services;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Repositories;
using Play.TimeClock.Domain.Services;
using Play.TimeClock.Persistence.Sql.Persistence;
using Play.TimeClock.Persistence.Sql.Repositories;

namespace PlayTimeClock.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ApiConfiguration identityApiConfiguration = builder.Configuration.GetSection("IdentityApi").Get<ApiConfiguration>();

        builder.Services.AddScoped<DbContext, TimeClockDbContext>();

        // Api Clients 
        builder.Services.AddScoped<IMerchantApi, MerchantApi>(a => new MerchantApi(new Configuration(identityApiConfiguration.BasePath)));
        builder.Services.AddScoped<IUserApi, UserApi>(a => new UserApi(new Configuration(identityApiConfiguration.BasePath)));

        // Repositories 
        builder.Services.AddScoped<IRepository<Employee, SimpleStringId>, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        // Services
        builder.Services.AddScoped<IEnsureEmployeeDoesNotExist, EmployeeRepository>();

        builder.Services.AddScoped<IRetrieveMerchants, MerchantRetriever>();
        builder.Services.AddScoped<IRetrieveUsers, UserRetriever>();

        // HACK: Should we make these scoped per request? That would mean that we would have to add them to EVERY controller.
        // HACK: Will this introduce race conditions? There are only Write methods so we're not tracking entity changes
        // HACK: so there shouldn't be any entities that are out of sync. Need to test and validate that singleton is the
        // HACK: right move here
        // Application Handlers
        //builder.Services.AddSingleton<EmployeeHandler>(); 

        return builder;
    }

    #endregion
}