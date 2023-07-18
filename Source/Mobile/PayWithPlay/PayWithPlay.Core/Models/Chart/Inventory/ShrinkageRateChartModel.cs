using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Inventory
{
    public class ShrinkageRateChartModel : MvxNotifyPropertyChanged
    {
        private bool _isLoading;

        public ShrinkageRateChartModel()
        {
            IsLoading = true;
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string ShrinkageRateText => Resource.ShrinkageRateByProduct;

        public List<ChartEntry>? Entries { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public void ReloadData()
        {
            Entries = MockDataUtils.RandomShrinkageRateChartData();

            IsLoading = false;
            ChartEntriesChangedAction?.Invoke();
        }
    }
}
