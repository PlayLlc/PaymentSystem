using Android.Views;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(LoyaltyRewardsViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_loyalty_rewards)]
    public class LoyaltyRewardsFragment : BaseFragment<LoyaltyRewardsViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_loyalty_rewards;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            SetValidators(root);

            return root;
        }

        private void SetValidators(View root)
        {
            var pointsPerDollar = root.FindViewById<EditTextWithValidation>(Resource.Id.points_per_dollarEt);
            var pointsRequired = root.FindViewById<EditTextWithValidation>(Resource.Id.points_requiredEt);
            var reward = root.FindViewById<EditTextWithValidation>(Resource.Id.rewardEt);

            ViewModel.SetInputValidators(pointsPerDollar, pointsRequired, reward);
        }
    }
}
