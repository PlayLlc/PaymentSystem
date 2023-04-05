using Android.Views;
using PayWithPlay.Core.ViewModels.Main.Inventory;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(CreateItemViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_create_item)]
    public class CreateItemFragment : BaseFragment<CreateItemViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_create_inventory_item;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }
    }
}
