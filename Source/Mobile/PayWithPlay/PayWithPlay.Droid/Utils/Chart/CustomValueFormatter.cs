using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;

namespace PayWithPlay.Droid.Utils.Chart
{
    public class CustomValueFormatter : ValueFormatter
    {
        public Func<Entry, string>? OnPointLabel { get; set; }
        public Func<float, AxisBase, string>? OnAxisLabel { get; set; }
        public Func<float, string>? OnFormattedValue { get; set; }

        public override string GetPointLabel(Entry entry)
        {
            if (OnPointLabel != null) 
            {
                return OnPointLabel.Invoke(entry);
            }

            return base.GetPointLabel(entry);
        }

        public override string GetAxisLabel(float value, AxisBase axis)
        {
            if (OnAxisLabel != null)
            {
                return OnAxisLabel.Invoke(value, axis);
            }

            return base.GetAxisLabel(value, axis);
        }

        public override string GetFormattedValue(float value)
        {
            if (OnFormattedValue != null)
            {
                return OnFormattedValue.Invoke(value);
            }

            return base.GetFormattedValue(value);
        }
    }
}
