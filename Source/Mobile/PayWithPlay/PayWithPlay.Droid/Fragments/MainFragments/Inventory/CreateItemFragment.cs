using Android.Text;
using Android.Views;
using AndroidX.Camera.Core;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils.InputFilters;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(CreateItemViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_create_item)]
    public class CreateItemFragment : BaseFragment<CreateItemViewModel>
    {
        private EditTextWithValidation? _priceText;

        public override int LayoutId => Resource.Layout.fragment_create_inventory_item;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _priceText = view.FindViewById<EditTextWithValidation>(Resource.Id.priceEt)!;
            _priceText.SetLimitedDecimalsInputFilter();

            return view;
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _priceText!.FocusChange += OnPriceTextFocusChange;
        }

        public override void OnDestroyView()
        {
            _priceText!.FocusChange -= OnPriceTextFocusChange;

            base.OnDestroyView();
        }

        private void OnPriceTextFocusChange(object? sender, View.FocusChangeEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.Item.Price) &&
                decimal.TryParse(ViewModel.Item.Price, out decimal priceInDecimal))
            {
                ViewModel.Item.Price = $"{priceInDecimal:0.00}";
            }
        }
    }
}
