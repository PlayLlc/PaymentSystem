using MvvmCross.ViewModels;
using PayWithPlay.Core.Constants;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class PurchaseSuccessfulViewModel : BaseViewModel
    {
        public PurchaseSuccessfulViewModel()
        {
            TotalAmount = 793.76m;
        }

        public decimal TotalAmount { get; set; }
        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public string Title => Resource.PurchaseSuccessful;

        public string DoneButtonText => Resource.Done;

        public void OnDone() 
        {
            NavigationService.Navigate<MainViewModel>(new MvxBundle(new Dictionary<string, string> { { AppConstants.ClearBackStack, "" } }));
        }
    }
}
