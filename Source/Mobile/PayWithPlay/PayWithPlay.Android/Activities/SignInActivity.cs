using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.Lifecycle;
using CommunityToolkit.Mvvm.Bindings;
using Microsoft.Extensions.DependencyInjection;
using PayWithPlay.Android.Extensions;
using PayWithPlay.Android.Lifecycle;
using PayWithPlay.Core;
using PayWithPlay.Core.ViewModels.SignIn;
using PayWithPlay.Core.ViewModels.Welcome;

namespace PayWithPlay.Android.Activities;

[Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar")]
public class SignInActivity : AppCompatActivity, SignInViewModel.INavigationService
{
    private readonly List<EventToCommandInfo> _eventToCommandInfo = new List<EventToCommandInfo>();

    private SignInViewModel? _viewModel;

    public void NavigateToCreateAccount()
    {
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_sign_in);
        WindowCompat.SetDecorFitsSystemWindows(Window!, false);

        _viewModel = ViewModelProviders.Of(this).Get(ServicesProvider.Current.Provider.GetService<SignInViewModel>);
        _viewModel.NavigationService = this;

        SetBindings();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(_viewModel != null)
        {
            _viewModel.NavigationService = null;
        }

        _eventToCommandInfo.DetachAll();
    }

    private void SetBindings()
    {
        var pageTitle = FindViewById<TextView>(Resource.Id.title_textView)!;
        var signInButton = FindViewById<Button>(Resource.Id.sign_in_btn)!;
        var noaccountQuestionTextView = FindViewById<TextView>(Resource.Id.no_account_question_tv)!;
        var createAccountButton = FindViewById<Button>(Resource.Id.create_account_btn)!;

        pageTitle.Text = SignInViewModel.Title;
        signInButton.Text = SignInViewModel.SignInButtonText;
        noaccountQuestionTextView.Text = SignInViewModel.NoAccountQuestionText;
        createAccountButton.Text = SignInViewModel.CreateAccountButtonText;

        _eventToCommandInfo.Add(signInButton.SetDetachableCommand(_viewModel!.SignInCommand));
        _eventToCommandInfo.Add(createAccountButton.SetDetachableCommand(_viewModel!.CreateAccountCommand));
    }
}