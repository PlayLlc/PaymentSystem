namespace PayWithPlay.Core.Models.Chart
{
    public class ChartEntry
    {
        public ChartEntry(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public string? Title { get; set; }
    }
}
