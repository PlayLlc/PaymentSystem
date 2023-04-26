using Android.Content.PM;
using PayWithPlay.Core.ViewModels.Main.PointOfSale;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class SaleAddATipActivity : BaseActivity<SaleAddATipViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_sale_add_a_tip;
    }
}
