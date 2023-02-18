using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration
{
    public class UserNameViewModel : BaseViewModel, ICreateAccountStep
    {
        private readonly Action<UserNameViewModel> _onContinueAction;
        private string? _firstName;
        private string? _lastName;

        public UserNameViewModel(Action<UserNameViewModel> onContinueAction)
        {
            _onContinueAction = onContinueAction;
        }

        public string? FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string? LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Title => Resource.UserRegistrationUserNameTitle;
        public string FirstNameText => Resource.FirstNameText;
        public string LastNameText => Resource.LastNameText;
        public string SafeMessage => Resource.UserRegistrationSafeMessage;
        public string ContinueText => Resource.Continue;

        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }
    }
}