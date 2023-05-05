using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.Inventory
{
    public class InventoryItemModel : MvxNotifyPropertyChanged
    {
        private bool _selected;

        public string SalePiceText => Resource.SalePrice;
        public string EditItemText => Resource.EditItem;
        public string DeleteItemText => Resource.DeleteItem;
        public string ManageStockText => Resource.ManageStock;

        public Action<InventoryItemModel>? OnEditAction { get; set; }
        public Action<InventoryItemModel>? OnManageStockAction { get; set; }
        public Action<InventoryItemModel>? OnDeleteAction { get; set; }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? PictureUrl { get; set; }

        public string? SKU { get; set; }

        public int Stock { get; set; }

        public decimal Price { get; set; }

        public List<CategoryItemModel>? Categories { get; set; }

        public string DisplayedStock => $"{Resource.Stock}: {Stock}";
        public string DisplayedPrice => $"${Price}";

        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public void OnEdit()
        {
            OnEditAction?.Invoke(this);
        }

        public void OnManageStock()
        {
            OnManageStockAction?.Invoke(this);
        }

        public void OnDelete() 
        {
            OnDeleteAction?.Invoke(this);
        }
    }
}
