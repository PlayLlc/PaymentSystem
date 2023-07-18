using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MikePhil.Charting.Util;

namespace PayWithPlay.Droid.Utils.Chart
{
    public class CustomValueFormatter : Java.Lang.Object, IValueFormatter, IAxisValueFormatter
    {
        public Func<float, AxisBase, string>? OnAxisLabel { get; set; }
        public Func<float, Entry?, int, ViewPortHandler?, string>? OnFormattedValue { get; set; }

        public string? GetFormattedValue(float value, Entry? entry, int dataSetIndex, ViewPortHandler? viewPortHandler)
        {
            if (OnFormattedValue != null)
            {
                return OnFormattedValue.Invoke(value, entry, dataSetIndex, viewPortHandler);
            }

            return string.Empty;
        }

        public string? GetFormattedValue(float value, AxisBase? axis)
        {
            if (OnAxisLabel != null)
            {
                return OnAxisLabel.Invoke(value, axis);
            }

            return string.Empty;
        }
    }
}
