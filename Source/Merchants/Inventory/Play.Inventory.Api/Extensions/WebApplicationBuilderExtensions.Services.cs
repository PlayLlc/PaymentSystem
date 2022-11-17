namespace Play.Inventory.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        //ConfigurationManager configurationManager = builder.Configuration;

        //builder.Services.AddScoped<DbContext, InventoryDbContext>();
        //builder.Services.AddScoped<IRepository<Item, SimpleStringId>, ItemRepository>();
        //builder.Services.AddScoped<IItemRepository, ItemRepository>();
        //builder.Services.AddScoped<Repository<Item, SimpleStringId>, ItemRepository>();
        //builder.Services.AddScoped<IStoreRepository, StoreRepository>();
        //builder.Services.AddScoped<IRetrieveMerchants, MerchantRetriever>();
        //builder.Services.AddScoped<IRetrieveUsers, UserRetriever>();

        return builder;
    }

    #endregion
}