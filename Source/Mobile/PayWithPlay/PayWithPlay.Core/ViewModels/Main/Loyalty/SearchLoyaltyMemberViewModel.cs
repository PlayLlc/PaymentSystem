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

        public string Title => Resource.SearchMember;
        public string PhoneNumberText => Resource.PhoneNumberText;
        public string SearchButtonText => Resource.Search;

        public bool SearchButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value, () => RaisePropertyChanged(() => SearchButtonEnabled));
        }

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnSearch()
        {
        }

        public void SetInputValidator(ITextInputValidator phoneNumber)
        {
            _inputValidator = phoneNumber;
            _inputValidator.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.PHONE_NUMBER, Resource.InvalidInputFormat)
            };

            RaisePropertyChanged(() => SearchButtonEnabled);
        }
    }
}
