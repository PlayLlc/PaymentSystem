using Android.Views;
using AndroidX.AppCompat.Widget;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(ScanItemViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_scan_item)]
    public class ScanItemFragment : BaseScanFragment<ScanItemViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_inventory_scan_item;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnNewScanAction = OnNewScan;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }

        public override void OnStop()
        {
            base.OnStop();

            ViewModel.OnNewScanAction = null;
        }

        protected override void OnResult(string result)
        {
            ViewModel.OnScanResult(result);
        }

        private void OnNewScan()
        {
            BindPreview(_cameraProvider);
        }

        protected override void InitViews(View root)
        {
            base.InitViews(root);

            var resultsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.result_container)!;
            resultsContainer.SetBackground(Resource.Color.white, topLeft: 5f.ToPx(), topRight: 5f.ToPx());
        }
    }
}
