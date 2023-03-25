using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.Inventory.CreateItem
{
    public class ProductDetailsModel : MvxNotifyPropertyChanged
    {
        private string? _sku;
        private string? _description;
        private bool _productDetailsExpanded;

        public string ProductDetailsText => Resource.ProductDetails;
        public string SKUText => Resource.SKU;
        public string DescriptionText => Resource.Descritption;

        public bool ProductDetailsExpanded
        {
            get => _productDetailsExpanded;
            set => SetProperty(ref _productDetailsExpanded, value);
        }

        public string? SKU
        {
            get => _sku;
            set => SetProperty(ref _sku, value);
        }

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public void OnProductDetails()
        {
            ProductDetailsExpanded = !ProductDetailsExpanded;
        }
    }
}