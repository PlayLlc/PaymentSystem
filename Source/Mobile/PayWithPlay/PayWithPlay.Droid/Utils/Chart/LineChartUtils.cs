using AndroidX.Core.Content.Resources;
using AndroidX.Core.Content;
using MikePhil.Charting.Charts;
using static MikePhil.Charting.Components.XAxis;
using MikePhil.Charting.Data;
using PayWithPlay.Core.Models.Chart;

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

        public static void SetLineDataSetProperties(LineDataSet lineDataSet, int coloResId)
        {
            if (lineDataSet == null)
            {
                return;
            }

            lineDataSet.HighlightEnabled = false;
            lineDataSet.Color = ContextCompat.GetColor(App.Context, coloResId);
            lineDataSet.LineWidth = 1;
            lineDataSet.SetDrawValues(false);
            lineDataSet.SetDrawCircles(false);
            lineDataSet.SetDrawCircleHole(false);
            lineDataSet.SetDrawHighlightIndicators(false);
            lineDataSet.SetMode(LineDataSet.Mode.CubicBezier);
        }

        public static void SetLineDataProperties(LineData lineData)
        {
            if (lineData == null)
            {
                return;
            }

            lineData.SetDrawValues(false);
        }

        public static LineDataSet? GetLineDataSet(List<ChartEntry> lineEntries, int colorResId)
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

            SetLineDataSetProperties(lineDataSet, colorResId);

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
