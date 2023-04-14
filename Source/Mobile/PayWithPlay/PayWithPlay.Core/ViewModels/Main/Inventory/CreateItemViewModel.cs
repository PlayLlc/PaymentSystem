using Microsoft.Maui.Media;
using MvvmCross.Navigation;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;
using static PayWithPlay.Core.ViewModels.Main.Inventory.CreateItemViewModel;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class CreateItemViewModel : BaseViewModel<EditInventoryItemModel>
    {
        public class EditInventoryItemModel
        {
            public string? PictureUrl { get; set; }
            public string? Name { get; set; }
            public string? SKU { get; set; }
            public int Stock { get; set; }
            public decimal Price { get; set; }
            public List<Category>? Categories { get; set; }

            public class Category { public string? Title { get; set; } }
        }

        private string? _image;
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

        public override void Prepare(EditInventoryItemModel data)
        {
            if (data == null)
            {
                return;
            }

            Type = InventoryItemPageType.Edit;

            Image = data.PictureUrl;

            Item.Name = data.Name;
            Item.Price = data.Price.ToString();

            ProductDetails.SKU = data.SKU;
        }

        public InventoryItemPageType Type { get; set; } = InventoryItemPageType.Create;

        public string Title => Resource.CreateItem;
        public string CreateItemButtonText => Type == InventoryItemPageType.Create ? Resource.CreateItem : Resource.SaveItem;

        public string? AddImageText
        {
            get => _addImageText;
            set => SetProperty(ref _addImageText, value);
        }

        public string? Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public ItemModel Item { get; } = new ItemModel();

        public ProductDetailsModel ProductDetails { get; } = new ProductDetailsModel();

        public StoresModel Stores { get; } = new StoresModel();

        public CategoryModel Category { get; } = new CategoryModel();

        public AlertsModel Alerts { get; } = new AlertsModel();

        public bool? CreateItemButtonEnabled => Item.AreInputsValid;

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnAddImage()
        {
            NavigationService.Navigate<AddImageViewModel, Action<string?>>(OnNewImageSelected);
        }

        public void OnCreateItem() { }

        private void OnNewImageSelected(string? url)
        {
            Image = url;
        }
    }
}