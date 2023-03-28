using MvvmCross.ViewModels;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.Models.Inventory.CreateItem
{
    public class ItemModel : MvxNotifyPropertyChanged
    {
        private readonly List<ITextInputValidator> _inputValidators = new();

        private bool _itemExpanded = true;
        private string? _name;
        private string? _price;

        public string ItemText => Resource.Item;
        public string NameText => Resource.Name;
        public string PriceText => Resource.Price;

        public Action? InputChangedAction { get; set; }

        public bool AreInputsValid => ValidationHelper.AreInputsValid(_inputValidators, false);

        public bool ItemExpanded
        {
            get => _itemExpanded;
            set => SetProperty(ref _itemExpanded, value);
        }

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value, InputChangedAction);
        }

        public string? Price
        {
            get => _price;
            set => SetProperty(ref _price, value, InputChangedAction);
        }

        public void OnItem()
        {
            ItemExpanded = !ItemExpanded;
        }

        public void SetInputValidators(ITextInputValidator name, ITextInputValidator price)
        {
            ClearValidators();

            _inputValidators.Add(name);
            _inputValidators.Add(price);

            name.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };

            price.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };

            InputChangedAction?.Invoke();
        }

        public void ClearValidators()
        {
            _inputValidators.Clear();
        }
    }
}
