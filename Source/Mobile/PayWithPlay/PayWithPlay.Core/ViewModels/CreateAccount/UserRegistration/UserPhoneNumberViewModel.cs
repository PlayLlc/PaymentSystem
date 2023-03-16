using MvvmCross.ViewModels;
using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration
{
    public class UserPhoneNumberViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<UserPhoneNumberViewModel> _onContinueAction;
        private ITextInputValidator? _inputValidator;
        private string? _phoneNumber;

        public UserPhoneNumberViewModel(Action<UserPhoneNumberViewModel> onContinueAction)
        {
            _onContinueAction = onContinueAction;
        }

        public string? PhoneNumber 
        { 
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public bool ContinueButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public string Title => Resource.UserRegistrationUserPhoneNumberTitle;
        public string PhoneNumberText => Resource.PhoneNumberText;
        public string SafeMessage => Resource.UserRegistrationSafeMessage;
        public string ContinueText => Resource.Continue;

        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }

        public void SetInputValidator(ITextInputValidator phoneNumber) 
        {
            ClearValidator();

            _inputValidator = phoneNumber;
            _inputValidator.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.PHONE_NUMBER, Resource.InvalidInputFormat),
            };

            RaisePropertyChanged(() => ContinueButtonEnabled);
        }

        public void ClearValidator()
        {
            _inputValidator = null;
        }
    }
}