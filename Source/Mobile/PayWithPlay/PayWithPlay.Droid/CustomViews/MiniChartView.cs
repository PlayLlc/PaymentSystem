using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using PayWithPlay.Core.Models.Chart;
using PayWithPlay.Droid.Utils.Chart;

namespace PayWithPlay.Droid.CustomViews
{
    public class MiniChartView : FrameLayout
    {
        private AppCompatImageView? _indicatorIcon;
        private AppCompatTextView? _value;
        private LineChart? _lineChart;

        #region ctors

        public MiniChartView(Context context) : base(context)
        {
            Init();
        }

        public MiniChartView(Context context, IAttributeSet? attrs) : base(context, attrs)
        {
            Init();
        }

        public MiniChartView(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public MiniChartView(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        protected MiniChartView(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        public void SetData(bool isPositive, string value, List<ChartEntry> chartEntries) 
        {
            _value!.Text = value;

            int stateColor; 
            if (isPositive) 
            {
                stateColor = ContextCompat.GetColor(Context, Resource.Color.positive_color);
                _indicatorIcon!.Rotation = 0;
            }
            else 
            {
                stateColor = ContextCompat.GetColor(Context, Resource.Color.negative_color);
                _indicatorIcon!.Rotation = 180;
            }

            _indicatorIcon!.ImageTintList = ColorStateList.ValueOf(new Color(stateColor));
            _value.SetTextColor(ColorStateList.ValueOf(new Color(stateColor)));

            SetAvgRevenueChartData(chartEntries, stateColor);
        }


        private void Init()
        {
            Inflate(Context, Resource.Layout.view_mini_chart, this);

            _indicatorIcon = FindViewById<AppCompatImageView>(Resource.Id.small_indicator_icon)!;
            _value = FindViewById<AppCompatTextView>(Resource.Id.value)!;
            _lineChart = FindViewById<LineChart>(Resource.Id.chart)!;

            MiniLineChartUtils.SetChartProperties(_lineChart);
        }

        private void SetAvgRevenueChartData(List<ChartEntry> chartEntries, int color)
        {
            var entries = new List<Entry>();

            foreach (var chartEntry in chartEntries)
            {
                entries.Add(new Entry(chartEntry.X, chartEntry.Y));
            }

            if (_lineChart!.Data is LineData lineData &&
                lineData.DataSets[0] is LineDataSet currentLineDataSet)
            {
                currentLineDataSet.Clear();
                currentLineDataSet.Values = entries;
                currentLineDataSet.Color = color;
                currentLineDataSet.NotifyDataSetChanged();
                _lineChart.Data.NotifyDataChanged();
                _lineChart.NotifyDataSetChanged();
            }
            else
            {
                var lineDataSet = new LineDataSet(entries, string.Empty)
                {
                    Color = color
                };
                MiniLineChartUtils.SetDataSetProperties(lineDataSet);
                _lineChart!.Data = new LineData(lineDataSet);
            }
        }
    }
}
