using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using PayWithPlay.Droid.Adapters;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root =  base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = root.FindViewById<MvxRecyclerView>(Resource.Id.rv_items)!;
            recyclerView.Adapter = new InventoryItemsAdapter((IMvxAndroidBindingContext)BindingContext);

            return root;
        }
    }
}