using PayWithPlay.Core.ViewModels.Main.PointOfSale.Return;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(ReturnViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_point_of_sale_return)]
    public class ReturnFragment : BaseFragment<ReturnViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_return;
    }
}
