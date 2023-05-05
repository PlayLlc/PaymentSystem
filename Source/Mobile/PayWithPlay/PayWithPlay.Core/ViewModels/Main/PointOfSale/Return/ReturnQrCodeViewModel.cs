using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Return
{
    public class ReturnQrCodeViewModel : BaseViewModel
    {
        public string Title => Resource.Return;

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnScanResult(string result)
        {
            NavigationService.Navigate<ReturnTransactionDetailsViewModel>();
        }
    }
}
