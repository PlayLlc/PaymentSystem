using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MikePhil.Charting.Util;

namespace PayWithPlay.Droid.Utils.Chart
{
    public class EmptyValueFormatter :Java.Lang.Object, IValueFormatter
    {
        public string? GetFormattedValue(float p0, Entry? p1, int p2, ViewPortHandler? p3)
        {
            return string.Empty;
        }
    }
}
