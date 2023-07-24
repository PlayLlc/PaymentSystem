using Android.Views;
using MvvmCross.DroidX.RecyclerView;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(LoyaltyMemberViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_loyalty_member)]
    public class LoyaltyMemberFragment : BaseFragment<LoyaltyMemberViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_loyalty_member;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);
            var purchasesRecyclerView = root.FindViewById<MvxRecyclerView>(Resource.Id.purchases_recyclerView)!;
            purchasesRecyclerView.AddItemDecoration(new RecyclerItemDecoration(2f.ToPx(), Resource.Color.separator_color));

            return root;
        }
    }
}
