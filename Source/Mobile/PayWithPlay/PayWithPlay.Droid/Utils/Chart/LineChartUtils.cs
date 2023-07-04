using AndroidX.Core.Content.Resources;
using AndroidX.Core.Content;
using MikePhil.Charting.Charts;
using static MikePhil.Charting.Components.XAxis;
using MikePhil.Charting.Data;
using PayWithPlay.Core.Models.Chart;
using Android.Graphics.Drawables;

namespace PayWithPlay.Droid.Utils.Chart
{
    public static class LineChartUtils
    {
        public static void SetChartProperties(LineChart lineChart)
        {
            lineChart.SetTouchEnabled(false);
            lineChart.DoubleTapToZoomEnabled = false;
            lineChart.Legend.Enabled = false;
            lineChart.Description = null;
            lineChart.SetPadding(0, 0, 0, 0);
            lineChart.SetExtraOffsets(0, 0, 0, 0);
            lineChart.SetPinchZoom(false);
            lineChart.SetDrawGridBackground(false);
            lineChart.SetDrawBorders(false);

            var xAxis = lineChart.XAxis;
            xAxis.Position = XAxisPosition.Bottom;
            xAxis.SetDrawGridLines(false);
            xAxis.SetDrawGridLinesBehindData(false);
            xAxis.SetDrawAxisLine(true);
            xAxis.YOffset = 0;
            xAxis.Granularity = 1f;
            xAxis.AxisLineWidth = 1;
            xAxis.AxisLineColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            xAxis.Typeface = ResourcesCompat.GetFont(App.Context, Resource.Font.poppins_regular);
            xAxis.TextSize = 8;

            var leftAxis = lineChart.AxisLeft;
            leftAxis.SetDrawAxisLine(true);
            leftAxis.SetDrawGridLines(false);
            leftAxis.SetDrawGridLinesBehindData(false);
            leftAxis.AxisLineWidth = 1;
            leftAxis.AxisLineColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            leftAxis.Typeface = ResourcesCompat.GetFont(App.Context, Resource.Font.poppins_regular);
            leftAxis.TextSize = 8;

            var rightAxis = lineChart.AxisRight;
            rightAxis.Enabled = false;
            rightAxis.SetDrawAxisLine(false);
            rightAxis.SetDrawGridLines(false);
            rightAxis.SetDrawGridLinesBehindData(false);
        }

        public static void SetLineDataSetProperties(LineDataSet lineDataSet, int coloResId, bool drawCircles = false, float lineWidth = 1f)
        {
            if (lineDataSet == null)
            {
                return;
            }

            lineDataSet.HighlightEnabled = false;
            lineDataSet.Color = ContextCompat.GetColor(App.Context, coloResId);
            lineDataSet.LineWidth = lineWidth;
            lineDataSet.SetDrawValues(false);
            lineDataSet.SetDrawCircleHole(false);
            lineDataSet.SetDrawHighlightIndicators(false);
            lineDataSet.SetMode(LineDataSet.Mode.CubicBezier);

            if (drawCircles) 
            {
                lineDataSet.SetDrawCircles(true);
                lineDataSet.CircleRadius = 1;
                lineDataSet.SetCircleColor(lineDataSet.Color);
            }
            else
            {
                lineDataSet.SetDrawCircles(false);
            }
        }

        public static void SetLineDataProperties(LineData lineData)
        {
            if (lineData == null)
            {
                return;
            }

            lineData.SetDrawValues(false);
        }

        public static LineDataSet? GetLineDataSet(List<ChartEntry> lineEntries, int colorResId, bool drawCircles = false, float lineWidth = 1f, GradientDrawable? fillDrawable = null)
        {
            if (lineEntries == null || lineEntries.Count == 0) 
            {
                return null;
            }

            var entries = new List<Entry>();
            foreach (var entry in lineEntries)
            {
                entries.Add(new Entry(entry.X, entry.Y) { Data = new Java.Lang.String(entry.Title) });
            }

            var lineDataSet = new LineDataSet(entries, null);

            SetLineDataSetProperties(lineDataSet, colorResId, drawCircles, lineWidth);

            if (fillDrawable != null)
            {
                lineDataSet.FillDrawable = fillDrawable;
                lineDataSet.SetDrawFilled(true);
            }
            else 
            {
                lineDataSet.FillDrawable = null;
                lineDataSet.SetDrawFilled(false);
            }

            return lineDataSet;
        }

        public static void SetDataSets(LineChart chart, params LineDataSet?[] lineDataSets)
        {
            if (lineDataSets == null || lineDataSets.Length == 0 || lineDataSets.All(t => t == null))
            {
                if (chart.LineData != null)
                {
                    chart!.ClearValues();
                    chart.NotifyDataSetChanged();
                    chart.Invalidate();
                    chart.Data = null;
                }

                return;
            }

            var lineData = new LineData();
            var xAxisLabelCount = 0;
            foreach (var lineDataSet in lineDataSets) 
            {
                if (lineDataSet != null) 
                {
                    lineData.AddDataSet(lineDataSet);
                    if (lineDataSet.Values.Count > xAxisLabelCount)
                    {
                        xAxisLabelCount = lineDataSet.Values.Count;
                    }
                }
            }

            chart.XAxis.LabelCount = xAxisLabelCount;
            SetLineDataProperties(lineData);
            if (chart.Data != null) 
            {
                chart.ClearValues();

            }
            chart.Data = lineData;
            chart.AnimateX(800);
        }
    }
}
