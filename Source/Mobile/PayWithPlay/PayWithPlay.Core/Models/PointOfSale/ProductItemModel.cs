using MvvmCross.ViewModels;

namespace PayWithPlay.Core.Models.PointOfSale
{
    public class ProductItemModel : MvxNotifyPropertyChanged
    {
        private bool _selected;

        public Action<ProductItemModel>? AddAction { get; set; }
        public Action<ProductItemModel>? RemoveAction { get; set; }

        public Guid ProductItemId { get; set; }

        public string? Title { get; set; }

        public decimal Price { get; set; }

        public string DisplayPrice => $"${Price:0.00}";

        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public void OnAdd()
        {
            AddAction?.Invoke(this);
        }

        public void OnRemove()
        {
            RemoveAction?.Invoke(this);
        }

        public ProductItemModel Clone()
        {
            return new ProductItemModel
            {
                ProductItemId = ProductItemId,
                Title = Title,
                Price = Price,
                AddAction = AddAction,
                RemoveAction = RemoveAction,
            };
        }
    }
}
