namespace PlayTimeClock.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static async Task SeedDb(this WebApplicationBuilder builder)
    {
        //ServiceProvider serviceBuilder = builder.Services.BuildServiceProvider();
        //IRetrieveUsers userService = serviceBuilder.GetService<IRetrieveUsers>()!;
        //IStoreRepository storeService = serviceBuilder.GetService<IStoreRepository>()!;
        //IRetrieveMerchants merchantService = serviceBuilder.GetService<IRetrieveMerchants>()!;
        //IItemRepository itemRepository = serviceBuilder.GetService<IItemRepository>()!;

        //InventoryDbSeeder seeder = new InventoryDbSeeder(userService, storeService, merchantService, itemRepository);

        //await seeder.Seed().ConfigureAwait(false);
    }

    #endregion
}