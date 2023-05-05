using Android.Content.PM;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class SaleSelectLoyaltyDiscountActivity : BaseActivity<SaleSelectLoyaltyDiscountViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_sale_select_loyalty_discount;
    }
}
