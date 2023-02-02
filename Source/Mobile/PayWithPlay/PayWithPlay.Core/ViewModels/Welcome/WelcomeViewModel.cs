using CommunityToolkit.Mvvm.Input;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Welcome
{
    public partial class WelcomeViewModel : BaseViewModel
    {
        public static string Title => Resource.WelcomeTo;
        public static string SignInButtonText => Resource.SignIn;
        public static string CreateAccountButtonText => Resource.CreateAccount;
        public static string TAndCButtonText => Resource.TermsOfService;
        public static string PrivacyPolicyButtonText => Resource.PrivacyPolicy;

        public interface INavigationService
        {
            void NavigateToSignIn();

            void NavigateToCreateAccount();
        }

        public INavigationService? NavigationService { get; set; }

        [RelayCommand]
        public void OnSignIn()
        {
            NavigationService?.NavigateToSignIn();
        }

        [RelayCommand]
        public void OnCreateAccount()
        {
            NavigationService?.NavigateToCreateAccount();
        }

        [RelayCommand]
        public void OnTermsOfService()
        {
        }

        [RelayCommand]
        public void OnPrivacyPolicy()
        {
        }
    }
}