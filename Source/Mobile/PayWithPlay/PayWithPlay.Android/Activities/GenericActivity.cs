using Android.Content.PM;
using PayWithPlay.Android.Fragments.UserRegistration;
using static PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity.BaseVerifyIdentityViewModel;

namespace PayWithPlay.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class GenericActivity : BaseActivity
    {
        protected override int LayoutId => Resource.Layout.activity_generic;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SupportFragmentManager.BeginTransaction()
                .Add(Resource.Id.fragment_containerView, VerifyIdentityFragment.NewInstance(VerifyIdentity.Email))
                .Commit();
        }
    }
} 