using PayWithPlay.Core.ViewModels.Main.Inventory;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(SearchInventoryViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_search_inventory)]
    public class SearchInventoryFragment : BaseFragment<SearchInventoryViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_search_inventory;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}