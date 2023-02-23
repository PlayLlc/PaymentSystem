using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using PayWithPlay.Core.ViewModels.SignIn;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public partial class CreateAccountViewModel : BaseViewModel
    {
        private string? _email;

        private string? _password;

        private bool _termsPolicyAccepted;

        public string Title => Resource.CreateAccount;
        public string SignInButtonText => Resource.SignIn;
        public string EmailAddressText => Resource.EmailAddress;
        public string PasswordText => Resource.Password;
        public string HaveAccountQuestionText => Resource.HaveAccountQuestion;
        public string CreateAccountButtonText => Resource.CreateAccount;
        public string TermOfServiceText => Resource.TermsOfService;
        public string PrivacyPolicyText => Resource.PrivacyPolicy;
        public string TermsAndPolicyFullText => $"{Resource.IAgreeToThe} {TermOfServiceText} {Resource.AndText} {PrivacyPolicyText}";

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

        public bool TermsPolicyAccepted
        {
            get => _termsPolicyAccepted;
            set => SetProperty(ref _termsPolicyAccepted, value);
        }

        public void OnSignIn()
        {
            NavigationService.Navigate<SignInViewModel>();
        }

        public void OnCreateAccount()
        {
            NavigationService.Navigate<VerifyEmailViewModel>();
        }

        public void OnTermsOfService()
        {
        }

        public void OnPrivacyPolicy()
        {
        }
    }
}
