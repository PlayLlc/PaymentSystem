using MvvmCross.ViewModels;
using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Models.CreateAccount;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.CreateAccount.UserRegistration
{
    public class UserNameViewModel : MvxNotifyPropertyChanged, ICreateAccountStep
    {
        private readonly List<ITextInputValidator> _inputValidators = new();
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
            set => SetProperty(ref _firstName, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public string? LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public bool ContinueButtonEnabled => ValidationHelper.AreInputsValid(_inputValidators, false);

        public string Title => Resource.UserRegistrationUserNameTitle;
        public string FirstNameText => Resource.FirstNameText;
        public string LastNameText => Resource.LastNameText;
        public string SafeMessage => Resource.UserRegistrationSafeMessage;
        public string ContinueText => Resource.Continue;


        public void OnContinue()
        {
            _onContinueAction?.Invoke(this);
        }

        public void SetInputValidators(ITextInputValidator firstName, ITextInputValidator lastName)
        {
            ClearValidators();

            _inputValidators.Add(firstName);
            _inputValidators.Add(lastName);

            firstName.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.USER_NAME, Resource.InvalidInputFormat),
                new MinLengthValidation(2)
            };
            lastName.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.USER_NAME, Resource.InvalidInputFormat),
                new MinLengthValidation(2)
            };

            RaisePropertyChanged(() => ContinueButtonEnabled);
        }

        public void ClearValidators()
        {
            _inputValidators.Clear();
        }
    }
}