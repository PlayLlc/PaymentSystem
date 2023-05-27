using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleCustomAmountViewModel : BaseViewModel<Action<Tuple<string?, string?>>>
    {
        private ITextInputValidator? _inputValidator;

        private string? _customAmount;
        private string? _description;
        private Action<Tuple<string?, string?>>? _onAddAction;

        public string CustomAmountText => Resource.CustomAmount;
        public string DescriptionText => Resource.Description;
        public string AddButtonText => Resource.Add;

        public bool AddButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public Action? ClearFocusAction { get; set; }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _inputValidator = null;
            _onAddAction = null;
            ClearFocusAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public override void Prepare(Action<Tuple<string?, string?>> parameter)
        {
            _onAddAction = parameter;
        }

        public string? CustomAmount
        {
            get => _customAmount;
            set => SetProperty(ref _customAmount, value, () => RaisePropertyChanged(() => AddButtonEnabled));
        }

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public void OnAdd()
        {
            ClearFocusAction?.Invoke();

            _onAddAction?.Invoke(Tuple.Create(CustomAmount, Description));
            CustomAmount = string.Empty;
            Description = string.Empty;
        }

        public void SetInputValidator(ITextInputValidator phoneNumber)
        {
            _inputValidator = phoneNumber;
            _inputValidator.Validations = new List<IValidation>
            {
                new NonEmptyValidation() { IsOptional = true }
            };

            RaisePropertyChanged(() => AddButtonEnabled);
        }
    }
}
