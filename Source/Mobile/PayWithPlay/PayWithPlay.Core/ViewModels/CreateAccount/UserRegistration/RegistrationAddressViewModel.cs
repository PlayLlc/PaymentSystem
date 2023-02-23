using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration
{
    public class RegistrationAddressViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly Action<RegistrationAddressViewModel>? _onContinueAction;
        private string? _streetAddress;
        private string? _apSuite;
        private string? _zipCode;
        private string? _state;
        private string? _city;

        public RegistrationAddressViewModel(Action<RegistrationAddressViewModel> onContinue, RegistrationAddressType registrationAddressType)
        {
            _onContinueAction = onContinue;
            RegistrationAddressType = registrationAddressType;
        }

        public string Title => RegistrationAddressType == RegistrationAddressType.User ? Resource.UserAddressTitle : Resource.BusinessAddressTitle;
        public string StreetAddressText => Resource.StreetAddress;
        public string ApSuiteText => Resource.ApSuite;
        public string ZipCodeText => Resource.ZipCode;
        public string StateText => Resource.State;
        public string CityText => Resource.City;
        public string SafeMessage => Resource.UserRegistrationSafeMessage;
        public string ContinueText => Resource.Continue;

        public string? StreetAddress
        {
            get => _streetAddress;
            set => SetProperty(ref _streetAddress, value);
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
        public RegistrationAddressType RegistrationAddressType { get; }

        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }
    }
}