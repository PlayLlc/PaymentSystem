using Android.Graphics;
using Android.Runtime;
using MikePhil.Charting.Animation;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MikePhil.Charting.Interfaces.Dataprovider;
using MikePhil.Charting.Renderer;
using MikePhil.Charting.Util;

namespace PayWithPlay.Droid.Utils.Chart
{
    internal class VerticalLabelBarChartRenderer : BarChartRenderer
    {
        public VerticalLabelBarChartRenderer(IBarDataProvider chart, ChartAnimator animator, ViewPortHandler viewPortHandler) : base(chart, animator, viewPortHandler)
        {
        }

        protected VerticalLabelBarChartRenderer(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void DrawValue(Canvas? c, IValueFormatter? formatter, float value, Entry? entry, int dataSetIndex, float x, float y, int color)
        {
            var valueText = formatter?.GetFormattedValue(value, entry, dataSetIndex, this.MViewPortHandler);
            if (string.IsNullOrWhiteSpace(valueText)) 
            {
                return;
            }

            Rect textBounds = new Rect();
            MValuePaint!.GetTextBounds(valueText, 0, valueText.Length, textBounds);

            y = Math.Min((MChart!.Height / 2), MChart.Height - MValuePaint.TextSize);
            x += Math.Abs(textBounds.Height()) / 2;

            c.Save();

            c.Rotate(-90f, x, y);
            c.DrawText(valueText, x, y, MValuePaint);

            c.Restore();
        }
    }
}
