using Android.Content.PM;
using Android.Views;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class SaleAddATipActivity : BaseActivity<SaleAddATipViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_sale_add_a_tip;
    }
}
