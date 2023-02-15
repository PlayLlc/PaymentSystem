using Android.Content;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CommunityToolkit.Mvvm.Bindings;
using Google.Android.Material.Button;
using Microsoft.Extensions.DependencyInjection;
using PayWithPlay.Android.Activities;
using PayWithPlay.Android.Activities.CreateAccount;
using PayWithPlay.Android.CustomViews;
using PayWithPlay.Android.Extensions;
using PayWithPlay.Android.Lifecycle;
using PayWithPlay.Core;
using PayWithPlay.Core.ViewModels.CreateAccount.VerifyIdentity;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace PayWithPlay.Android.Fragments.UserRegistration
{
    public class VerifyIdentityFragment : Fragment, BaseVerifyIdentityViewModel.INavigationService
    {
        private readonly List<EventToCommandInfo> _eventToCommandInfo = new();
        private readonly List<Binding> _bindings = new();

        private readonly BaseVerifyIdentityViewModel.VerifyIdentity _verifyIdentityType;
        private BaseVerifyIdentityViewModel? _viewModel;

        private InputBoxesView? _inputBoxes;
        private MaterialButton? _verifyBtn;
        private MaterialButton? _resendCodeBtn;

        public VerifyIdentityFragment(BaseVerifyIdentityViewModel.VerifyIdentity verifyIdentityType)
        {
            _verifyIdentityType = verifyIdentityType;
        }

        public static VerifyIdentityFragment NewInstance(BaseVerifyIdentityViewModel.VerifyIdentity verifyIdentityType)
        {
            return new VerifyIdentityFragment(verifyIdentityType);
        }

        public void NavigateToNextPage()
        {
            var intent = new Intent(RequireActivity(), typeof(EnableDeviceSettingsActivity));
            StartActivity(intent);
        }

        public override void OnStart()
        {
            base.OnStart();

            _viewModel!.NavigationService = this;
        }

        public override void OnStop()
        {
            base.OnStop();

            _viewModel!.NavigationService = null;
        }

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (_verifyIdentityType == BaseVerifyIdentityViewModel.VerifyIdentity.Email)
            {
                _viewModel = ViewModelProviders.Of(this).Get(ServicesProvider.Current.Provider.GetService<VerifyEmailViewModel>);
            }
            else
            {
                _viewModel = ViewModelProviders.Of(this).Get(ServicesProvider.Current.Provider.GetService<VerifyPhoneNumberViewModel>);
            }
        }

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.fragment_verify_identity, container, false);

            InitViews(root);
            SetBindings(root);

            return root;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            _bindings.DetachAll();
            _eventToCommandInfo.DetachAll();
        }

        private void SetBindings(View root)
        {
            var title = root.FindViewById<TextView>(Resource.Id.title_textView)!;
            var subTitle = root.FindViewById<TextView>(Resource.Id.subTitle_textView)!;
            var message = root.FindViewById<TextView>(Resource.Id.message_textView)!;
            var expiresInTextView = root.FindViewById<AppCompatTextView>(Resource.Id.expires_in_tv)!;

            var identityImage = root.FindViewById<AppCompatImageView>(Resource.Id.identity_image)!;
            if (_verifyIdentityType == BaseVerifyIdentityViewModel.VerifyIdentity.Email)
            {
                identityImage.SetImageResource(Resource.Drawable.verify_email_image);
            }
            else
            {
                identityImage.SetImageResource(Resource.Drawable.verify_phone_number_image);
            }

            title.Text = _viewModel!.Title;
            subTitle.Text = _viewModel!.Subtitle;
            message.Text = _viewModel!.Message;
            _verifyBtn!.Text = BaseVerifyIdentityViewModel.VerifyButtonText;

            expiresInTextView.Text = BaseVerifyIdentityViewModel.ExipresAfter;
            _resendCodeBtn!.Text = BaseVerifyIdentityViewModel.ResendCodeButtonText;

            _bindings.Add(this.SetBinding(() => _viewModel.InputValue, () => _inputBoxes!.TextValue, BindingMode.TwoWay));

            _eventToCommandInfo.Add(_verifyBtn.SetDetachableCommand(_viewModel!.VerifyCommand));
            _eventToCommandInfo.Add(_resendCodeBtn.SetDetachableCommand(_viewModel!.ResendCommand));
        }

        private void InitViews(View root)
        {
            _inputBoxes = root.FindViewById<InputBoxesView>(Resource.Id.inputsView)!;
            _verifyBtn = root.FindViewById<MaterialButton>(Resource.Id.verify_btn)!;
            _resendCodeBtn = root.FindViewById<MaterialButton>(Resource.Id.resend_code_btn)!;
        }
    }
}
