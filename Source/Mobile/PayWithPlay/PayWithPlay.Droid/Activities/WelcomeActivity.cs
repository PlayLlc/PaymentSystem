using Android.Content.PM;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.ViewModels.Welcome;

namespace PayWithPlay.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.App.Starting", MainLauncher = true, ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class WelcomeActivity : BaseActivity<WelcomeViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_welcome;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            AndroidX.Core.SplashScreen.SplashScreen.InstallSplashScreen(this);

            base.OnCreate(savedInstanceState);
        }
    }
}