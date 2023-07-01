using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Inventory
{
    public class ShrinkageRateChartModel
    {
        public ShrinkageRateChartModel()
        {
            ReloadData();
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string ShrinkageRateText => Resource.ShrinkageRateByProduct;

        public List<ChartEntry>? Entries { get; set; }

        public void ReloadData()
        {
            Entries = MockDataUtils.RandomShrinkageRateChartData();

            ChartEntriesChangedAction?.Invoke();
        }
    }
}
