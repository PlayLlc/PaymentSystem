using Android.Content.PM;
using Android.OS;
using Android.Text;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using AndroidX.Core.Content.Resources;
using Cards.Pay.Paycardsrecognizer.Sdk;
using Java.Interop;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Utils;
using PayWithPlay.Droid.Utils.Callbacks;
using static Android.Widget.TextView;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar.Dark", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class PaymentManualEntryActivity : BaseActivity<PaymentManualEntryViewModel>
    {
        private ActivityResultLauncher? _scanCardLauncher;

        protected override int LayoutId => Resource.Layout.activity_payment_manual_entry;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetTotalTextViewStyle();
            SetInputValidators();

            _scanCardLauncher = RegisterForActivityResult(new ActivityResultContracts.StartActivityForResult(), new ActivityResultCallback((result) =>
            {
                if (result is ActivityResult activityResult)
                {
                    if (activityResult.ResultCode == (int)Result.Ok)
                    {
                        Card? card;
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
                        {
                            card = activityResult!.Data!.GetParcelableExtra(ScanCardIntent.ResultPaycardsCard, Java.Lang.Class.FromType(typeof(Card)))!.JavaCast<Card>();
                        }
                        else
                        {
                            card = activityResult!.Data!.GetParcelableExtra(ScanCardIntent.ResultPaycardsCard)!.JavaCast<Card>();
                        }

                        if (card != null) 
                        {
                            ViewModel?.OnScanResult(card.CardNumber, card.CardHolderName, card.ExpirationDate);
                        }
                    }
                    else if (activityResult.ResultCode == (int)Result.Canceled)
                    {
                    }
                    else if (activityResult.ResultCode == ScanCardIntent.ResultCodeError)
                    {
                    }
                }
            }));

            ViewModel!.ScanCardAction = () =>
            {
                var builder = new ScanCardIntent.Builder(this);
                _scanCardLauncher.Launch(builder.Build());
            };
        }

        private void SetTotalTextViewStyle()
        {
            var total = FindViewById<TextView>(Resource.Id.totalTv);

            total!.SetText(ViewModel!.TotalDisplayed, BufferType.Spannable);
            var spannable = total.TextFormatted.JavaCast<ISpannable>()!;

            var dollarSignPosition = ViewModel.TotalDisplayed.IndexOf("$");
            spannable!.SetSpan(new CustomTypefaceSpan(ResourcesCompat.GetFont(this, Resource.Font.poppins_semibold)), dollarSignPosition, ViewModel.TotalDisplayed.Length, SpanTypes.ExclusiveExclusive);
        }

        private void SetInputValidators()
        {
            var cardNumberEt = FindViewById<EditTextWithValidation>(Resource.Id.card_number_et);
            var cardHolderNameEt = FindViewById<EditTextWithValidation>(Resource.Id.card_holder_name_et);
            var monthEt = FindViewById<EditTextWithValidation>(Resource.Id.month_et);
            var yearEt = FindViewById<EditTextWithValidation>(Resource.Id.year_et);
            var cvvEt = FindViewById<EditTextWithValidation>(Resource.Id.cvv_et);
            ViewModel!.SetInputValidators(cardNumberEt, cardHolderNameEt, monthEt, yearEt, cvvEt);
        }
    }
}
