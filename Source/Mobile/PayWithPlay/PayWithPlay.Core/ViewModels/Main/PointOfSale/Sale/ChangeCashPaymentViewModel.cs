using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class ChangeCashPaymentViewModel : BaseViewModel<decimal>
    {
        private decimal _receivedAmount;

        public ChangeCashPaymentViewModel()
        {
            TotalAmount = 793.76m;
        }

        public string Title => Resource.Change;

        public decimal TotalAmount { get; set; }

        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public string ChangeAmount => $"${(_receivedAmount - TotalAmount):0.00}";

        public string DoneButtonText => Resource.Done;

        public void OnDone() 
        {
            NavigationService.Navigate<ReceiptOptionsViewModel>();
        }

        public override void Prepare(decimal amount)
        {
            _receivedAmount = amount;
        }
    }
}
