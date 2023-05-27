using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxViewPagerFragmentPresentation(ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(SaleChooseProductsViewModel))]
    internal class SaleCustomAmountFragment : BaseFragment<SaleCustomAmountViewModel>
    {
        private EditTextWithValidation? _amount;
        private EditTextWithValidation? _description;

        public override int LayoutId => Resource.Layout.fragment_sale_custom_amount;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            _amount = root.FindViewById<EditTextWithValidation>(Resource.Id.custom_amountEt)!;
            _description = root.FindViewById<EditTextWithValidation>(Resource.Id.descriptionEt)!;
            _amount.SetLimitedDecimalsInputFilter();
            ViewModel.SetInputValidator(_amount);

            return root;
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _amount!.FocusChange += OnPriceTextFocusChange;

            ViewModel.ClearFocusAction = () =>
            {
                _amount.ClearFocus();
                _description!.ClearFocus();
            };
        }

        public override void OnDestroyView()
        {
            _amount!.FocusChange -= OnPriceTextFocusChange;

            base.OnDestroyView();
        }

        private void OnPriceTextFocusChange(object? sender, View.FocusChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.CustomAmount) &&
                decimal.TryParse(ViewModel.CustomAmount, out decimal amountDecimal))
            {
                ViewModel.CustomAmount = $"{amountDecimal:0.00}";
            }
        }
    }
}
