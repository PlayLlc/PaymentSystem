using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Return
{
    public class ReturnViewModel : BaseViewModel
    {
        public string Title => Resource.Return;
        public string Subtitle => Resource.ReturnSubtitle;
        public string QRCodeButtonText => Resource.QRCode;
        public string TicketNumberButtonText => Resource.TicketNumber;
        public string LoyaltyNumber => Resource.LoyaltyNumber;

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnQRCode()
        {
            NavigationService.Navigate<ReturnQrCodeViewModel>();
        }

        public void OnTicketNumber()
        {
            NavigationService.Navigate<ReturnTicketNumberViewModel>();
        }

        public void OnLoyaltyNumber()
        {
            NavigationService.Navigate<ReturnLoyaltyNumberViewModel>();
        }
    }
}
