using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Return
{
    public class ReturnTicketNumberViewModel : BaseViewModel
    {
        private ITextInputValidator? _inputValidator;

        public string? _ticketNumber;

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _inputValidator = null;
        }

        public string Title => Resource.Return;
        public string TicketNumberText => Resource.TicketNumber;
        public string ContinueButtonText => Resource.Continue;

        public bool ContinueButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public string? TicketNumber
        {
            get => _ticketNumber;
            set => SetProperty(ref _ticketNumber, value, () => RaisePropertyChanged(() => ContinueButtonEnabled));
        }

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnContinue()
        {
            NavigationService.Navigate<ReturnTransactionDetailsViewModel>();
        }

        public void SetInputValidator(ITextInputValidator phoneNumber)
        {
            _inputValidator = phoneNumber;
            _inputValidator.Validations = new List<IValidation>
            {
                new MinLengthValidation(9, Resource.InvalidInputFormat)
            };

            RaisePropertyChanged(() => ContinueButtonEnabled);
        }
    }
}
