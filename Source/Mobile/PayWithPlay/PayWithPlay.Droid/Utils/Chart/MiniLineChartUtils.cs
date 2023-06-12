using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;

namespace PayWithPlay.Droid.Utils.Chart
{
    public static class MiniLineChartUtils
    {
        public static void SetChartProperties(LineChart lineChart) 
        {
            if (lineChart == null) 
            {
                return;
            }

            lineChart.SetTouchEnabled(false);
            lineChart.DoubleTapToZoomEnabled = false;
            lineChart.Legend.Enabled = false;
            lineChart.Description = null;
            lineChart.SetPadding(0, 0, 0, 0);
            lineChart.SetViewPortOffsets(0, 0, 0, 0);
            lineChart.SetExtraOffsets(0, 0, 0, 0);
            lineChart.SetPinchZoom(false);

            lineChart.XAxis.AxisMinimum = 0;
            lineChart.XAxis.MAxisMinimum = 0;
            lineChart.XAxis.AxisMaximum = 12;
            lineChart.XAxis.MAxisMaximum = 12;

            SetAxisProperties(lineChart.XAxis);
            SetAxisProperties(lineChart.AxisLeft);
            SetAxisProperties(lineChart.AxisRight);
        }

        public static void SetDataSetProperties(LineDataSet lineDataSet)
        {
            if (lineDataSet == null) 
            {
                return;
            }

            lineDataSet.ValueFormatter = new EmptyValueFormatter();
            lineDataSet.SetDrawCircles(false);
            lineDataSet.SetDrawValues(false);
            lineDataSet.SetDrawCircleHole(false);
            lineDataSet.LineWidth = 1;
        }

        public static void SetAxisProperties(AxisBase axis)
        {
            axis.XOffset = 0;
            axis.YOffset = 0;
            axis.SetDrawLabels(false);
            axis.SetDrawAxisLine(false);
            axis.SetDrawGridLines(false);
            axis.SetDrawGridLinesBehindData(false);
            axis.SetCenterAxisLabels(false);

            if (axis is YAxis yAxis)
            {
                yAxis.SetDrawTopYLabelEntry(false);
                yAxis.SetDrawZeroLine(false);
            }
        }
    }
}
