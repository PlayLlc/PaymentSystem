using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class LoyaltySetDiscountViewModel : BaseViewModel<InventoryItemModel>
    {
        private readonly List<RadioButtonModel> _discountsButtons = new()
        {
            new RadioButtonModel
            {
                Title = Resource.Amount,
                Type = (int)DiscountType.Amount,
                Color = RadioButtonModel.ColorType.LightBlue
            },
            new RadioButtonModel
            {
                Title = Resource.Percentage,
                Type = (int)DiscountType.Percentage,
                Color = RadioButtonModel.ColorType.LightBlue
            },
        };

        private int? _selectedDiscountType;
        private string? _discountValue;
        private string? _suffixValue;
        private string? _prefixValue;

        public LoyaltySetDiscountViewModel()
        {
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            DiscountTypeChangedAction = null;
            base.ViewDestroy(viewFinishing);
        }

        public override void Prepare(InventoryItemModel parameter)
        {
            if(parameter == null)
            {
                return;
            }

            InventoryItem = parameter;

            if (parameter.DiscountType == null)
            {
                SelectedDiscountType = (int)DiscountType.Amount;
            }
            else
            {
                SelectedDiscountType = (int)parameter.DiscountType;
            }
        }

        public string Title => Resource.ItemDiscount;
        public string DiscountText => Resource.Discount;
        public string SaveButtonText => Resource.Save;
        public string DiscountHint => "0";

        public Action? DiscountTypeChangedAction { get; set; }

        public List<RadioButtonModel> DiscountsButtons => _discountsButtons;

        public int? SelectedDiscountType
        {
            get => _selectedDiscountType;
            set => SetProperty(ref _selectedDiscountType, value, () => 
            {
                DiscountTypeChangedAction?.Invoke();

                DiscountValue = string.Empty;
                if (_selectedDiscountType == (int)DiscountType.Amount) 
                {
                    PrefixValue = "$";
                    SuffixValue = string.Empty;
                }
                else
                {
                    PrefixValue = string.Empty;
                    SuffixValue = "%";
                }
            });
        }

        public string? DiscountValue
        {
            get => _discountValue;
            set => SetProperty(ref _discountValue, value);
        }

        public string? SuffixValue
        {
            get => _suffixValue;
            set => SetProperty(ref _suffixValue, value);
        }

        public string? PrefixValue
        {
            get => _prefixValue;
            set => SetProperty(ref _prefixValue, value);
        }

        public InventoryItemModel? InventoryItem { get; set; }

        public List<ChipModel>? ItemCategories => InventoryItem?.Categories.Where(t => t.Type == ChipType.ItemCategory).ToList();

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnSave()
        {
            NavigationService.Close(this);
        }
    }
}
