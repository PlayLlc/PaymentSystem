using CommunityToolkit.Mvvm.Input;
using PayWithPlay.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayWithPlay.Core.ViewModels.SignIn
{
    public partial class SignInViewModel : BaseViewModel
    {
        public static string Title = Resource.SignIn;
        public static string SignInButtonText => Resource.SignIn;
        public static string NoAccountQuestionText => Resource.NoAccountQuestion;
        public static string CreateAccountButtonText => Resource.CreateAccount;

        public interface INavigationService
        {
            void NavigateToCreateAccount();
        }

        public INavigationService? NavigationService { get; set; }


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
