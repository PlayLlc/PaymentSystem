using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.PointOfSale;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    //    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(PointOfSaleViewModel))]
    public class PointOfSaleFragment : BaseFragment<PointOfSaleViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_point_of_sale;
    }
}