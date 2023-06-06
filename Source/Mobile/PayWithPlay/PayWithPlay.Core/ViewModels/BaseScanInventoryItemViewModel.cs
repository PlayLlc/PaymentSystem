using PayWithPlay.Core.Models.Inventory;
using PayWithPlay.Core.Resources;
using System.Collections.ObjectModel;

namespace PayWithPlay.Core.ViewModels
{
    public abstract class BaseScanInventoryItemViewModel : BaseViewModel
    {
        private bool _isLoading;
        private bool _isScanning;

        public BaseScanInventoryItemViewModel()
        {
            IsScanning = true;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            OnNewScanAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public abstract string Title { get; }
        public string ResultsText => Resource.Results;
        public string NewScanButtonText => Resource.NewScan;

        public Action? OnNewScanAction { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsScanning
        {
            get => _isScanning;
            set => SetProperty(ref _isScanning, value);
        }

        public ObservableCollection<InventoryItemModel> Items { get; set; } = new ObservableCollection<InventoryItemModel>();

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnNewScan()
        {
            OnNewScanAction?.Invoke();

            IsScanning = true;
        }

        public abstract void OnScanResult(string value);

        public abstract void OnInventoryItem(InventoryItemModel inventoryItemModel);
    }
}
