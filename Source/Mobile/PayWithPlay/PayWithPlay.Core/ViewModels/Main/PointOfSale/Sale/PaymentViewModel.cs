using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class PaymentViewModel : BaseViewModel
    {
        public PaymentViewModel()
        {
            TotalAmount = 793.76m;
        }

        public string Title => Resource.PaymentTitle;
        public string Subtitle => Resource.PaymentSubtitle;
        public string ManualEntryButtonText => Resource.ManualEntry;

        public decimal TotalAmount { get; set; }
        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public void OnManualEntry()
        {
            NavigationService.Navigate<PaymentManualEntryViewModel>();
        }
    }
}
