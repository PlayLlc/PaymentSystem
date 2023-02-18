using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public class CreateAccountStepsViewModel : BaseViewModel
    {
        public CreateAccountStepsViewModel()
        {
            StepsModels.Add(new UserNameViewModel(OnContinue));
            StepsModels.Add(new UserPhoneNumberViewModel(OnContinue));
            StepsModels.Add(new VerifyPhoneNumberViewModel(OnContinue));
        }

        public Action<int>? ScrollToPageAction { get; set; }

        public ObservableCollection<ICreateAccountStep> StepsModels { get; } = new ObservableCollection<ICreateAccountStep>();

        private void OnContinue(object model)
        {
            if(model is UserNameViewModel userNameViewModel)
            {
                ScrollToPageAction?.Invoke(1);
            }
            else if (model is UserPhoneNumberViewModel userPhoneNumberViewModel)
            {
                ScrollToPageAction?.Invoke(2);
            }
            else if(model is VerifyPhoneNumberViewModel verifyPhoneNumberViewModel)
            {
                //ScrollToPageAction?.Invoke(2);
            }
        }
    }
}