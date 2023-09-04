using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using System.Drawing;

namespace PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration
{
    public class RegistrationAddressViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly List<ITextInputValidator> _inputValidators = new();
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
        public string SafeMessage => Resource.InformationSafeMessage;
        public string ContinueText => Resource.Continue;

        public string? StreetAddress
        {
            get => _streetAddress;
            set => SetProperty(ref _streetAddress, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public string? ApSuite
        {
            get => _apSuite;
            set => SetProperty(ref _apSuite, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public string? ZipCode
        {
            get => _zipCode;
            set => SetProperty(ref _zipCode, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public string? State
        {
            get => _state;
            set => SetProperty(ref _state, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public string? City
        {
            get => _city;
            set => SetProperty(ref _city, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public bool ContinueButtonEnabled => ValidationHelper.AreInputsValid(_inputValidators, false);

        public RegistrationAddressType RegistrationAddressType { get; }

        public void OnState()
        {
            var navigationService = MvxIoCProvider.Instance!.Resolve<IMvxNavigationService>();

            navigationService.Navigate<StatesSelectionViewModel, BaseItemSelectionViewModel.NavigationData>(new BaseItemSelectionViewModel.NavigationData
            {
                ResultItemsAction = (items) =>
                {
                    var selectedState = items.First().Name!;
                    State = selectedState.Substring(selectedState.LastIndexOf(',') + 2, 2);
                },
                SelectionType = ItemSelectionType.Single
            });
        }

        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }

        public void SetInputValidators(ITextInputValidator streetAddress, ITextInputValidator apSuite, ITextInputValidator zipCode, ITextInputValidator state, ITextInputValidator city)
        {
            ClearValidators();

            _inputValidators.Add(streetAddress);
            _inputValidators.Add(apSuite);
            _inputValidators.Add(zipCode);
            _inputValidators.Add(state);
            _inputValidators.Add(city);

            streetAddress.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };
            apSuite.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };
            zipCode.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };
            state.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };
            city.Validations = new List<IValidation>
            {
                new NonEmptyValidation()
            };

            RaisePropertyChanged(() => ContinueButtonEnabled);
        }

        public void ClearValidators()
        {
            _inputValidators.Clear();
        }
    }
}