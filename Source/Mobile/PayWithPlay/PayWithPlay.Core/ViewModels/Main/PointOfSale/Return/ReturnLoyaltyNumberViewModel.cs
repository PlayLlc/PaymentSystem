using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.Main.Loyalty;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Return
{
    public class ReturnLoyaltyNumberViewModel : SearchLoyaltyMemberViewModel
    {
        public override string Title => Resource.Return;
        public override string InputText => Resource.LoyaltyNumber;
        public override string NextButtonText => Resource.Continue;

        public override void OnNext()
        {
            NavigationService.Navigate<ReturnTransactionDetailsViewModel>();
        }
    }
}
