using Android.Content.PM;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar.Dark", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class ChangeCashPaymentActivity : BaseActivity<ChangeCashPaymentViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_change_cash_payment;
    }
}
