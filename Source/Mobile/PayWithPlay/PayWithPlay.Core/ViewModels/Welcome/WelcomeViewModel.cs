using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.CreateAccount;
using PayWithPlay.Core.ViewModels.SignIn;

namespace PayWithPlay.Core.ViewModels.Welcome
{
    public partial class WelcomeViewModel : BaseViewModel
    {
        public string Title => Resource.WelcomeTo;
        public string SignInButtonText => Resource.SignIn;
        public string CreateAccountButtonText => Resource.CreateAccount;
        public string TAndCButtonText => Resource.TermsOfService;
        public string PrivacyPolicyButtonText => Resource.PrivacyPolicy;

        public void OnSignIn()
        {
            NavigationService.Navigate<SignInViewModel>();
        }

        public void OnCreateAccount()
        {
            NavigationService.Navigate<CreateAccountViewModel>();
        }

        public void OnTermsOfService()
        {
        }

        public void OnPrivacyPolicy()
        {
        }
    }
}