using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class LoyaltyDiscountsViewModel : BaseViewModel
    {
        private readonly IWheelPicker _wheelPicker;
        private readonly string[] _sortByValues = new[] { Resource.NoDiscounts, Resource.AllDiscounts };

        private string? _searchTerm;
        private string? _sortByValue;

        public LoyaltyDiscountsViewModel(IWheelPicker wheelPicker)
        {
            _wheelPicker = wheelPicker;

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
                };

                var hasDiscounts = random.NextDouble() >= 0.5d;

                if (hasDiscounts)
                {
                    var discountType = random.NextDouble() >= 0.5d ? DiscountType.Amount : DiscountType.Percentage;
                    var discountValue = 5m;
                    inventoryItem.DiscountType = discountType;
                    inventoryItem.Categories.Insert(0, 
                        new ChipModel() { Title = $"{(discountType == DiscountType.Amount ? $"${discountValue:0.00}" : $"{discountValue:0}%")} OFF", Type = ChipType.ItemDiscount});
                    inventoryItem.HasDiscount = true;
                    inventoryItem.DiscountValue = discountValue;
                }

                Items.Add(inventoryItem);
            }
        }

        public string Title => Resource.Discounts;
        public string SearchText => Resource.Search;
        public string ScanButtonText => Resource.Scan;
        public string SortByText => Resource.SortBy;
        public string SelectCategoriesText => Resource.SelectCategories;
        public string CategoriesText => Resource.Categories;

        public MvxObservableCollection<InventoryItemModel> Items { get; set; } = new MvxObservableCollection<InventoryItemModel>();

        public string? SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public string? SortByValue
        {
            get => _sortByValue;
            set => SetProperty(ref _sortByValue, value);
        }

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnScan()
        {
        }

        public void OnSortBy()
        {
            var index = Array.IndexOf(_sortByValues, SortByValue);

            _wheelPicker.Show(_sortByValues, index == -1 ? 0 : index, Resource.SortBy, Resource.Ok, Resource.Cancel, (index) =>
            {
                SortByValue = _sortByValues[index];
            });
        }

        public void OnSelectCategories() 
        {
        }

        public void OnItemClick(InventoryItemModel inventoryItemModel)
        {
            NavigationService.Navigate<LoyaltySetDiscountViewModel, InventoryItemModel>(inventoryItemModel);
        }
    }
}
