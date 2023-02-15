using Microsoft.Extensions.DependencyInjection;
using PayWithPlay.Android.Lifecycle;
using PayWithPlay.Core;
using PayWithPlay.Core.ViewModels.CreateAccount;
using PayWithPlay.Android.Extensions;
using Google.Android.Material.Button;
using CommunityToolkit.Mvvm.Bindings;
using PayWithPlay.Android.Adapters;
using AndroidX.RecyclerView.Widget;

namespace PayWithPlay.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar")]
    public class EnableDeviceSettingsActivity : BaseActivity, EnableDeviceSettingsViewModel.INavigationService
    {
        private MaterialButton? _continueBtn;

        private DeviceSettingsRecyclerAdapter _adapter = new();
        private EnableDeviceSettingsViewModel? _viewModel;

        protected override int LayoutId => Resource.Layout.activity_enable_device_settings;

        public void NavigateToNextPage()
        {
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = ViewModelProviders.Of(this).Get(ServicesProvider.Current.Provider.GetService<EnableDeviceSettingsViewModel>);
            _viewModel.NavigationService = this;

            InitViews();
            SetBindings();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_viewModel != null)
            {
                _viewModel.NavigationService = null;
            }

            _adapter.SafeDispose();
        }

        private void SetBindings()
        {
            var title = FindViewById<TextView>(Resource.Id.title_textView)!;
            var subTitle = FindViewById<TextView>(Resource.Id.subTitle_textView)!;

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.settings_rv)!;
            _adapter.Items = _viewModel.Settings;
            recyclerView.SetAdapter(_adapter);

            title.Text = EnableDeviceSettingsViewModel.Title;
            subTitle.Text = EnableDeviceSettingsViewModel.Subtitle;

            _continueBtn.Text = EnableDeviceSettingsViewModel.ContinueButtonText;
            _eventToCommandInfo.Add(_continueBtn.SetDetachableCommand(_viewModel!.ContinueCommand));
        }

        private void InitViews()
        {
            _continueBtn = FindViewById<MaterialButton>(Resource.Id.continue_btn)!;
        }
    }
}
