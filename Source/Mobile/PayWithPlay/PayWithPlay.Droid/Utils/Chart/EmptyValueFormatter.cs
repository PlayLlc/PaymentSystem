using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;

namespace PayWithPlay.Droid.Utils.Chart
{
    public class EmptyValueFormatter : ValueFormatter
    {
        public override string GetPointLabel(Entry entry)
        {
            return string.Empty;
        }

        public override string GetAxisLabel(float value, AxisBase axis)
        {
            return string.Empty;
        }

        public override string GetFormattedValue(float value)
        {
            return string.Empty;
        }
    }
}
