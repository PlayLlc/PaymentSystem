using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;
  
namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxViewPagerFragmentPresentation(ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(SaleChooseProductsViewModel))]
    public class SaleScanItemFragment : BaseScanFragment<SaleScanItemViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_sale_scan_item;

        protected override void OnResult(string result)
        {
            ViewModel.OnScanResult(result);
        }
    }
}
