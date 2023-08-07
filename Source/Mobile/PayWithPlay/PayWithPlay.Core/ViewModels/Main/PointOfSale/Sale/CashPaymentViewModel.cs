using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class CashPaymentViewModel : BaseViewModel
    {
        private string? _amountReceived;

        private ITextInputValidator? _inputValidator;

        public CashPaymentViewModel()
        {
            TotalAmount = 793.76m;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _inputValidator = null;
        }

        public string Title => Resource.CashPayment;

        public decimal TotalAmount { get; set; }

        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public string PleaseEnterAmountReceivedText => Resource.PleaseEnterAmountReceived;
        public string AmountReceivedHint => Resource.AmountReceived;

        public string SubmitButtonText => Resource.Submit;

        public bool SubmitButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public string? AmountReceived
        {
            get => _amountReceived;
            set => SetProperty(ref _amountReceived, value, () => RaisePropertyChanged(() => SubmitButtonEnabled));
        }

        public void SetInputValidator(ITextInputValidator inputValidator) 
        {
            _inputValidator = inputValidator;
            _inputValidator.Validations = new List<IValidation>
            {
                new NonEmptyValidation(),
                new CustomValidation((value) => 
                {
                    if(decimal.TryParse(value, out decimal decimalValue))
                    {
                        if(decimalValue < TotalAmount)
                        {
                            return ValidationResultFactory.Error("The received amount is less than total amount.");
                        }

                        return ValidationResultFactory.Success;
                    }

                    return ValidationResultFactory.Error("Invalid input");
                })
            };

            RaisePropertyChanged(() => SubmitButtonEnabled);
        }

        public void OnSubmit() 
        {
            NavigationService.Navigate<ChangeCashPaymentViewModel, decimal>(decimal.Parse(AmountReceived));
        }
    }
}
