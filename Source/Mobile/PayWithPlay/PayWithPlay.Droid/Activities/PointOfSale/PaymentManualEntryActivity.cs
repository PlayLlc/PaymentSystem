using Android.Content.PM;
using Android.Text;
using AndroidX.Core.Content.Resources;
using Java.Interop;
using PayWithPlay.Core.ViewModels.Main.PointOfSale;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Utils;
using static Android.Widget.TextView;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar.Dark", ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class PaymentManualEntryActivity : BaseActivity<PaymentManualEntryViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_payment_manual_entry;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTotalTextViewStyle();
            SetInputValidators();
        }

        private void SetTotalTextViewStyle()
        {
            var total = FindViewById<TextView>(Resource.Id.totalTv);

            total!.SetText(ViewModel.TotalDisplayed, BufferType.Spannable);
            var spannable = total.TextFormatted.JavaCast<ISpannable>()!;

            var dollarSignPosition = ViewModel.TotalDisplayed.IndexOf("$");
            spannable!.SetSpan(new CustomTypefaceSpan(ResourcesCompat.GetFont(this, Resource.Font.poppins_semibold)), dollarSignPosition, ViewModel.TotalDisplayed.Length, SpanTypes.ExclusiveExclusive);
        }

        private void SetInputValidators()
        {
            var cardNumberEt = FindViewById<EditTextWithValidation>(Resource.Id.card_number_et);
            var monthEt = FindViewById<EditTextWithValidation>(Resource.Id.month_et);
            var yearEt = FindViewById<EditTextWithValidation>(Resource.Id.year_et);
            var cvvEt = FindViewById<EditTextWithValidation>(Resource.Id.cvv_et);
            ViewModel.SetInputValidators(cardNumberEt, monthEt, yearEt, cvvEt);
        }
    }
}
