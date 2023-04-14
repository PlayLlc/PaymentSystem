using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class CreateLoyaltyMemberViewModel : BaseViewModel
    {
        private readonly List<ITextInputValidator> _inputValidators = new();

        private string? _phoneNumber;
        private string? _name;
        private string? _email;

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _inputValidators.Clear();
        }

        public string Title => Resource.LoyaltyMember;
        public string PhoneNumberText => Resource.PhoneNumberText;
        public string NameText => Resource.Name;
        public string EmailText => Resource.Email;
        public string CreateButtonText => Resource.Create;

        public bool CreateButtonEnabled => ValidationHelper.AreInputsValid(_inputValidators, false);

        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value, () => RaisePropertyChanged(() => CreateButtonEnabled));
        }

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value, () => RaisePropertyChanged(() => CreateButtonEnabled));
        }

        public string? Email
        {
            get => _email;
            set => SetProperty(ref _email, value, () => RaisePropertyChanged(() => CreateButtonEnabled));
        }

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnCreate() { }

        public void SetInputValidators(ITextInputValidator phoneNumber, ITextInputValidator name, ITextInputValidator email)
        {
            _inputValidators.Clear();

            _inputValidators.Add(phoneNumber);
            _inputValidators.Add(name);
            _inputValidators.Add(email);

            phoneNumber.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.PHONE_NUMBER, Resource.InvalidInputFormat)
            };
            name.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.USER_NAME, Resource.InvalidInputFormat) { IsOptional = true },
                new MinLengthValidation(2) { IsOptional = true }
            };
            email.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.EMAIL, Resource.InvalidInputFormat) { IsOptional = true }
            };

            RaisePropertyChanged(() => CreateButtonEnabled);
        }
    }
}
