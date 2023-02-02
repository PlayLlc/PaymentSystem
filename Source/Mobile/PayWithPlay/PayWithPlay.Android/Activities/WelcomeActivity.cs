using Android.Content;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.Lifecycle;
using CommunityToolkit.Mvvm.Bindings;
using Microsoft.Extensions.DependencyInjection;
using PayWithPlay.Android.Extensions;
using PayWithPlay.Android.Lifecycle;
using PayWithPlay.Core;
using PayWithPlay.Core.ViewModels.Welcome;

namespace PayWithPlay.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.App.Starting", MainLauncher = true)]
    public class WelcomeActivity : AppCompatActivity, WelcomeViewModel.INavigationService
    {
        private readonly List<EventToCommandInfo> _eventToCommandInfo = new List<EventToCommandInfo>();

        private WelcomeViewModel? _viewModel;

        public void NavigateToCreateAccount()
        {
        }

        public void NavigateToSignIn()
        {
            var intent = new Intent(this, typeof(SignInActivity));
            StartActivity(intent);
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            AndroidX.Core.SplashScreen.SplashScreen.InstallSplashScreen(this);

            base.OnCreate(savedInstanceState);
            WindowCompat.SetDecorFitsSystemWindows(Window!, false);

            SetContentView(Resource.Layout.activity_welcome);

            _viewModel = ViewModelProviders.Of(this).Get(ServicesProvider.Current.Provider.GetService<WelcomeViewModel>);
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

            _eventToCommandInfo.DetachAll();
        }

        private void SetBindings()
        {
            var pageTitle = FindViewById<TextView>(Resource.Id.title_textView)!;
            var signInButton = FindViewById<Button>(Resource.Id.sign_in_btn)!;
            var createAccountButton = FindViewById<Button>(Resource.Id.create_account_btn)!;
            var tAndCButton = FindViewById<Button>(Resource.Id.t_and_c_btn)!;
            var privayPolicyButton = FindViewById<Button>(Resource.Id.privacy_btn)!;

            pageTitle.Text = WelcomeViewModel.Title;
            signInButton.Text = WelcomeViewModel.SignInButtonText;
            createAccountButton.Text = WelcomeViewModel.CreateAccountButtonText;
            tAndCButton.Text = WelcomeViewModel.TAndCButtonText;
            privayPolicyButton.Text = WelcomeViewModel.PrivacyPolicyButtonText;

            _eventToCommandInfo.Add(signInButton.SetDetachableCommand(_viewModel!.SignInCommand));
            _eventToCommandInfo.Add(createAccountButton.SetDetachableCommand(_viewModel!.CreateAccountCommand));
            _eventToCommandInfo.Add(tAndCButton.SetDetachableCommand(_viewModel!.TermsOfServiceCommand));
            _eventToCommandInfo.Add(privayPolicyButton.SetDetachableCommand(_viewModel!.PrivacyPolicyCommand));
        }
    }
}