using PayWithPlay.Core.ViewModels.Main.Loyalty;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(LoyaltyMemberViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_loyalty_member)]
    public class LoyaltyMemberFragment : BaseFragment<LoyaltyMemberViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_loyalty_member;
    }
}
