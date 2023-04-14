using Android.Views;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(CreateLoyaltyMemberViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_create_member)]
    public class CreateLoyaltyMemberFragment : BaseFragment<CreateLoyaltyMemberViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_create_loyalty_member;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root =  base.OnCreateView(inflater, container, savedInstanceState);

            SetValidators(root);

            return root;
        }

        private void SetValidators(View root)
        {
            var phoneNumber = root.FindViewById<EditTextWithValidation>(Resource.Id.phone_numberEt);
            var name = root.FindViewById<EditTextWithValidation>(Resource.Id.nameEt);
            var email = root.FindViewById<EditTextWithValidation>(Resource.Id.emailEt);

            ViewModel.SetInputValidators(phoneNumber, name, email);
        }
    }
}
