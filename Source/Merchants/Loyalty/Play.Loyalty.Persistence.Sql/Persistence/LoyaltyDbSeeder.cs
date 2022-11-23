using Microsoft.EntityFrameworkCore;

namespace Play.Loyalty.Persistence.Sql.Persistence;

public class LoyaltyDbSeeder
{
    #region Constructor

    public LoyaltyDbSeeder()
    { }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="DbUpdateException"></exception>
    public async Task Seed()
    {
        //var userId = Randomize.AlphaNumericSpecial.String(20);
        //var merchantId = "rAo-@V-kU_UY(WZPhn/_";
        //User user = new User(userId, merchantId);
        //var itemId = "r2]!J9}^}GxhO<w=>Y C";
        //var item = await _ItemRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false);
        //item.AddQuantity(_UserService, new AddQuantityToInventory()
        //{
        //    Action = StockActions.Restock,
        //    ItemId = itemId,
        //    Quantity = 50,
        //    UserId = userId
        //});
        //await _ItemRepository.SaveAsync(item).ConfigureAwait(false);

        //var userId = Randomize.AlphaNumericSpecial.String(20);
        //var merchantId = Randomize.AlphaNumericSpecial.String(20);
        //var storeId = Randomize.AlphaNumericSpecial.String(20);
        //var itemId = Randomize.AlphaNumericSpecial.String(20);

        //User user = new User(userId, merchantId);
        //Store store = new Store(storeId, itemId);
        //Merchant merchant = new Merchant(merchantId, Array.Empty<Category>());

        //CreateItem createItemCommand = new CreateItem()
        //{
        //    MerchantId = merchant.Id,
        //    Description = "Ben Sherman fall collection knitted sleeves",
        //    Name = "Ben Sherman",
        //    Price = new Money(100, new NumericCurrencyCode(840)),
        //    Sku = "123456",
        //    Stores = new List<StoreDto>() {store.AsDto()},
        //    UserId = userId
        //};

        //var item = Item.CreateNewItem(user, merchant, createItemCommand);

        //CreateCategory createCategoryCommand = new()
        //{
        //    ItemId = item.GetId(),
        //    CategoryName = "Large",
        //    Stores = new List<StoreDto>() {store.AsDto()}
        //};

        //var categoryId = item.CreateCategory(user, createCategoryCommand);

        //var updateCategory = new UpdateCategory()
        //{
        //    CategoryId = categoryId,
        //    ItemId = item.GetId(),
        //    Stores = new List<StoreDto>() {store.AsDto()},
        //    UserId = userId
        //};

        //item.AddCategory(user, updateCategory);
        //    await _ItemRepository.SaveAsync(item).ConfigureAwait(false);
    }

    #endregion
}