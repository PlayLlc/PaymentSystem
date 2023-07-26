using Android.Views;
using AndroidX.AppCompat.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using PayWithPlay.Droid.Adapters;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(InventoryScanItemViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_scan_item)]
    public class InventoryScanItemFragment : BaseScanFragment<InventoryScanItemViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_inventory_scan_item;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnNewScanAction = StartScanning;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }

        protected override void OnResult(string result)
        {
            StopScanning();

            ViewModel.OnScanResult(result);
        }

        protected override void InitViews(View root)
        {
            base.InitViews(root);

            var resultsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.result_container)!;
            resultsContainer.SetBackground(Resource.Color.white, topLeft: 5f.ToPx(), topRight: 5f.ToPx());

            var recyclerView = root.FindViewById<MvxRecyclerView>(Resource.Id.rv_items)!;
            recyclerView.Adapter = new InventoryItemsAdapter((IMvxAndroidBindingContext)BindingContext);
        }
    }
}
