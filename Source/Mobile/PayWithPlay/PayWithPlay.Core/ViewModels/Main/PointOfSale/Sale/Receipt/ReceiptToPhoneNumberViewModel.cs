using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt
{
    public class ReceiptToPhoneNumberViewModel : BaseReceiptToViewModel
    {
        public override string Title => Resource.PleaseEnterCustomerPhoneNumber;

        public override string InputHint => Resource.PhoneNumberText;


        public override void SetInputValidator(ITextInputValidator inputValidator)
        {
            ClearValidator();

            _inputValidator = inputValidator;
            _inputValidator.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.PHONE_NUMBER, Resource.InvalidInputFormat),
            };

            RaisePropertyChanged(() => SendButtonEnabled);
        }
    }
}
