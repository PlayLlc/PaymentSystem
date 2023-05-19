using Android.Text;
using Android.Views;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(LoyaltySetDiscountViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_loyalty_set_discounts)]
    public class LoyaltySetDiscountsFragment : BaseFragment<LoyaltySetDiscountViewModel>
    {
        private EditTextWithValidation? discountValueInput;

        public override int LayoutId => Resource.Layout.fragment_loyalty_set_discounts;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.DiscountTypeChangedAction = OnDiscountTypeChanged;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            discountValueInput = root.FindViewById<EditTextWithValidation>(Resource.Id.discountValueEt);

            SetDiscountValueInputType();

            return root;
        }

        private void OnDiscountTypeChanged()
        {
            discountValueInput!.ClearFocus();
            SetDiscountValueInputType();
        }

        private void SetDiscountValueInputType()
        {
            if (ViewModel.SelectedDiscountType == (int)DiscountType.Amount)
            {
                discountValueInput!.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

                var currentFilters = discountValueInput.GetFilters();
                if (currentFilters != null)
                {
                    discountValueInput.SetFilters(currentFilters.Where(t => t is not InputFilterMaxNumber).ToArray());
                }
            }
            else
            {
                discountValueInput!.InputType = InputTypes.ClassNumber | InputTypes.NumberVariationNormal;
                var currentFilters = discountValueInput.GetFilters();

                var limitFilter = new InputFilterMaxNumber(100);
                if (currentFilters == null)
                {
                    currentFilters = new IInputFilter[] { limitFilter };
                }
                else
                {
                    currentFilters = currentFilters.Append(limitFilter).ToArray();
                }

                discountValueInput.SetFilters(currentFilters);
            }
        }
    }
}
