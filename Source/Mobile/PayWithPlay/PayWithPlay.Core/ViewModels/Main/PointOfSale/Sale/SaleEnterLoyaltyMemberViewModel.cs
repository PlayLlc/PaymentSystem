using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleEnterLoyaltyMemberViewModel : BaseViewModel
    {
        private ITextInputValidator? _inputValidator;
        private string? _loyaltyNumber;

        public SaleEnterLoyaltyMemberViewModel()
        {
            TotalAmount = 793.76m;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _inputValidator = null;
        }

        public string Title => Resource.Loyalty;
        public string PleaseEnterLoyatlyNumberText => Resource.PleaseEnterLoyaltyNumber;
        public string LoyaltyNumberHint => Resource.LoyaltyNumber;
        public string SkipButtonText => Resource.Skip;
        public string ContinueButtonText => Resource.Continue;
        public string CreateLoyaltyAccountButtonText => Resource.CreateNewLoyaltyAccount;

        public bool ContinueButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public decimal TotalAmount { get; set; }

        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public string? LoyaltyNumber
        {
            get => _loyaltyNumber;
            set => SetProperty(ref _loyaltyNumber, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public void OnSkip()
        {
            NavigationService.Navigate<SaleAddATipViewModel>();
        }

        public void OnContinue()
        {
            NavigationService.Navigate<SaleSelectLoyaltyDiscountViewModel>();
        }

        public void OnCreateLoyaltyAccount()
        {
        }

        public void SetInputValidator(ITextInputValidator loyaltyNumber)
        {
            _inputValidator = loyaltyNumber;

            loyaltyNumber.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.PHONE_NUMBER, Resource.InvalidInputFormat)
            };

            RaisePropertyChanged(() => ContinueButtonEnabled);
        }
    }
}
