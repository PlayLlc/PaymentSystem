using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Inventory
{
    public class InventoryOnHandChartModel
    {
        public InventoryOnHandChartModel()
        {
            ReloadData();
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string DaysInventoryOnHandText => Resource.DaysInventoryOnHand;

        public List<ChartEntry>? Entries { get; set; }

        public void ReloadData()
        {
            Entries = MockDataUtils.RandomInventoryOnHandChartData();

            ChartEntriesChangedAction?.Invoke();
        }
    }
}
