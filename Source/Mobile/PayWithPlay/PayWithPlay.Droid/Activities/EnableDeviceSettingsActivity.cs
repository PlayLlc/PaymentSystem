using Android.Content.PM;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.ViewModels.CreateAccount;

namespace PayWithPlay.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Mobility.NoActionBar", ScreenOrientation = ScreenOrientation.UserPortrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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
