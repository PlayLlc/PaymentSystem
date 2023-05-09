using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxViewPagerFragmentPresentation(ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(SaleChooseProductsViewModel))]
    internal class SaleCustomAmountFragment : BaseFragment<SaleCustomAmountViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_sale_custom_amount;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            var amount = root.FindViewById<EditTextWithValidation>(Resource.Id.custom_amountEt);
            ViewModel.SetInputValidator(amount);

            return root;
        }
    }
}
  