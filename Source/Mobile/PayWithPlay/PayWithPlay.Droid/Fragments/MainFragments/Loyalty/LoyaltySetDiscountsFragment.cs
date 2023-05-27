using Android.Text;
using Android.Views;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Utils.InputFilters;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(LoyaltySetDiscountViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_loyalty_set_discounts)]
    public class LoyaltySetDiscountsFragment : BaseFragment<LoyaltySetDiscountViewModel>
    {
        private EditTextWithValidation? _discountValueInput;

        public override int LayoutId => Resource.Layout.fragment_loyalty_set_discounts;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.DiscountTypeChangedAction = OnDiscountTypeChanged;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            _discountValueInput = root.FindViewById<EditTextWithValidation>(Resource.Id.discountValueEt);

            SetDiscountValueInputType();

            return root;
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _discountValueInput!.FocusChange += OnPriceTextFocusChange;
        }

        public override void OnDestroyView()
        {
            _discountValueInput!.FocusChange -= OnPriceTextFocusChange;

            base.OnDestroyView();
        }

        private void OnPriceTextFocusChange(object? sender, View.FocusChangeEventArgs e)
        {
            if (ViewModel.SelectedDiscountType == (int)DiscountType.Amount) 
            {
                if (!string.IsNullOrWhiteSpace(ViewModel.DiscountValue) &&
                    decimal.TryParse(ViewModel.DiscountValue, out decimal discountDecimal))
                {
                    ViewModel.DiscountValue = $"{discountDecimal:0.00}";
                }
            }
        }

        private void OnDiscountTypeChanged()
        {
            _discountValueInput!.ClearFocus();
            SetDiscountValueInputType();
        }

        private void SetDiscountValueInputType()
        {
            var currentFilters = _discountValueInput!.GetFilters();
            currentFilters ??= Array.Empty<IInputFilter>();

            if (ViewModel.SelectedDiscountType == (int)DiscountType.Amount)
            {
                _discountValueInput.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

                var decimalLimtedInputFilter = new InputFilterWithLimitedDecimals(2);
                currentFilters = currentFilters.Where(t => t is not InputFilterMaxNumber).Append(decimalLimtedInputFilter).ToArray();
            }
            else
            {
                _discountValueInput.InputType = InputTypes.ClassNumber | InputTypes.NumberVariationNormal;

                var limitFilter = new InputFilterMaxNumber(100);
                currentFilters = currentFilters.Where(t => t is not InputFilterWithLimitedDecimals).Append(limitFilter).ToArray();
            }

            _discountValueInput.SetFilters(currentFilters);
        }
    }
}
