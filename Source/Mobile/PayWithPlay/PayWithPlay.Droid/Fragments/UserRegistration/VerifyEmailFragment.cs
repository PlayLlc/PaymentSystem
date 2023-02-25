using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using PayWithPlay.Core.ViewModels;
using Android.Content;
using PayWithPlay.Droid.Activities;

namespace PayWithPlay.Droid.Fragments.UserRegistration
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(GenericViewModel), FragmentContentId = Resource.Id.fragment_containerView, ViewModelType = typeof(VerifyEmailViewModel))]
    public class VerifyEmailFragment : BaseFragment<VerifyEmailViewModel>
    {
        public VerifyEmailFragment()
        {
        }

        public override int LayoutId => Resource.Layout.fragment_verify_identity;

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);

            if (Activity is GenericActivity genericActivity)
            {
                genericActivity.SetTopImage(Resource.Drawable.signin_page_logo);
            }
        }
    }
}