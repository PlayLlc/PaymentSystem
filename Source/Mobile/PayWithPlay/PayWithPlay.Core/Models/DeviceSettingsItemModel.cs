using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;

namespace PayWithPlay.Core.Models
{
    public class DeviceSettingsItemModel : MvxNotifyPropertyChanged
    {
        private bool _enabled;

        public string? Title { get; set; }

        public DeviceSettingsType Type { get; set; }

        public bool Enabled 
        {
            get => _enabled;
            set => SetProperty(ref _enabled, value);
        }
    }
}