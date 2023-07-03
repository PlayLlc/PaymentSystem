using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Inventory
{
    public class SalesVsShrinkageChartModel
    {
        public SalesVsShrinkageChartModel()
        {
            ReloadData();
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string SalesVsShrinkageText => Resource.SalesVsShrinkage;
        public string SalesText => Resource.Sales;
        public string ShrinkageText => Resource.Shrinkage;

        public List<ChartEntry>? SalesEntries { get; set; }
        public List<ChartEntry>? ShrinkageEntries { get; set; }

        public void ReloadData()
        {
            SalesEntries = MockDataUtils.RandomSalesVsShrinkageChartData(20000, 120000);
            ShrinkageEntries = MockDataUtils.RandomSalesVsShrinkageChartData(5000, 60000);

            ChartEntriesChangedAction?.Invoke();
        }
    }
}
