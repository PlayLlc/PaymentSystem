using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Models.Inventory.CreateItem
{
    public class AlertsModel : MvxNotifyPropertyChanged
    {
        private bool _alertsExpanded;
        private bool _alertsEnabled = true;
        private string? _lowInventoryValue;

        public string AlertsText => Resource.Alerts;
        public string AlertsMessageText => Resource.LowInventoryItemAlert;
        public string LowInventoryWarningText => Resource.LowInventoryItemWarning;

        public bool AlertsExpanded
        {
            get => _alertsExpanded;
            set => SetProperty(ref _alertsExpanded, value);
        }

        public bool AlertsEnabled
        {
            get => _alertsEnabled;
            set => SetProperty(ref _alertsEnabled, value);
        }

        public string? LowInventoryValue
        {
            get => _lowInventoryValue;
            set => SetProperty(ref _lowInventoryValue, value);
        }

        public void OnAlerts()
        {
            AlertsExpanded = !AlertsExpanded;
        }
    }
}
