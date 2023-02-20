using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration
{
    public class UserAdressViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private string? _streetAdress;
        private string? _apSuite;
        private string? _zipCode;
        private string? _state;
        private string? _city;
        private Action<UserAdressViewModel>? _onContinueAction;

        public UserAdressViewModel(Action<UserAdressViewModel> onContinue)
        {
            _onContinueAction = onContinue;
        }

        public string Title => Resource.UserAdressTitle;
        public string StreetAdressText => Resource.StreeAdress;
        public string ApSuiteText => Resource.ApSuite;
        public string ZipCodeText => Resource.ZipCode;
        public string StateText => Resource.State;
        public string CityText => Resource.City;
        public string SafeMessage => Resource.UserRegistrationSafeMessage;
        public string ContinueText => Resource.Continue;

        public string? StreetAdress
        {
            get => _streetAdress;
            set => SetProperty(ref _streetAdress, value);
        }

        public string? ApSuite
        {
            get => _apSuite;
            set => SetProperty(ref _apSuite, value);
        }

        public string? ZipCode
        {
            get => _zipCode;
            set => SetProperty(ref _zipCode, value);
        }

        public string? State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public string? City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }
    }
}
