using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity
{
    public abstract partial class BaseVerifyIdentityViewModel : BaseViewModel
    {
        private string? _inputValue;

        public abstract VerifyIdentityType VerifyIdentityType { get; }

        public abstract TextStyleType TitleTextStyleType { get; }

        public abstract string Title { get; }

        public abstract string Subtitle { get; }

        public abstract string Message { get; }

        public string VerifyButtonText => Resource.Verify;
        public string ExipresAfter => Resource.ExpiresAfter3;
        public string ResendCodeButtonText => Resource.ResendCode;

        public string? InputValue
        {
            get => _inputValue;
            set => SetProperty(ref _inputValue, value);
        }

        public abstract void OnVerify();

        public void OnResend()
        {
        }
    }
}