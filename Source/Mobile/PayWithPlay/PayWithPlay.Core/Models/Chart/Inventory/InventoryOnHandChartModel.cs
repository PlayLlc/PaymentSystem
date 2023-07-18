using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Inventory
{
    public class InventoryOnHandChartModel : MvxNotifyPropertyChanged
    {
        private bool _isLoading;

        public InventoryOnHandChartModel()
        {
            IsLoading = true;
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string DaysInventoryOnHandText => Resource.DaysInventoryOnHand;

        public List<ChartEntry>? Entries { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public void ReloadData()
        {
            Entries = MockDataUtils.RandomInventoryOnHandChartData();

            IsLoading = false;
            ChartEntriesChangedAction?.Invoke();
        }
    }
}
