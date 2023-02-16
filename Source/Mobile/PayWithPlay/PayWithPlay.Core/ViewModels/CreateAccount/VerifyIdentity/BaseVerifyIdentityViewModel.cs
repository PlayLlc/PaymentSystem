using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity
{
    public abstract partial class BaseVerifyIdentityViewModel : BaseViewModel
    {
        private string? _inputValue;

        public enum VerifyIdentity { Email, PhoneNumber }

        public abstract VerifyIdentity VerifyIdentityType { get; }

        public abstract string Title { get; }

        public abstract string Subtitle { get; }

        public abstract string Message { get; }

        public static string VerifyButtonText => Resource.Verify;
        public static string ExipresAfter => Resource.ExpiresAfter3;
        public static string ResendCodeButtonText => Resource.ResendCode;

        public string? InputValue
        {
            get => _inputValue;
            set => SetProperty(ref _inputValue, value);
        }

        public void OnVerify()
        {
            NavigationService.Navigate<EnableDeviceSettingsViewModel>();
        }

        public void OnResend()
        {
        }
    }
}
