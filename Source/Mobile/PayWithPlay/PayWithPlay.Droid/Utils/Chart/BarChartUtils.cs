using AndroidX.Core.Content.Resources;
using AndroidX.Core.Content;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using static MikePhil.Charting.Components.XAxis;
using PayWithPlay.Core.Models.Chart;

namespace PayWithPlay.Droid.Utils.Chart
{
    public static class BarChartUtils
    {
        public static void SetChartProperties(BarChart barChart)
        {
            barChart.SetTouchEnabled(false);
            barChart.DoubleTapToZoomEnabled = false;
            barChart.Legend!.Enabled = false;
            barChart.Description = null;
            barChart.SetPadding(0, 0, 0, 0);
            barChart.SetExtraOffsets(0, 0, 0, 0);
            barChart.SetPinchZoom(false);
            barChart.DragYEnabled = false;
            barChart.DragXEnabled = false;
            barChart.DragEnabled = false;
            barChart.ScaleXEnabled = false;
            barChart.ScaleYEnabled = false;
            barChart.SetScaleEnabled(false);
            barChart.SetDrawValueAboveBar(false);
            barChart.SetDrawGridBackground(false);
            barChart.SetDrawBarShadow(false);
            barChart.SetDrawBorders(false);

            var xAxis = barChart.XAxis!;
            xAxis.Position = XAxisPosition.Bottom;
            xAxis.SetDrawGridLines(false);
            xAxis.SetDrawGridLinesBehindData(false);
            xAxis.SetDrawAxisLine(true);
            xAxis.SetCenterAxisLabels(false);
            xAxis.YOffset = 0;
            xAxis.Granularity = 1f;
            xAxis.GranularityEnabled = true;
            xAxis.AxisLineWidth = 1;
            xAxis.AxisLineColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            xAxis.Typeface = ResourcesCompat.GetFont(App.Context, Resource.Font.poppins_regular);
            xAxis.TextSize = 8;

            var leftAxis = barChart.AxisLeft!;
            leftAxis.SetDrawAxisLine(true);
            leftAxis.SetDrawGridLines(false);
            leftAxis.SetDrawGridLinesBehindData(false);
            leftAxis.AxisLineWidth = 1;
            leftAxis.AxisLineColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            leftAxis.Typeface = ResourcesCompat.GetFont(App.Context, Resource.Font.poppins_regular);
            leftAxis.TextSize = 8;

            var rightAxis = barChart.AxisRight!;
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

        public static void SetBarDataProperties(BarData barData, float barWidth = 0.65f)
        {
            if (barData == null)
            {
                return;
            }

            barData.BarWidth = barWidth;
        }

        public static void SetBarEntries(List<ChartEntry> chartEntries, BarChart chart, bool showVerticalLabels, bool animate = false, float barWidth = 0.65f)
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
            var min = 0f;
            var max = 0f;
            foreach (var chartEntry in chartEntries!)
            {
                if (min > chartEntry.Y)
                {
                    min = chartEntry.Y;
                }
                else if (max < chartEntry.Y)
                {
                    max = chartEntry.Y;
                }

                barEntries.Add(new BarEntry(chartEntry.X, chartEntry.Y) { Data = new Java.Lang.String(chartEntry.Title) });
            }

            chart.AxisLeft!.AxisMinimum = min;
            chart.AxisLeft.AxisMaximum = max;
            chart.XAxis!.AxisMaxLabels = barEntries.Count;
            chart.XAxis!.MEntryCount = barEntries.Count;
            chart.XAxis!.SetLabelCount(barEntries.Count, true);
            chart.XAxis.LabelCount = barEntries.Count;

            if (chart!.Data is BarData lineData &&
                lineData.DataSets![0] is BarDataSet curentBarDataSet)
            {
                curentBarDataSet.Clear();
                curentBarDataSet.Entries = barEntries;
                curentBarDataSet.NotifyDataSetChanged();
                chart.Data.NotifyDataChanged();
                chart.NotifyDataSetChanged();
                chart.Invalidate();
            }
            else
            {
                var barDataSet = new BarDataSet(barEntries, null);
                if (showVerticalLabels)
                {
                    barDataSet.SetDrawValues(true);
                    barDataSet.ValueFormatter = new CustomValueFormatter()
                    {
                        OnFormattedValue = (value, entry, dataSetIndex, viewPortHandler) =>
                        {
                            if (entry != null && entry.Data is Java.Lang.String data)
                            {
                                return data.ToString();
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
                SetBarDataProperties(barData, barWidth);

                chart.Data = barData;
            } 

            //if (animate)
            //{
            //    chart.AnimateY(800);
            //}
            //else 
            //{
            //    chart.Invalidate();
            //}
        }
    }
}
