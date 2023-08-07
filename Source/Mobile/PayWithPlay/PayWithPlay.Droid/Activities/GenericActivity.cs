using Android.Content.PM;
using AndroidX.AppCompat.Widget;
using PayWithPlay.Core.ViewModels;

namespace PayWithPlay.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class GenericActivity : BaseActivity<GenericViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_generic;

        public void SetTopImage(int resId)
        {
            var topImage = FindViewById<AppCompatImageView>(Resource.Id.top_image)!;
            topImage.SetImageResource(resId);
        }
    }
}