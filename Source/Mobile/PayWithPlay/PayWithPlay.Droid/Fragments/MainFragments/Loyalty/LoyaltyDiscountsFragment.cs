using Android.Views;
using AndroidX.AppCompat.Widget;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.Extensions;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(LoyaltyDiscountsViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_loyalty_discounts)]
    public class LoyaltyDiscountsFragment : BaseFragment<LoyaltyDiscountsViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_loyalty_discounts;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            var topContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.top_container)!;
            topContainer.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());

            return root;
        }
    }
}
