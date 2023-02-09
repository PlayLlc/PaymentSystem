using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public partial class CreateAccountViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _emailOrPhoneNumber;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _termsPolicyAccepted;

        public static string Title => Resource.CreateAccount;
        public static string SignInButtonText => Resource.SignIn;
        public static string EmailOrPhoneNumberText => Resource.EmaiOrPhoneNumber;
        public static string PasswordText => Resource.Password;
        public static string HaveAccountQuestionText => Resource.HaveAccountQuestion;
        public static string CreateAccountButtonText => Resource.CreateAccount;
        public static string TermOfServiceText => Resource.TermsOfService;
        public static string PrivacyPolicyText => Resource.PrivacyPolicy;
        public static string TermsAndPolicyFullText => $"{Resource.IAgreeToThe} {TermOfServiceText} {Resource.AndText} {PrivacyPolicyText}";


        public interface INavigationService
        {
            void NavigateToNextPage();

            void NavigateToSignIn();
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
            NavigationService?.NavigateToNextPage();
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
