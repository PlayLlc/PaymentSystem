using PayWithPlay.Core.ViewModels.Main.Inventory;
using PayWithPlay.Core.ViewModels.Main.PointOfSale;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(ReturnQrCodeViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_return_qr_code)]
    public class ReturnQrCodeFragment : BaseScanFragment<ReturnQrCodeViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_return_qr_code;

        protected override void OnResult(string result)
        {
            ViewModel.OnScanResult(result);
        }
    }
}
