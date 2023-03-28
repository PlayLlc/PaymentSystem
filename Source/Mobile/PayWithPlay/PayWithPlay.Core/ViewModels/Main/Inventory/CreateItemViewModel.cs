using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class CreateItemViewModel : BaseViewModel
    {
        private string? _addImageText;

        public CreateItemViewModel()
        {
            AddImageText = "Add image";

            Item.InputChangedAction = () => RaisePropertyChanged(nameof(CreateItemButtonEnabled));

            Stores.Stores.Add(new StoreItemModel() { Title = "Store 100" });
            Stores.Stores.Add(new StoreItemModel() { Title = "Store 101" });
            Stores.Stores.Add(new StoreItemModel() { Title = "Store 102" });
            Stores.Stores.Add(new StoreItemModel() { Title = "Store 103" });
            Stores.Stores.Add(new StoreItemModel() { Title = "Store 104" });
            Stores.Stores.Add(new StoreItemModel() { Title = "Store 105" });
            Stores.Stores.Add(new StoreItemModel() { Title = "Store 106" });
            Stores.Stores.Add(new StoreItemModel() { Title = "Store 107" });
            Stores.Stores.Add(new StoreItemModel() { Title = "Store 108" });
            Stores.OnAllStores();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Item.ClearValidators();

            base.ViewDestroy(viewFinishing);
        }

        public string Title => Resource.CreateItem;
        public string CreateItemButtonText => Resource.CreateItem;

        public Action? OnBackAction { get; set; }

        public string? AddImageText
        {
            get => _addImageText;
            set => SetProperty(ref _addImageText, value);
        }

        public ItemModel Item { get; } = new ItemModel();

        public ProductDetailsModel ProductDetails { get; } = new ProductDetailsModel();

        public StoresModel Stores { get; } = new StoresModel();

        public CategoryModel Category { get; } = new CategoryModel();

        public AlertsModel Alerts { get; } = new AlertsModel();

        public bool? CreateItemButtonEnabled => Item.AreInputsValid;

        public void OnBack()
        {
            OnBackAction?.Invoke();
        }

        public void OnAddImage()
        {
        }

        public void OnCreateItem() { }
    }
}