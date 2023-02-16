using PayWithPlay.Core.Enums;
using PayWithPlay.Core.ViewModels.CreateAccount;

namespace PayWithPlay.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar")]
    public class EnableDeviceSettingsActivity : BaseActivity<EnableDeviceSettingsViewModel>
    {
        protected override int LayoutId => Resource.Layout.activity_enable_device_settings;

        public void NavigateToNextPage()
        {
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnSettingItemAction = OnSettingItem;
        }

        protected override void OnDestroy()
        {
            ViewModel.OnSettingItemAction = null;

            base.OnDestroy();
        }

        private void OnSettingItem(DeviceSettingsType obj)
        {
            ViewModel.Settings.First(t => t.Type == obj).Enabled = true;
        }
    }
}
