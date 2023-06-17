using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class InventoryScanItemViewModel : BaseScanInventoryItemViewModel
    {
        public InventoryScanItemViewModel()
        {
        }

        public override string Title => Resource.ScanItem;

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
                    categories.Add(new ChipModel() { Title = $"Category: {j}" , Type = ChipType.ItemCategory});
                }

                var inventoryItem = new InventoryItemModel
                {
                    Name = "Classic Male T-shirt",
                    SKU = value,
                    Stock = 39,
                    Price = 14.9m,
                    PictureUrl = pictureUrl,
                    Categories = categories,
                    OnEditAction = OnEditInventoryItem,
                    OnManageStockAction = OnManageStockInventoryItem,
                    OnDeleteAction = OnDeleteInventoryItem
                };

                Items.Insert(0, inventoryItem);
            });
        }

        public override void OnInventoryItem(InventoryItemModel inventoryItemModel)
        {
            var currentSelected = Items.FirstOrDefault(x => x.Selected);
            if (currentSelected != null)
            {
                currentSelected.Selected = false;
            }

            inventoryItemModel.Selected = !inventoryItemModel.Selected;
        }

        private void OnEditInventoryItem(InventoryItemModel obj)
        {
            var editObj = new CreateItemViewModel.EditInventoryItemModel
            {
                Name = obj.Name,
                SKU = obj.SKU,
                PictureUrl = obj.PictureUrl,
                Price = obj.Price,
                Stock = obj.Stock,
                Categories = obj.Categories.Select(t => new CreateItemViewModel.EditInventoryItemModel.Category { Title = t.Title }).ToList()
            };

            NavigationService.Navigate<CreateItemViewModel, CreateItemViewModel.EditInventoryItemModel>(editObj);
        }

        private void OnManageStockInventoryItem(InventoryItemModel obj)
        {
            NavigationService.Navigate<ManageStockViewModel>();
        }

        private void OnDeleteInventoryItem(InventoryItemModel obj)
        {
            InvokeOnMainThread(() => Items.Remove(obj));
        }
    }
}
