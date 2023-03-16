using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.CreateAccount;
using PayWithPlay.Core.ViewModels.Main;
using PayWithPlay.Core.ViewModels.PIN;

namespace PayWithPlay.Core.ViewModels.SignIn
{
    public partial class SignInViewModel : BaseViewModel
    {
        private string? _email;

        private string? _password;

        public string Title => Resource.SignIn;
        public string SignInButtonText => Resource.SignIn;
        public string EmailAddressText => Resource.EmailAddress;
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
            //NavigationService.Navigate<PINViewModel>(new MvxBundle(new Dictionary<string, string> { { PINViewModel.PINSignIn, string.Empty } }));
            NavigationService.Navigate<MainViewModel>();
        }

        public void OnCreateAccount()
        {
            NavigationService.Navigate<CreateAccountViewModel>();
        }
    }
}