using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt
{
    public class ReceiptToEmailViewModel : BaseReceiptToViewModel
    {
        public override string Title => Resource.PleaseEnterCustomerEmail;

        public override string InputHint => Resource.Email;


        public override void SetInputValidator(ITextInputValidator inputValidator)
        {
            ClearValidator();

            _inputValidator = inputValidator;
            _inputValidator.Validations = new List<IValidation>
            {
                new RegexValidation(RegexenConstants.EMAIL, Resource.InvalidInputFormat),
            };

            RaisePropertyChanged(() => SendButtonEnabled);
        }
    }
}
