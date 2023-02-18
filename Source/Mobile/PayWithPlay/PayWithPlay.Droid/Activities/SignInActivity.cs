using Android.Content.PM;
using PayWithPlay.Core.ViewModels.SignIn;

namespace PayWithPlay.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class SignInActivity : BaseActivity<SignInViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_sign_in;
    }
}