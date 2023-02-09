using Android.Content;
using Android.Content.PM;
using CommunityToolkit.Mvvm.Bindings;
using Microsoft.Extensions.DependencyInjection;
using PayWithPlay.Android.Activities.CreateAccount;
using PayWithPlay.Android.Extensions;
using PayWithPlay.Android.Lifecycle;
using PayWithPlay.Core;
using PayWithPlay.Core.ViewModels.SignIn;

namespace PayWithPlay.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait)]
    public class SignInActivity : BaseActivity, SignInViewModel.INavigationService
    {
        private readonly List<EventToCommandInfo> _eventToCommandInfo = new();
        private readonly List<Binding> _bindings = new();

        private EditText? _emailAddressEt;
        private EditText? _passwordEt;

        private SignInViewModel? _viewModel;

        protected override int LayoutId => Resource.Layout.activity_sign_in;

        public void NavigateToCreateAccount()
        {
            var intent = new Intent(this, typeof(CreateAccountActivity));
            StartActivity(intent);
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = ViewModelProviders.Of(this).Get(ServicesProvider.Current.Provider.GetService<SignInViewModel>);
            _viewModel.NavigationService = this;

            SetBindings();
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

        private void SetBindings()
        {
            var pageTitle = FindViewById<TextView>(Resource.Id.title_textView)!;

            var emailLabel = FindViewById<TextView>(Resource.Id.email_label_tv)!;
            _emailAddressEt = FindViewById<EditText>(Resource.Id.email_et)!;

            var passwordLabel = FindViewById<TextView>(Resource.Id.password_label_tv)!;
            _passwordEt = FindViewById<EditText>(Resource.Id.password_et)!;

            var forgotPasswordButton = FindViewById<Button>(Resource.Id.forgot_password_btn)!;

            var signInButton = FindViewById<Button>(Resource.Id.sign_in_btn)!;
            var noaccountQuestionTextView = FindViewById<TextView>(Resource.Id.no_account_question_tv)!;
            var createAccountButton = FindViewById<Button>(Resource.Id.create_account_btn)!;

            pageTitle.Text = SignInViewModel.Title;
            emailLabel.Text = _emailAddressEt.Hint = SignInViewModel.EmailAdressText;
            passwordLabel.Text = _passwordEt.Hint = SignInViewModel.PasswordText;
            forgotPasswordButton.Text = SignInViewModel.ForgotPasswordText;
            signInButton.Text = SignInViewModel.SignInButtonText;
            noaccountQuestionTextView.Text = SignInViewModel.NoAccountQuestionText;
            createAccountButton.Text = SignInViewModel.CreateAccountButtonText;

            _bindings.Add(this.SetBinding(() => _viewModel!.Email, () => _emailAddressEt.Text, BindingMode.TwoWay));
            _bindings.Add(this.SetBinding(() => _viewModel!.Password, () => _passwordEt.Text, BindingMode.TwoWay));

            _eventToCommandInfo.Add(forgotPasswordButton.SetDetachableCommand(_viewModel!.ForgotPasswordCommand));
            _eventToCommandInfo.Add(signInButton.SetDetachableCommand(_viewModel!.SignInCommand));
            _eventToCommandInfo.Add(createAccountButton.SetDetachableCommand(_viewModel!.CreateAccountCommand));
        }
    }
}