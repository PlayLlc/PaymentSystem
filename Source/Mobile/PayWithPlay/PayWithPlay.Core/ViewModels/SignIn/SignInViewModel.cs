using CommunityToolkit.Mvvm.Input;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.SignIn
{
    public partial class SignInViewModel : BaseViewModel
    {
        public static string Title => Resource.SignIn;
        public static string SignInButtonText => Resource.SignIn;
        public static string EmailAdressText => Resource.EmailAdress;
        public static string PasswordText => Resource.Password;
        public static string ForgotPasswordText => Resource.ForgotPassword;
        public static string NoAccountQuestionText => Resource.NoAccountQuestion;
        public static string CreateAccountButtonText => Resource.CreateAccount;

        public interface INavigationService
        {
            void NavigateToCreateAccount();
        }

        public INavigationService? NavigationService { get; set; }


        [RelayCommand]
        public void OnForgotPassword()
        {
        }

        [RelayCommand]
        public void OnSignIn()
        {
        }

        [RelayCommand]
        public void OnCreateAccount()
        {
            NavigationService?.NavigateToCreateAccount();
        }
    }
}
