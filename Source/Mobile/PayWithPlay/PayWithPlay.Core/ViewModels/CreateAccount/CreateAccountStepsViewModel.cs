using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.CreateAccount.MerchantRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using PayWithPlay.Core.ViewModels.PIN;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public class CreateAccountStepsViewModel : BaseViewModel
    {
        private string? _title;
        private int _maxProgress;
        private int _currentProgress;

        public CreateAccountStepsViewModel()
        {
            Title = Resource.UserRegistration;

            StepsModels.Add(new UserNameViewModel(OnContinue));
            StepsModels.Add(new UserPhoneNumberViewModel(OnContinue));
            StepsModels.Add(new VerifyPhoneNumberViewModel(OnContinue));
            StepsModels.Add(new RegistrationAddressViewModel(OnContinue, RegistrationAddressType.User));
            StepsModels.Add(new HomeOrBusinessAddressViewModel(OnContinue));
            StepsModels.Add(new RegistrationAddressViewModel(OnContinue, RegistrationAddressType.Business));
            StepsModels.Add(new MerchantTypeViewModel(OnContinue));
            StepsModels.Add(new BusinessNameViewModel(OnContinue));
            StepsModels.Add(new PINViewModel(OnContinue, PINPageType.Create));
            StepsModels.Add(new PINViewModel(OnContinue, PINPageType.CreateConfirm));
            StepsModels.Add(new BiometricsRegistrationViewModel(OnContinue));
            StepsModels.Add(new CongratsRegistrationViewModel(OnContinue));

            CurrentPageIndex = 0;
            MaxProgress = StepsModels.Count;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            ((UserNameViewModel)StepsModels.First(t => t is UserNameViewModel)).ClearValidators();
            ((UserPhoneNumberViewModel)StepsModels.First(t => t is UserPhoneNumberViewModel)).ClearValidator();
            ((RegistrationAddressViewModel)StepsModels.First(t => t is RegistrationAddressViewModel)).ClearValidators();
            ((BusinessNameViewModel)StepsModels.First(t => t is BusinessNameViewModel)).ClearValidator();

            base.ViewDestroy(viewFinishing);
        }

        public Action<int>? ScrollToPageAction { get; set; }

        public ObservableCollection<ICreateAccountStep> StepsModels { get; } = new ObservableCollection<ICreateAccountStep>();

        public int CurrentPageIndex { get; set; }

        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public int MaxProgress
        {
            get => _maxProgress;
            set => SetProperty(ref _maxProgress, value);
        }

        public int CurrentProgress
        {
            get => _currentProgress;
            set => SetProperty(ref _currentProgress, value);
        }

        private void OnContinue(object model)
        {
            var nextPageIndex = CurrentPageIndex;
            var nextProgress = CurrentProgress;

            if (model is UserNameViewModel userNameViewModel)
            {
                nextPageIndex = 1;
                nextProgress = 1;
            }
            else if (model is UserPhoneNumberViewModel userPhoneNumberViewModel)
            {
                nextPageIndex = 2;
                nextProgress = 2;
            }
            else if (model is VerifyPhoneNumberViewModel verifyPhoneNumberViewModel)
            {
                nextPageIndex = 3;
                nextProgress = 3;
            }
            else if (model is RegistrationAddressViewModel registrationAddressViewModel)
            {
                if (registrationAddressViewModel.RegistrationAddressType == RegistrationAddressType.User)
                {
                    Title = Resource.MerchantRegistration;
                    nextPageIndex = 4;
                    nextProgress = 4;
                }
                else
                {
                    nextPageIndex = 6;
                    nextProgress = 6;
                }
            }
            else if (model is HomeOrBusinessAddressViewModel homeOrBusinessAddressViewModel)
            {
                if (homeOrBusinessAddressViewModel.SameAddress)
                {
                    nextPageIndex = 6;
                    nextProgress = 6;
                }
                else
                {
                    nextPageIndex = 5;
                    nextProgress = 5;
                }
            }
            else if (model is MerchantTypeViewModel merchantTypeViewModel)
            {
                nextPageIndex = 7;
                nextProgress = 7;
            }
            else if (model is BusinessNameViewModel businessNameViewModel)
            {
                nextPageIndex = 8;
                nextProgress = 8;
                Title = Resource.Approval;
            }
            else if (model is PINViewModel pINViewModel)
            {
                if (pINViewModel.Type == PINPageType.Create)
                {
                    nextPageIndex = 9;
                    nextProgress = 9;
                }
                else
                {
                    nextPageIndex = 10;
                    nextProgress = 10;
                }
            }
            else if (model is BiometricsRegistrationViewModel biometricsRegistrationViewModel)
            {
                nextPageIndex = 11;
                nextProgress = 11;
            }

            CurrentPageIndex = nextPageIndex;
            ScrollToPageAction?.Invoke(nextPageIndex);
            CurrentProgress = nextProgress;
        }
    }
}