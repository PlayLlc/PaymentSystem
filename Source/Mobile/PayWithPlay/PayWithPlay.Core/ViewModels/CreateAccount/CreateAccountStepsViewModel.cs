using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.CreateAccount.MerchantRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public class CreateAccountStepsViewModel : BaseViewModel
    {
        private int _maxProgress;
        private int _currentProgress;

        public CreateAccountStepsViewModel()
        {
            StepsModels.Add(new UserNameViewModel(OnContinue));
            StepsModels.Add(new UserPhoneNumberViewModel(OnContinue));
            StepsModels.Add(new VerifyPhoneNumberViewModel(OnContinue));
            StepsModels.Add(new RegistrationAddressViewModel(OnContinue, RegistrationAddressType.User));
            StepsModels.Add(new HomeOrBusinessAddressViewModel(OnContinue));
            StepsModels.Add(new RegistrationAddressViewModel(OnContinue, RegistrationAddressType.Business));
            StepsModels.Add(new MerchantTypeViewModel(OnContinue));
            StepsModels.Add(new BusinessNameViewModel(OnContinue));

            CurrentPageIndex = 0;
            MaxProgress = StepsModels.Count;
        }

        public string Title => Resource.UserRegistration;

        public Action<int>? ScrollToPageAction { get; set; }

        public ObservableCollection<ICreateAccountStep> StepsModels { get; } = new ObservableCollection<ICreateAccountStep>();

        public int CurrentPageIndex { get; set; }

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
            if(model is UserNameViewModel userNameViewModel)
            {
                CurrentPageIndex = 1;
                ScrollToPageAction?.Invoke(1);
                CurrentProgress = 1;
            }
            else if (model is UserPhoneNumberViewModel userPhoneNumberViewModel)
            {
                CurrentPageIndex = 2;

                ScrollToPageAction?.Invoke(2);
                CurrentProgress = 2;
            }
            else if(model is VerifyPhoneNumberViewModel verifyPhoneNumberViewModel)
            {
                CurrentPageIndex = 3;
                ScrollToPageAction?.Invoke(3);
                CurrentProgress = 3;
            }
            else if (model is RegistrationAddressViewModel registrationAddressViewModel)
            {
                if(registrationAddressViewModel.RegistrationAddressType == RegistrationAddressType.User)
                {
                    CurrentPageIndex = 4;
                    ScrollToPageAction?.Invoke(4);
                    CurrentProgress = 4;
                }
                else
                {
                    CurrentPageIndex = 6;
                    ScrollToPageAction?.Invoke(6);
                    CurrentProgress = 6;
                }
            }
            else if(model is HomeOrBusinessAddressViewModel homeOrBusinessAddressViewModel)
            {
                if (homeOrBusinessAddressViewModel.SameAddress)
                {
                    CurrentPageIndex = 6;
                    ScrollToPageAction?.Invoke(6);
                    CurrentProgress = 6;
                }
                else
                {
                    CurrentPageIndex = 5;
                    ScrollToPageAction?.Invoke(5);
                    CurrentProgress = 5;
                }
            }
            else if (model is MerchantTypeViewModel merchantTypeViewModel)
            {
                CurrentPageIndex = 7;
                ScrollToPageAction?.Invoke(7);
                CurrentProgress = 7;
            }
            else if(model is BusinessNameViewModel businessNameViewModel)
            {

            }
        }
    }
}