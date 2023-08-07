﻿using Android.Content.PM;
using Android.Text;
using AndroidX.Core.Content.Resources;
using Java.Interop;
using PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale;
using PayWithPlay.Droid.CustomViews;
using PayWithPlay.Droid.Utils;
using static Android.Widget.TextView;

namespace PayWithPlay.Droid.Activities.PointOfSale
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar.Dark", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CashPaymentActivity : BaseActivity<CashPaymentViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_cash_payment;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetTotalTextViewStyle();
            SetInputValidators();
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
            var amountReceivedEt = FindViewById<EditTextWithValidation>(Resource.Id.amount_received_et);
            ViewModel!.SetInputValidator(amountReceivedEt);
        }
    }
}