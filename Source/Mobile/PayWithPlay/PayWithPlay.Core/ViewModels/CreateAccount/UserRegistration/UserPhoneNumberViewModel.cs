using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration
{
    public class UserPhoneNumberViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<UserPhoneNumberViewModel> _onContinueAction;
        private string? _phoneNumber;

        public UserPhoneNumberViewModel(Action<UserPhoneNumberViewModel> onContinueAction)
        {
            _onContinueAction = onContinueAction;
        }

        public string? PhoneNumber 
        { 
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public string Title => Resource.UserRegistrationUserPhoneNumberTitle;
        public string PhoneNumberText => Resource.PhoneNumberText;
        public string SafeMessage => Resource.UserRegistrationSafeMessage;
        public string ContinueText => Resource.Continue;

        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }
    }
}