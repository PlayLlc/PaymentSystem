using Microsoft.EntityFrameworkCore;

using Play.Identity.Api.Client;
using Play.Loyalty.Application.Services;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;
using Play.Restful.Clients;

namespace Play.Loyalty.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ApiConfiguration identityApiConfiguration = builder.Configuration.GetSection("IdentityApi").Get<ApiConfiguration>();

        builder.Services.AddScoped<DbContext, InventoryDbContext>();

        // Api Clients 
        builder.Services.AddScoped<IMerchantApi, MerchantApi>(a => new MerchantApi(new Configuration(identityApiConfiguration.BasePath)));
        builder.Services.AddScoped<IUserApi, UserApi>(a => new UserApi(new Configuration(identityApiConfiguration.BasePath)));

        // Repositories
        builder.Services.AddScoped<ILoyaltyMemberRepository, CategoryRepository>();
        builder.Services.AddScoped<IItemRepository, ItemRepository>();

        // Services
        builder.Services.AddScoped<IRetrieveMerchants, MerchantRetriever>();
        builder.Services.AddScoped<IRetrieveUsers, UserRetriever>();

        // HACK: Should we make these scoped per request? That would mean that we would have to add them to EVERY controller.
        // HACK: Will this introduce race conditions? There are only Write methods so we're not tracking entity changes
        // HACK: so there shouldn't be any entities that are out of sync. Need to test and validate that singleton is the
        // HACK: right move here
        // Application Handlers
        //builder.Services.AddSingleton<CategoryHandler>();
        //builder.Services.AddSingleton<ItemHandler>();

        return builder;
    }

    #endregion
}