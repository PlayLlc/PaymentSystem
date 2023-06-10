namespace PayWithPlay.Core.Models.Chart
{
    public class MiniChartModel
    {
        public string ValueDisplayed => $"{Value * 100:0.00}%";

        public float Value { get; set; }

        public bool IsPositive { get; set; }

        public List<ChartEntry>? Entries { get; set; }
    }
}
