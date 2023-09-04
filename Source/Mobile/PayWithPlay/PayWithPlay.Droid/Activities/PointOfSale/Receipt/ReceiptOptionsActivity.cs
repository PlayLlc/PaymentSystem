using Android.Content.PM;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt;

namespace PayWithPlay.Droid.Activities.PointOfSale.Receipt
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class ReceiptOptionsActivity : BaseActivity<ReceiptOptionsViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_receipt_options;

        protected override bool OnBackPressed()
        {
            return false;
        }
    }
}