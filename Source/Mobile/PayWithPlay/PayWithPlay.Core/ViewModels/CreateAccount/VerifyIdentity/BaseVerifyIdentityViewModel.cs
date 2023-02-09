using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity
{
    public abstract partial class BaseVerifyIdentityViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _inputValue;

        public enum VerifyIdentity { Email, PhoneNumber }

        public interface INavigationService
        {
            void NavigateToNextPage();
        }

        public INavigationService? NavigationService { get; set; }

        public abstract VerifyIdentity VerifyIdentityType { get; }

        public abstract string Title { get; } 

        public abstract string Subtitle { get; }

        public abstract string Message { get; }

        public static string VerifyButtonText => Resource.Verify;
        public static string ExipresAfter => Resource.ExpiresAfter3;
        public static string ResendCodeButtonText => Resource.ResendCode;

        [RelayCommand]
        public void OnVerify()
        {
            NavigationService?.NavigateToNextPage();
        }

        [RelayCommand]
        public void OnResend()
        {
        }
    }
}
