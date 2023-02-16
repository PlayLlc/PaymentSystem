using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using PayWithPlay.Core.ViewModels;

namespace PayWithPlay.Android.Fragments.UserRegistration
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(GenericViewModel), FragmentContentId = Resource.Id.fragment_containerView, ViewModelType = typeof(VerifyEmailViewModel))]
    public class VerifyEmailFragment : VerifyIdentityFragment<VerifyEmailViewModel>
    {
        public VerifyEmailFragment()
        {
        }
    }
}