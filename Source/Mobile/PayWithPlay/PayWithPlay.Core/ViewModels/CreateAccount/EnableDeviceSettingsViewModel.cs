using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models;
using PayWithPlay.Core.Resources;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels.CreateAccount
{
    public partial class EnableDeviceSettingsViewModel : BaseViewModel
    {
        public EnableDeviceSettingsViewModel()
        {
            Settings.Add(new DeviceSettingsItemModel
            {
                Title = Resource.EnableDeviceLocation,
                Type = DeviceSettingsType.Location,
                Enabled = true
            });
            Settings.Add(new DeviceSettingsItemModel
            {
                Title = Resource.EnableDeviceStorage,
                Type = DeviceSettingsType.Storage
            });
            Settings.Add(new DeviceSettingsItemModel
            {
                Title = Resource.EnablePhoneAccess,
                Type = DeviceSettingsType.PhoneAccess
            });
        }

        public ObservableCollection<DeviceSettingsItemModel> Settings { get; set; } = new();

        public Action<DeviceSettingsType>? OnSettingItemAction { get; set; }

        public string Title => Resource.EnableDeviceSettingsTitle;
        public string Subtitle => Resource.EnableDeviceSettingsSubtitle;
        public string ContinueButtonText => Resource.Continue;

        public void OnSettingItem(DeviceSettingsItemModel deviceSettingsItemModel)
        {
            OnSettingItemAction?.Invoke(deviceSettingsItemModel.Type);
        }

        public void OnContinue()
        {
        }
    }
}
