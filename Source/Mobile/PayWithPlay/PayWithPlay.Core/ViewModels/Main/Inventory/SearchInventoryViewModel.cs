using MvvmCross.Navigation;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;
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

                var categories = new List<ChipModel>();

                for (int j = 0; j < i + 1; j++)
                {
                    categories.Add(new ChipModel() { Title = MockDataUtils.RandomString(random.Next(0, 20)), Type = ChipType.ItemCategory });
                }

                var inventoryItem = new InventoryItemModel
                {
                    Name = $"Classic Male T-shirt {i}",
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

        public string? Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        public ObservableCollection<InventoryItemModel> Items { get; set; } = new ObservableCollection<InventoryItemModel>();

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnCategories()
        {
            NavigationService.Navigate<CategoriesSelectionViewModel, BaseItemSelectionViewModel.NavigationData>(new BaseItemSelectionViewModel.NavigationData
            {
                ResultItemsAction = (items) =>
                {
                },
                SelectionType = ItemSelectionType.Multiple
            });
        }

        public void OnStores()
        {
            NavigationService.Navigate<StoresSelectionViewModel, BaseItemSelectionViewModel.NavigationData>(new BaseItemSelectionViewModel.NavigationData
            {
                ResultItemsAction = (items) =>
                {
                },
                SelectionType = ItemSelectionType.Multiple
            });
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
