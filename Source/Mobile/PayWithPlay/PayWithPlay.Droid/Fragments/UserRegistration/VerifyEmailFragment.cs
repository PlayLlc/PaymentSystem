using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using PayWithPlay.Core.ViewModels;

namespace PayWithPlay.Droid.Fragments.UserRegistration
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(GenericViewModel), FragmentContentId = Resource.Id.fragment_containerView, ViewModelType = typeof(VerifyEmailViewModel))]
    public class VerifyEmailFragment : BaseFragment<VerifyEmailViewModel>
    {
        public VerifyEmailFragment()
        {
        }

        public override int LayoutId => Resource.Layout.fragment_verify_identity;
    }
}