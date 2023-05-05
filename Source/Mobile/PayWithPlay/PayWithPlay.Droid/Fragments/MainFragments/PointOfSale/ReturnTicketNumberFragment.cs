using Android.Views;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Return;
using PayWithPlay.Droid.CustomViews;

namespace PayWithPlay.Droid.Fragments.MainFragments.PointOfSale
{
    [MvxNavFragmentPresentation(ViewModelType = typeof(ReturnTicketNumberViewModel), FragmentMainNavContainerId = Resource.Id.nav_host_container, FragmentNavigationActionId = Resource.Id.action_to_return_ticket_number)]
    public class ReturnTicketNumberFragment : BaseFragment<ReturnTicketNumberViewModel>
    {
        public override int LayoutId => Resource.Layout.fragment_return_ticket_number;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            var phoneNumber = root.FindViewById<EditTextWithValidation>(Resource.Id.ticket_numberEt);
            ViewModel.SetInputValidator(phoneNumber);

            return root;
        }
    }
}
