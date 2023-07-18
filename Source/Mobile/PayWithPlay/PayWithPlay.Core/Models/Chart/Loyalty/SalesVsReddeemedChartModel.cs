using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Loyalty
{
    public class SalesVsReddeemedChartModel : MvxNotifyPropertyChanged
    {
        private bool _isLoading;
        public SalesVsReddeemedChartModel()
        {
            IsLoading = true;
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string SalesVsRedeemedText => Resource.LoyatlySalesVsRedeemed;
        public string LoyaltySalesText => Resource.LoyaltySales;
        public string RedeemedText => Resource.Redeemed;

        public List<ChartEntry>? SalesEntries { get; set; }
        public List<ChartEntry>? RedeemedEntries { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public void ReloadData()
        {
            SalesEntries = MockDataUtils.RandomLoyaltySalesVsRedeemedChartData(20000, 120000);
            RedeemedEntries = MockDataUtils.RandomLoyaltySalesVsRedeemedChartData(5000, 60000);

            IsLoading = false;
            ChartEntriesChangedAction?.Invoke();
        }
    }
}
