using Microsoft.EntityFrameworkCore;

using Play.Identity.Api.Client;
using Play.Inventory.Application.Handlers;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Persistence.Sql.Persistence;
using Play.Inventory.Persistence.Sql.Repositories;
using Play.Mvc.Swagger;
using Play.Restful.Clients;

namespace Play.Inventory.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ApiConfiguration identityApiConfiguration = builder.Configuration.GetSection("IdentityApi").Get<ApiConfiguration>();

        builder.Services.AddScoped<DbContext, InventoryDbContext>();

        //builder.Services.AddScoped<DbContext, InventoryDbContext>();
        //builder.Services.AddScoped<IRepository<Item, SimpleStringId>, ItemRepository>();
        //builder.Services.AddScoped<IItemRepository, ItemRepository>();
        //builder.Services.AddScoped<Repository<Item, SimpleStringId>, ItemRepository>();
        //builder.Services.AddScoped<IStoreRepository, StoreRepository>();
        //builder.Services.AddScoped<IRetrieveMerchants, MerchantRetriever>();
        //builder.Services.AddScoped<IRetrieveUsers, UserRetriever>();

        // Api Clients 
        builder.Services.AddScoped<IMerchantApi, MerchantApi>(a => new MerchantApi(new Configuration(identityApiConfiguration.BasePath)));
        builder.Services.AddScoped<IUserApi, UserApi>(a => new UserApi(new Configuration(identityApiConfiguration.BasePath)));
        builder.Services.AddScoped<IRegistrationApi, RegistrationApi>(a => new RegistrationApi(new Configuration(identityApiConfiguration.BasePath)));

        // Repositories
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IItemRepository, ItemRepository>();

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