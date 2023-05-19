using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleSelectItemViewModel : BaseViewModel<Action<InventoryItemModel>>
    {
        private string? _search;
        private Action<InventoryItemModel>? _onItemAction;

        public SaleSelectItemViewModel()
        {
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
                    Id = Guid.NewGuid(),
                    Name = $"{i} Classic Male T-shirt",
                    SKU = "03-GRN-1-XL",
                    Stock = 39,
                    Price = 14.9m,
                    PictureUrl = pictureUrl,
                    Categories = categories,
                };

                Items.Add(inventoryItem);
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _onItemAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public string SearchText => Resource.Search;

        public string? Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        public ObservableCollection<InventoryItemModel> Items { get; set; } = new ObservableCollection<InventoryItemModel>();

        public void OnInventoryItem(InventoryItemModel inventoryItemModel)
        {
            _onItemAction?.Invoke(inventoryItemModel);
        }

        public override void Prepare(Action<InventoryItemModel> parameter)
        {
            _onItemAction = parameter;
        }
    }
}
