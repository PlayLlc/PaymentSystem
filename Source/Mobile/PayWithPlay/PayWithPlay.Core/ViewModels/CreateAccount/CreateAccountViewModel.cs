using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using PayWithPlay.Core.ViewModels.SignIn;
using System.Collections.Generic;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public partial class CreateAccountViewModel : BaseViewModel
    {
        private readonly List<ITextInputValidator> _inputValidations = new();

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
            set => SetProperty(ref _email, value, () => RaisePropertyChanged(() => CreateAccountButtonEnabled));
        }

        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value, () => RaisePropertyChanged(() => CreateAccountButtonEnabled));
        }

        public bool TermsPolicyAccepted
        {
            get => _termsPolicyAccepted;
            set => SetProperty(ref _termsPolicyAccepted, value, () => RaisePropertyChanged(() => CreateAccountButtonEnabled));
        }

        public bool CreateAccountButtonEnabled => ValidationHelper.AreInputsValid(_inputValidations, false) && TermsPolicyAccepted;

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _inputValidations.Clear();
        }

        public void OnCreateAccount()
        {
            NavigationService.Navigate<VerifyEmailViewModel>();
        }

        public void OnSignIn()
        {
            NavigationService.Navigate<SignInViewModel>();
        }

        public void OnTermsOfService()
        {
        }

        public void OnPrivacyPolicy()
        {
        }

        public void SetInputValidations(ITextInputValidator email, ITextInputValidator password)
        {
            _inputValidations.Add(email);
            _inputValidations.Add(password);

            email.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.EMAIL, Resource.InvalidInputFormat),
            };
            password.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.PASSWORD, Resource.InvalidInputFormat),
                new MinLengthValidation(7)
            };

            RaisePropertyChanged(() => CreateAccountButtonEnabled);
        }
    }
}