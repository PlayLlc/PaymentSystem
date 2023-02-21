using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.CreateAccount;

namespace PayWithPlay.Core.ViewModels.SignIn
{
    public partial class SignInViewModel : BaseViewModel
    {
        private string? _email;

        private string? _password;

        public string Title => Resource.SignIn;
        public string SignInButtonText => Resource.SignIn;
        public string EmailAdressText => Resource.EmailAdress;
        public string PasswordText => Resource.Password;
        public string ForgotPasswordText => Resource.ForgotPassword;
        public string NoAccountQuestionText => Resource.NoAccountQuestion;
        public string CreateAccountButtonText => Resource.CreateAccount;

        public string? Email
        {
            get => _email;
            set => SetProperty(ref _email, value); 
        }

        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public void OnForgotPassword()
        {
        }

        public void OnSignIn()
        {
        }

        public void OnCreateAccount()
        {
            NavigationService.Navigate<CreateAccountViewModel>();
        }
    }
}