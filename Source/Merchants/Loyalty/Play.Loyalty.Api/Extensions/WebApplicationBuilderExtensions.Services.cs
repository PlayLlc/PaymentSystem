using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Identity.Api.Client;
using Play.Loyalty.Application.Services;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;
using Play.Loyalty.Persistence.Sql.Persistence;
using Play.Loyalty.Persistence.Sql.Repositories;
using Play.Restful.Clients;

namespace Play.Loyalty.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ApiConfiguration identityApiConfiguration = builder.Configuration.GetSection("IdentityApi").Get<ApiConfiguration>();

        builder.Services.AddScoped<DbContext, LoyaltyDbContext>();

        // Api Clients 
        builder.Services.AddScoped<IMerchantApi, MerchantApi>(a => new MerchantApi(new Configuration(identityApiConfiguration.BasePath)));
        builder.Services.AddScoped<IUserApi, UserApi>(a => new UserApi(new Configuration(identityApiConfiguration.BasePath)));

        // Repositories 
        builder.Services.AddScoped<IRepository<Member, SimpleStringId>, LoyaltyMemberRepository>();
        builder.Services.AddScoped<IRepository<Programs, SimpleStringId>, LoyaltyProgramRepository>();
        builder.Services.AddScoped<ILoyaltyMemberRepository, LoyaltyMemberRepository>();
        builder.Services.AddScoped<ILoyaltyProgramRepository, LoyaltyProgramRepository>();

        // Services
        builder.Services.AddScoped<IEnsureRewardsNumbersAreUnique, LoyaltyMemberRepository>();
        builder.Services.AddScoped<IRetrieveMerchants, MerchantRetriever>();
        builder.Services.AddScoped<IRetrieveUsers, UserRetriever>();

        // HACK: Should we make these scoped per request? That would mean that we would have to add them to EVERY controller.
        // HACK: Will this introduce race conditions? There are only Write methods so we're not tracking entity changes
        // HACK: so there shouldn't be any entities that are out of sync. Need to test and validate that singleton is the
        // HACK: right move here
        // Application Handlers
        //builder.Services.AddSingleton<LoyaltyMemberHandler>();
        //builder.Services.AddSingleton<LoyaltyProgramHandler>();

        return builder;
    }

    #endregion
}