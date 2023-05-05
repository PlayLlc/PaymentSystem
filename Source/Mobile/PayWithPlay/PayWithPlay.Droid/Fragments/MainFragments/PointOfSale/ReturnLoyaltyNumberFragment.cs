using PayWithPlay.Core.ViewModels.Main.PointOfSale.Return;
using PayWithPlay.Droid.Fragments.MainFragments.Loyalty;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(ReturnLoyaltyNumberViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_return_loyalty_member)]
    public class ReturnLoyaltyNumberFragment : BaseSearchLoyaltyMemberFragment<ReturnLoyaltyNumberViewModel>
    {
    }
}
