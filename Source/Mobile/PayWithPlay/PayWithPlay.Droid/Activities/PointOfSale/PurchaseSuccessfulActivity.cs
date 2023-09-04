using Android.Content.PM;
using Google.Android.Material.Button;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar.Dark", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class PurchaseSuccessfulActivity : BaseActivity<PurchaseSuccessfulViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_purchase_successful;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var doneBtn = FindViewById<MaterialButton>(Resource.Id.done_btn)!;
            doneBtn.Alpha = 0f;
            doneBtn.Animate()!.Alpha(1f).SetStartDelay(1500).SetDuration(2000).Start();
        }
    }
}
