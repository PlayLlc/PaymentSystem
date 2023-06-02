using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class LoyaltyDiscountsScanItemViewModel : BaseScanInventoryItemViewModel
    {
        public LoyaltyDiscountsScanItemViewModel()
        {
        }

        public override string Title => Resource.LoyaltyDiscountScanItem;

        public override void OnScanResult(string value)
        {
            Task.Run(async () =>
            {
                IsScanning = false;
                IsLoading = true;

                await Task.Delay(2500);

                IsLoading = false;

                var random = new Random();

                var randomValue = random.Next(0, 100);
                string? pictureUrl = null;
                if (randomValue > 30)
                {
                    pictureUrl = $"https://picsum.photos/200?random";
                }

                var categories = new List<ChipModel>();
                for (int j = 0; j < 4; j++)
                {
                    categories.Add(new ChipModel() { Title = $"Category: {j}", Type = ChipType.ItemCategory });
                }

                var inventoryItem = new InventoryItemModel
                {
                    Name = "Classic Male T-shirt",
                    SKU = value,
                    Stock = 39,
                    Price = 14.9m,
                    PictureUrl = pictureUrl,
                    Categories = categories,
                };

                var hasDiscounts = random.NextDouble() >= 0.5d;

                if (hasDiscounts)
                {
                    var discountType = random.NextDouble() >= 0.5d ? DiscountType.Amount : DiscountType.Percentage;
                    var discountValue = 5m;
                    inventoryItem.DiscountType = discountType;
                    inventoryItem.Categories.Insert(0,
                        new ChipModel() { Title = $"{(discountType == DiscountType.Amount ? $"${discountValue:0.00}" : $"{discountValue:0}%")} OFF", Type = ChipType.ItemDiscount });
                    inventoryItem.HasDiscount = true;
                    inventoryItem.DiscountValue = discountValue;
                }


                Items.Insert(0, inventoryItem);
            });
        }

        public override void OnInventoryItem(InventoryItemModel inventoryItemModel)
        {
            NavigationService.Navigate<LoyaltySetDiscountViewModel, InventoryItemModel>(inventoryItemModel);
        }
    }
}
