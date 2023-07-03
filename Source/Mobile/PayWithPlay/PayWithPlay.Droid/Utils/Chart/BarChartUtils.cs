using AndroidX.Core.Content.Resources;
using AndroidX.Core.Content;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using static MikePhil.Charting.Components.XAxis;
using PayWithPlay.Core.Models.Chart;
using Android.Runtime;

namespace PayWithPlay.Droid.Utils.Chart
{
    public static class BarChartUtils
    {
        public static void SetChartProperties(BarChart barChart)
        {
            barChart.SetTouchEnabled(false);
            barChart.DoubleTapToZoomEnabled = false;
            barChart.Legend.Enabled = false;
            barChart.Description = null;
            barChart.SetPadding(0, 0, 0, 0);
            barChart.SetExtraOffsets(0, 0, 0, 0);
            barChart.SetPinchZoom(false);
            barChart.SetDrawValueAboveBar(false);
            barChart.SetDrawGridBackground(false);
            barChart.SetDrawBarShadow(false);
            barChart.SetDrawBorders(false);

            var xAxis = barChart.XAxis;
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

            var leftAxis = barChart.AxisLeft;
            leftAxis.SetDrawAxisLine(true);
            leftAxis.SetDrawGridLines(false);
            leftAxis.SetDrawGridLinesBehindData(false);
            leftAxis.AxisLineWidth = 1;
            leftAxis.AxisLineColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            leftAxis.Typeface = ResourcesCompat.GetFont(App.Context, Resource.Font.poppins_regular);
            leftAxis.TextSize = 8;

            var rightAxis = barChart.AxisRight;
            rightAxis.Enabled = false;
            rightAxis.SetDrawAxisLine(false);
            rightAxis.SetDrawGridLines(false);
            rightAxis.SetDrawGridLinesBehindData(false);
        }

        public static void SetBarDataSetProperties(BarDataSet barDataSet)
        {
            if (barDataSet == null)
            {
                return;
            }

            barDataSet.HighlightEnabled = false;
            barDataSet.Color = ContextCompat.GetColor(App.Context, Resource.Color.chart_primary_color);
            barDataSet.ValueTextColor = ContextCompat.GetColor(App.Context, Resource.Color.primary_text_color);
            barDataSet.ValueTextSize = 8;
            barDataSet.ValueTypeface = ResourcesCompat.GetFont(App.Context, Resource.Font.poppins_regular);
        }

        public static void SetBarDataProperties(BarData barData)
        {
            if (barData == null)
            {
                return;
            }

            barData.BarWidth = 0.65f;
        }

        public static void SetBarEntries(List<ChartEntry> chartEntries, BarChart chart, bool showVerticalLabels)
        {
            if (chartEntries == null || chartEntries.Count == 0)
            {
                if (chart.BarData != null)
                {
                    chart!.ClearValues();
                    chart.NotifyDataSetChanged();
                    chart.Invalidate();
                    chart.Data = null;
                }

                return;
            }

            var barEntries = new List<BarEntry>();
            foreach (var chartEntry in chartEntries!)
            {
                barEntries.Add(new BarEntry(chartEntry.X, chartEntry.Y) { Data = new Java.Lang.String(chartEntry.Title) });
            }

            if (chart!.Data is BarData lineData &&
                lineData.DataSets[0] is BarDataSet curentBarDataSet)
            {
                chart.XAxis.LabelCount = barEntries.Count;
                curentBarDataSet.Clear();
                curentBarDataSet.Values = barEntries;
                curentBarDataSet.NotifyDataSetChanged();
                chart.Data.NotifyDataChanged();
                chart.NotifyDataSetChanged();
                chart.Invalidate();
            }
            else
            {
                chart.XAxis.LabelCount = barEntries.Count;

                var barDataSet = new BarDataSet(barEntries, null);
                if (showVerticalLabels)
                {
                    barDataSet.SetDrawValues(true);
                    barDataSet.ValueFormatter = new CustomValueFormatter()
                    {
                        OnFormattedValue = (value) =>
                        {
                            if (chart!.Data is BarData lineData &&
                                lineData.DataSets[0] is BarDataSet curentBarDataSet && curentBarDataSet.Values is JavaList barEntriesJavaList)
                            {
                                var barEntries = barEntriesJavaList.Cast<BarEntry>();
                                var entry = barEntries.FirstOrDefault(t => t.GetY() == value);
                                if (entry != null && entry.Data is Java.Lang.String data)
                                {
                                    return data.ToString();
                                }
                            }

                            return value.ToString();
                        }
                    };
                }
                else
                {
                    barDataSet.SetDrawValues(false);
                }

                SetBarDataSetProperties(barDataSet);

                var barData = new BarData(barDataSet);
                SetBarDataProperties(barData);

                chart.Data = barData;
            }

            chart.AnimateY(800);
        }
    }
}
