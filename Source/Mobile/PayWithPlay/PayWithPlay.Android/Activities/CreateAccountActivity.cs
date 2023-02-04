using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using AndroidX.Core.Content;
using AndroidX.Core.Content.Resources;
using CommunityToolkit.Mvvm.Bindings;
using Google.Android.Material.CheckBox;
using Microsoft.Extensions.DependencyInjection;
using PayWithPlay.Android.Extensions;
using PayWithPlay.Android.Lifecycle;
using PayWithPlay.Android.Utils;
using PayWithPlay.Core;
using PayWithPlay.Core.ViewModels.CreateAccount;
using static Android.Widget.TextView;

namespace PayWithPlay.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar")]
    public class CreateAccountActivity : BaseActivity, CreateAccountViewModel.INavigationService 
    {
        private readonly List<EventToCommandInfo> _eventToCommandInfo = new();
        private readonly List<Binding> _bindings = new();

        private EditText? _emailOrPhoneNumberEt;
        private EditText? _passwordEt;
        private MaterialCheckBox? _termsPolicyCb;

        private CreateAccountViewModel? _viewModel;

        protected override int LayoutId => Resource.Layout.activity_create_account;

        public void NavigateToSignIn()
        {
            var intent = new Intent(this, typeof(SignInActivity));
            StartActivity(intent);
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = ViewModelProviders.Of(this).Get(ServicesProvider.Current.Provider.GetService<CreateAccountViewModel>);
            _viewModel.NavigationService = this;

            InitViews();
            SetBindings();
            SetTermsAndPivacyCheckBox();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_viewModel != null)
            {
                _viewModel.NavigationService = null;
            }

            _bindings.DetachAll();
            _eventToCommandInfo.DetachAll();
        }

        private void InitViews()
        {
            _emailOrPhoneNumberEt = FindViewById<EditText>(Resource.Id.email_or_phoneNumber_et)!;
            _passwordEt = FindViewById<EditText>(Resource.Id.password_et)!;
            _termsPolicyCb = FindViewById<MaterialCheckBox>(Resource.Id.terms_and_policy_cb)!;
        }

        private void SetBindings()
        {
            var pageTitle = FindViewById<TextView>(Resource.Id.title_textView)!;
            var emailOrPhoneNumberLabel = FindViewById<TextView>(Resource.Id.email_or_phoneNumber_label_tv)!;
            var passwordLabel = FindViewById<TextView>(Resource.Id.password_label_tv)!;

            var createAccountButton = FindViewById<Button>(Resource.Id.create_account_btn)!;
            var haveAccountQuestionTextView = FindViewById<TextView>(Resource.Id.have_account_question_tv)!;
            var signInButton = FindViewById<Button>(Resource.Id.sign_in_btn)!;

            pageTitle.Text = CreateAccountViewModel.Title;
            emailOrPhoneNumberLabel.Text = _emailOrPhoneNumberEt!.Hint = CreateAccountViewModel.EmailOrPhoneNumberText;
            passwordLabel.Text = _passwordEt!.Hint = CreateAccountViewModel.PasswordText;
            signInButton.Text = CreateAccountViewModel.SignInButtonText;
            haveAccountQuestionTextView.Text = CreateAccountViewModel.HaveAccountQuestionText;
            createAccountButton.Text = CreateAccountViewModel.CreateAccountButtonText;

            _bindings.Add(this.SetBinding(() => _viewModel!.EmailOrPhoneNumber, () => _emailOrPhoneNumberEt.Text, BindingMode.TwoWay));
            _bindings.Add(this.SetBinding(() => _viewModel!.Password, () => _passwordEt.Text, BindingMode.TwoWay));
            _bindings.Add(this.SetBinding(() => _viewModel!.TermsPolicyAccepted, () => _termsPolicyCb!.Checked, BindingMode.TwoWay));

            _eventToCommandInfo.Add(createAccountButton.SetDetachableCommand(_viewModel!.CreateAccountCommand));
            _eventToCommandInfo.Add(signInButton.SetDetachableCommand(_viewModel!.SignInCommand));
        }

        private void SetTermsAndPivacyCheckBox()
        {
            _termsPolicyCb!.SetText(CreateAccountViewModel.TermsAndPolicyFullText, BufferType.Spannable);
            var spannable = _termsPolicyCb.TextFormatted as ISpannable;

            var semiBoldTypefaceFont = ResourcesCompat.GetFont(this, Resource.Font.poppins_semibold);

            var termsAndServiceIndex = CreateAccountViewModel.TermsAndPolicyFullText.IndexOf(CreateAccountViewModel.TermOfServiceText, StringComparison.OrdinalIgnoreCase);
            spannable!.SetSpan(new CustomTypefaceSpan(semiBoldTypefaceFont), termsAndServiceIndex, termsAndServiceIndex + CreateAccountViewModel.TermOfServiceText.Length, SpanTypes.ExclusiveExclusive);
            spannable!.SetSpan(new CustomClickableSpan((v) =>
            {
                v.CancelPendingInputEvents();
                _viewModel!.TermsOfServiceCommand.Execute(null);
            }), termsAndServiceIndex, termsAndServiceIndex + CreateAccountViewModel.TermOfServiceText.Length, SpanTypes.ExclusiveExclusive);
            spannable!.SetSpan(new ForegroundColorSpan(new Color(ContextCompat.GetColor(this, Resource.Color.primary_text_color))), termsAndServiceIndex, termsAndServiceIndex + CreateAccountViewModel.TermOfServiceText.Length, SpanTypes.ExclusiveExclusive);

            var privacyPolicyIndex = CreateAccountViewModel.TermsAndPolicyFullText.IndexOf(CreateAccountViewModel.PrivacyPolicyText, StringComparison.OrdinalIgnoreCase);
            spannable!.SetSpan(new CustomTypefaceSpan(semiBoldTypefaceFont), privacyPolicyIndex, privacyPolicyIndex + CreateAccountViewModel.PrivacyPolicyText.Length, SpanTypes.ExclusiveExclusive);
            spannable!.SetSpan(new CustomClickableSpan((v) =>
            {
                v.CancelPendingInputEvents();
                _viewModel!.PrivacyPolicyCommand.Execute(null);
            }), privacyPolicyIndex, privacyPolicyIndex + CreateAccountViewModel.PrivacyPolicyText.Length, SpanTypes.ExclusiveExclusive);
            spannable!.SetSpan(new ForegroundColorSpan(new Color(ContextCompat.GetColor(this, Resource.Color.primary_text_color))), privacyPolicyIndex, privacyPolicyIndex + CreateAccountViewModel.PrivacyPolicyText.Length, SpanTypes.ExclusiveExclusive);

            _termsPolicyCb.SetText(spannable, BufferType.Spannable);
            _termsPolicyCb.MovementMethod = LinkMovementMethod.Instance;
        }
    }
}