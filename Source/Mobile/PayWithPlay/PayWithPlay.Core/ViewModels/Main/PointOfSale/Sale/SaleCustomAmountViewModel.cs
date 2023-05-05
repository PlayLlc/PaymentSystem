using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleCustomAmountViewModel : BaseViewModel<Action<string>>
    {
        private ITextInputValidator? _inputValidator;

        private string? _customAmount;
        private Action<string>? _onAddAction;

        public string CustomAmountText => Resource.CustomAmount;
        public string AddButtonText => Resource.Add;

        public bool AddButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);


        public override void ViewDestroy(bool viewFinishing = true)
        {
            _inputValidator = null;
            _onAddAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public override void Prepare(Action<string> parameter)
        {
            _onAddAction = parameter;
        }

        public string? CustomAmount
        {
            get => _customAmount;
            set => SetProperty(ref _customAmount, value, () => RaisePropertyChanged(() => AddButtonEnabled));
        }

        public void OnAdd()
        {
            _onAddAction?.Invoke(CustomAmount);
            CustomAmount = string.Empty;
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
