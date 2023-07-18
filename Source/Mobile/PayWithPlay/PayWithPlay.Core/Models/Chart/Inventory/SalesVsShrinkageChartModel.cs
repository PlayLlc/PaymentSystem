using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Inventory
{
    public class SalesVsShrinkageChartModel : MvxNotifyPropertyChanged
    {
        private bool _isLoading;

        public SalesVsShrinkageChartModel()
        {
            IsLoading = true;
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string SalesVsShrinkageText => Resource.SalesVsShrinkage;
        public string SalesText => Resource.Sales;
        public string ShrinkageText => Resource.Shrinkage;

        public List<ChartEntry>? SalesEntries { get; set; }
        public List<ChartEntry>? ShrinkageEntries { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public void ReloadData()
        {
            SalesEntries = MockDataUtils.RandomSalesVsShrinkageChartData(20000, 120000);
            ShrinkageEntries = MockDataUtils.RandomSalesVsShrinkageChartData(5000, 60000);

            IsLoading = false;
            ChartEntriesChangedAction?.Invoke();
        }
    }
}
