using PayWithPlay.Core.ViewModels.Main.Loyalty;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(LoyaltyProgramsViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_loyalty_programs)]
    public class LoyaltyProgramsFragment : BaseFragment<LoyaltyProgramsViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_loyalty_programs;
    }
}
