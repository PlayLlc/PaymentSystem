using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Loyalty
{
    public class SalesVsReddeemedChartModel
    {
        public SalesVsReddeemedChartModel()
        {
            ReloadData();
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string SalesVsRedeemedText => Resource.LoyatlySalesVsRedeemed;
        public string LoyaltySalesText => Resource.LoyaltySales;
        public string RedeemedText => Resource.Redeemed;

        public List<ChartEntry>? SalesEntries { get; set; }
        public List<ChartEntry>? RedeemedEntries { get; set; }

        public void ReloadData()
        {
            SalesEntries = MockDataUtils.RandomLoyaltySalesVsRedeemedChartData(20000, 120000);
            RedeemedEntries = MockDataUtils.RandomLoyaltySalesVsRedeemedChartData(5000, 60000);

            ChartEntriesChangedAction?.Invoke();
        }
    }
}
