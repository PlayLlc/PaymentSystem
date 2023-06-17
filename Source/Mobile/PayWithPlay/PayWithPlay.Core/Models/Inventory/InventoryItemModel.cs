using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.Inventory.CreateItem;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.Inventory
{
    public class InventoryItemModel : MvxNotifyPropertyChanged
    {
        private bool _hasDiscount;
        private bool _selected;
        private bool _deleting;

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

        public List<ChipModel>? Categories { get; set; }

        public string DisplayedStock => $"{Resource.Stock}: {Stock}";
        public string DisplayedPrice => GetDisplayedPrice();
        public string InitialPrice => $"${Price:0.00}";

        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value, () => 
            {
                if (!_selected) 
                {
                    Deleting = false;
                }
            });
        }

        public bool Deleting
        {
            get => _deleting;
            set => SetProperty(ref _deleting, value);
        }

        public bool HasDiscount
        {
            get => _hasDiscount;
            set => SetProperty(ref _hasDiscount, value);
        }

        public DiscountType? DiscountType { get; set; }

        public decimal DiscountValue { get; set; }

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
            Deleting = true;
        }

        public void OnSwipeToDelete() 
        {
            Task.Run(async () =>
            {
                await Task.Delay(200);

                OnDeleteAction?.Invoke(this);
            });
        }

        private string GetDisplayedPrice()
        {
            if (HasDiscount && DiscountType != null)
            {
                if(DiscountType.Value == Enums.DiscountType.Amount)
                {
                    return $"${Price - DiscountValue:0.00}";
                }
                else
                {
                    return $"${Price - Price * DiscountValue / 100:0.00}";
                }
            }
            else
            {
                return $"${Price:0.00}";
            }
        }
    }
}
