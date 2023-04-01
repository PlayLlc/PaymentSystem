using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;
using System;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class SearchInventoryViewModel : BaseViewModel
    {
        private string? _search;
        private string? _sortByButtonText;
        private string? _storeButtonText;

        public SearchInventoryViewModel()
        {
            SortByButtonText = "Categories";
            StoreButtonText = "All stores";

            var random = new Random();
            for (int i = 0; i < 20; i++)
            {
                var randomValue = random.Next(0, 100);
                string? pictureUrl = null;
                if (randomValue > 30)
                {
                    pictureUrl = $"https://picsum.photos/200?random{i}";
                }

                var categories = new List<CategoryItemModel>();

                for (int j = 0; j < i + 1; j++)
                {
                    categories.Add(new CategoryItemModel() { Title = RandomString(random.Next(0, 20)) });
                }

                var inventoryItem = new InventoryItemModel
                {
                    Name = "Classic Male T-shirt",
                    SKU = "03-GRN-1-XL",
                    Stock = 39,
                    Price = 14.9m,
                    PictureUrl = pictureUrl,
                    Categories = categories,
                    OnEditAction = OnEditInventoryItem,
                    OnManageStockAction = OnManageStockInventoryItem,
                    OnDeleteAction = OnDeleteInventoryItem
                };

                Items.Add(inventoryItem);
            }
        }

        public string Title => Resource.SearchInventory;
        public string SearchText => Resource.Search;
        public string SortByText => Resource.SortBy;
        public string StoreText => Resource.Store;

        public string? SortByButtonText
        {
            get => _sortByButtonText;
            set => SetProperty(ref _sortByButtonText, value);
        }

        public string? StoreButtonText
        {
            get => _storeButtonText;
            set => SetProperty(ref _storeButtonText, value);
        }

        public Action? OnBackAction { get; set; }

        public string? Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        public ObservableCollection<InventoryItemModel> Items { get; set; } = new ObservableCollection<InventoryItemModel>();

        public void OnBack()
        {
            OnBackAction?.Invoke();
        }

        public void OnInventoryItem(InventoryItemModel inventoryItemModel)
        {
            var currentSelected = Items.FirstOrDefault(x => x.Selected);
            if (currentSelected != null)
            {
                currentSelected.Selected = false;
            }

            inventoryItemModel.Selected = !inventoryItemModel.Selected;
        }

        //TODO:: to be removed
        private static string RandomString(int length)
        {
            var random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        private void OnEditInventoryItem(InventoryItemModel obj)
        {
        }

        private void OnManageStockInventoryItem(InventoryItemModel obj)
        {
        }

        private void OnDeleteInventoryItem(InventoryItemModel obj)
        {
            Items.Remove(obj);
        }
    }
}
