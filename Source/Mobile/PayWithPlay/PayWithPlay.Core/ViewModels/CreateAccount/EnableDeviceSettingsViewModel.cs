using CommunityToolkit.Mvvm.Input;
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
                Type = DeviceSettingsType.Location
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

        public interface INavigationService
        {
            void NavigateToNextPage();
        }

        public ObservableCollection<DeviceSettingsItemModel> Settings { get; set; } = new();

        public INavigationService? NavigationService { get; set; }

        public static string Title => Resource.EnableDeviceSettingsTitle;
        public static string Subtitle => Resource.EnableDeviceSettingsSubtitle;
        public static string ContinueButtonText => Resource.Continue;

        [RelayCommand]
        public void OnContinue()
        {
            NavigationService?.NavigateToNextPage();
        }
    }
}
