using PayWithPlay.Core.ViewModels.Main.Loyalty;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(LoyaltyDiscountsScanItemViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_loyalty_discount_scan_item)]
    internal class LoyaltyDiscountsScanItemFragment : BaseScanFragment<LoyaltyDiscountsScanItemViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_inventory_scan_item;

        protected override void OnResult(string result)
        {
            ViewModel.OnScanResult(result);
        }
    }
}
