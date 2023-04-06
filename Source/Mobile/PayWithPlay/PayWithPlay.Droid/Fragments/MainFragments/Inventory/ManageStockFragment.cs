using PayWithPlay.Core.ViewModels.Main.Inventory;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(ManageStockViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_manage_stock)]
    public class ManageStockFragment : BaseFragment<ManageStockViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_inventory_manage_stock;
    }
}
