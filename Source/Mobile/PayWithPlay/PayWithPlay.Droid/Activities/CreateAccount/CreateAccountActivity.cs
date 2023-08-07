using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using AndroidX.Core.Content;
using AndroidX.Core.Content.Resources;
using Google.Android.Material.CheckBox;
using PayWithPlay.Droid.Utils;
using PayWithPlay.Core.ViewModels.CreateAccount;
using static Android.Widget.TextView;
using PayWithPlay.Droid.CustomViews;
using Android.Content.PM;

namespace PayWithPlay.Droid.Activities.CreateAccount
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CreateAccountActivity : BaseActivity<CreateAccountViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_create_account;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetTermsAndPivacyCheckBox();

            var emailEt = FindViewById<EditTextWithValidation>(Resource.Id.email_et);
            var passwordEt = FindViewById<EditTextWithValidation>(Resource.Id.password_et);
            ViewModel.SetInputValidations(emailEt, passwordEt);
        }

        private void SetTermsAndPivacyCheckBox()
        {
            var termsPolicyCb = FindViewById<MaterialCheckBox>(Resource.Id.terms_and_policy_cb)!;
            termsPolicyCb!.SetText(ViewModel.TermsAndPolicyFullText, BufferType.Spannable);
            var spannable = termsPolicyCb.TextFormatted.JavaCast<ISpannable>();

            var semiBoldTypefaceFont = ResourcesCompat.GetFont(this, Resource.Font.poppins_semibold);

            var termsAndServiceIndex = ViewModel.TermsAndPolicyFullText.IndexOf(ViewModel.TermOfServiceText, StringComparison.OrdinalIgnoreCase);
            spannable!.SetSpan(new CustomTypefaceSpan(semiBoldTypefaceFont), termsAndServiceIndex, termsAndServiceIndex + ViewModel.TermOfServiceText.Length, SpanTypes.ExclusiveExclusive);
            spannable!.SetSpan(new CustomClickableSpan((v) =>
            {
                v.CancelPendingInputEvents();
                ViewModel!.OnTermsOfService();
            }), termsAndServiceIndex, termsAndServiceIndex + ViewModel.TermOfServiceText.Length, SpanTypes.ExclusiveExclusive);
            spannable!.SetSpan(new ForegroundColorSpan(new Color(ContextCompat.GetColor(this, Resource.Color.primary_text_color))), termsAndServiceIndex, termsAndServiceIndex + ViewModel.TermOfServiceText.Length, SpanTypes.ExclusiveExclusive);

            var privacyPolicyIndex = ViewModel.TermsAndPolicyFullText.IndexOf(ViewModel.PrivacyPolicyText, StringComparison.OrdinalIgnoreCase);
            spannable!.SetSpan(new CustomTypefaceSpan(semiBoldTypefaceFont), privacyPolicyIndex, privacyPolicyIndex + ViewModel.PrivacyPolicyText.Length, SpanTypes.ExclusiveExclusive);
            spannable!.SetSpan(new CustomClickableSpan((v) =>
            {
                v.CancelPendingInputEvents();
                ViewModel!.OnPrivacyPolicy();
            }), privacyPolicyIndex, privacyPolicyIndex + ViewModel.PrivacyPolicyText.Length, SpanTypes.ExclusiveExclusive);
            spannable!.SetSpan(new ForegroundColorSpan(new Color(ContextCompat.GetColor(this, Resource.Color.primary_text_color))), privacyPolicyIndex, privacyPolicyIndex + ViewModel.PrivacyPolicyText.Length, SpanTypes.ExclusiveExclusive);

            termsPolicyCb.SetText(spannable, BufferType.Spannable);
            termsPolicyCb.MovementMethod = LinkMovementMethod.Instance;
        }
    }
}