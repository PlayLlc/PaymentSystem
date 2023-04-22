using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Core.ViewModels.Main.PointOfSale;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
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
