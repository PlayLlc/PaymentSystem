using Android.Views;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(SearchLoyaltyMemberViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_search_member)]
    public class SearchLoyaltyMemberFragment : BaseFragment<SearchLoyaltyMemberViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_search_loyalty_member;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            var phoneNumber = root.FindViewById<EditTextWithValidation>(Resource.Id.phone_numberEt);
            ViewModel.SetInputValidator(phoneNumber);

            return root;
        }
    }
}
