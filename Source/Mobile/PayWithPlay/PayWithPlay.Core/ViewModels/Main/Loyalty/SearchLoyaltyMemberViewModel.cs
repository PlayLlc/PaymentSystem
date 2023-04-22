using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class SearchLoyaltyMemberViewModel : BaseViewModel
    {
        private ITextInputValidator? _inputValidator;

        private string? _phoneNumber;

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _inputValidator = null;
        }

        public virtual string Title => Resource.SearchMember;
        public virtual string InputText => Resource.PhoneNumberText;
        public virtual string NextButtonText => Resource.Search;

        public bool NextButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value, () => RaisePropertyChanged(() => NextButtonEnabled));
        }

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public virtual void OnNext()
        {
            NavigationService.Navigate<LoyaltyMemberViewModel>();
        }

        public void SetInputValidator(ITextInputValidator phoneNumber)
        {
            _inputValidator = phoneNumber;
            _inputValidator.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.PHONE_NUMBER, Resource.InvalidInputFormat)
            };

            RaisePropertyChanged(() => NextButtonEnabled);
        }
    }
}
