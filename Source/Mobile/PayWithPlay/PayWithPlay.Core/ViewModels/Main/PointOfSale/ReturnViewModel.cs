using PayWithPlay.Core.Resources;
using PayWithPlay.Droid.Fragments.MainFragments.Inventory;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale
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
