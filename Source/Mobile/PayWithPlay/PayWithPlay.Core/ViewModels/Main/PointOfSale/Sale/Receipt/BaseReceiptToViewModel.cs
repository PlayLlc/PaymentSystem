using PayWithPlay.Core.Interfaces;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt
{
    public abstract class BaseReceiptToViewModel : BaseViewModel
    {
        protected ITextInputValidator? _inputValidator;
        private string? _input;

        public BaseReceiptToViewModel()
        {
            TotalAmount = 793.76m;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            ClearValidator();

            base.ViewDestroy(viewFinishing);
        }

        public abstract string Title { get; }
        public abstract string InputHint { get; }

        public decimal TotalAmount { get; set; }
        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";
        public string SafeMessage => Resource.InformationSafeMessage;
        public string SendButtonText => Resource.Send;

        public bool SendButtonEnabled => ValidationHelper.IsInputValid(_inputValidator, false);

        public string? Input
        {
            get => _input;
            set => SetProperty(ref _input, value, () => RaisePropertyChanged(()=> SendButtonEnabled));
        }

        public abstract void SetInputValidator(ITextInputValidator inputValidator);

        public void ClearValidator()
        {
            _inputValidator = null;
        }

        public void OnSend() 
        {
            NavigationService.Navigate<PurchaseSuccessfulViewModel>();
        }
    }
}
