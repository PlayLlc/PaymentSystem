using MvvmCross.ViewModels;
using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.CreateAccount.MerchantRegistration
{
    public class BusinessNameViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<BusinessNameViewModel> _onContinueAction;
        private ITextInputValidator? _inputValidator;

        private string? _businessName;

        public BusinessNameViewModel(Action<BusinessNameViewModel> onContinueAction)
        {
            _onContinueAction = onContinueAction;
        }

        public string? BusinessName
        {
            get => _businessName;
            set => SetProperty(ref _businessName, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public bool ContinueButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public string Title => Resource.BusinessNameTitle;
        public string BusinessNameText => Resource.BusinessName;
        public string ContinueText => Resource.Continue;

        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }

        public void SetInputValidator(ITextInputValidator inputValidator)
        {
            ClearValidator();

            _inputValidator = inputValidator;
            _inputValidator.Validations = new List<IValidation>
            {
                new NonEmptyValidation(),
            };

            RaisePropertyChanged(() => ContinueButtonEnabled);
        }

        public void ClearValidator()
        {
            _inputValidator = null;
        }
    }
}