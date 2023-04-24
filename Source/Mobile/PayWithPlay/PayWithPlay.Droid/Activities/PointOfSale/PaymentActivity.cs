using Android.Content.PM;
using Android.Text;
using AndroidX.Core.Content.Resources;
using Java.Interop;
using PayWithPlay.Core.ViewModels.Main.PointOfSale;
using PayWithPlay.Droid.Utils;
using static Android.Widget.TextView;
using static System.Net.Mime.MediaTypeNames;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar.Dark", ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class PaymentActivity : BaseActivity<PaymentViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_payment;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTotalTextViewStyle();
        }

        private void SetTotalTextViewStyle()
        {
            var total = FindViewById<TextView>(Resource.Id.totalTv);

            total!.SetText(ViewModel.TotalDisplayed, BufferType.Spannable);
            var spannable = total.TextFormatted.JavaCast<ISpannable>()!;

            var dollarSignPosition = ViewModel.TotalDisplayed.IndexOf("$");
            spannable!.SetSpan(new CustomTypefaceSpan(ResourcesCompat.GetFont(this, Resource.Font.poppins_semibold)), dollarSignPosition, ViewModel.TotalDisplayed.Length, SpanTypes.ExclusiveExclusive);
        }
    }
}
