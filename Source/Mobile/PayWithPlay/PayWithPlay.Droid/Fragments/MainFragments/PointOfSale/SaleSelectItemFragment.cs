using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxViewPagerFragmentPresentation(ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(SaleChooseProductsViewModel))]
    public class SaleSelectItemFragment : BaseFragment<SaleSelectItemViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_sale_select_items;
    }
}
