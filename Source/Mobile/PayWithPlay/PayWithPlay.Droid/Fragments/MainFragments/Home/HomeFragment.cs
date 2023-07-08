using Android.Graphics;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using PayWithPlay.Core.ViewModels.Main.Home;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils.Chart;
using static Android.Views.ViewGroup;

namespace PayWithPlay.Droid.Fragments.MainFragments.Home
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(HomeViewModel))]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        private PieChart? _terminalsPieChart;

        private LinearLayoutCompat? _totalSalesContainer;
        private LineChart? _totalSalesChart;

        private LinearLayoutCompat? _avgTransactionValueContainer;
        private LineChart? _avgTransactionValueChart;

        public override int LayoutId => Resource.Layout.fragment_home;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnlineTerminalsAction = SetTerminalsChartData;
            ViewModel.TotalSalesChartModel.ChartEntriesChangedAction = () => TotalSalesChartDataChanged(true);
            ViewModel.AvgTransactionsValueChartModel.ChartEntriesChangedAction = () => AvgTransactionValueChartDataChanged(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            InitViews(root);
            SetTerminalsPieChart();
            SetTotalSalesChart();
            SetAvgTransactionValueChart();

            return root;
        }

        private void SetTerminalsChartData()
        {
            var entries = new List<PieEntry>
            {
                new PieEntry(ViewModel.OnlineTerminals),
                new PieEntry(ViewModel.TotalTerminals - ViewModel.OnlineTerminals),
            };

            if (_terminalsPieChart!.Data is PieData pieData)
            {
                if (pieData.DataSets[0] is PieDataSet pieDataSet)
                {
                    pieDataSet.Clear();
                    pieDataSet.Values = entries;

                    pieData.NotifyDataChanged();
                    _terminalsPieChart.NotifyDataSetChanged();
                    _terminalsPieChart.Invalidate();
                }
            }
            else
            {
                var pieDataSet = new PieDataSet(entries, null);
                pieDataSet.Color = Color.Transparent;
                pieDataSet.SetColors(new[] { ContextCompat.GetColor(Context, Resource.Color.accent_color), ContextCompat.GetColor(Context, Resource.Color.negative_color) });
                pieDataSet.SliceSpace = 0f;
                pieDataSet.HighlightEnabled = false;
                pieDataSet.SetDrawValues(false);

                _terminalsPieChart.Data = new PieData(pieDataSet);
            }
        }

        private void TotalSalesChartDataChanged(bool animate = false)
        {
            var totalSales = LineChartUtils.GetLineDataSet(ViewModel.TotalSalesChartModel.Entries, Resource.Color.chart_primary_color);

            LineChartUtils.SetDataSets(_totalSalesChart, animate, totalSales);
        }

        private void AvgTransactionValueChartDataChanged(bool animate = false)
        {
            var avgTransactionValue = LineChartUtils.GetLineDataSet(ViewModel.AvgTransactionsValueChartModel.Entries, Resource.Color.chart_secondary_color);

            LineChartUtils.SetDataSets(_avgTransactionValueChart, animate, avgTransactionValue);
        }

        private void InitViews(View root)
        {
            _terminalsPieChart = root.FindViewById<PieChart>(Resource.Id.terminals_pie_chart)!;

            _totalSalesContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.total_sales_container)!;
            _totalSalesChart = root.FindViewById<LineChart>(Resource.Id.total_sales_line_chart)!;

            _avgTransactionValueContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.avg_transaction_value_container)!;
            _avgTransactionValueChart = root.FindViewById<LineChart>(Resource.Id.avg_transaction_value_line_chart)!;
        }

        private void SetTerminalsPieChart()
        {
            _terminalsPieChart!.SetPadding(0, 0, 0, 0);
            _terminalsPieChart.SetExtraOffsets(0, 0, 0, 0);
            _terminalsPieChart.Description.Enabled = false;
            _terminalsPieChart.SetDrawCenterText(false);
            _terminalsPieChart.Legend.Enabled = false;
            _terminalsPieChart.SetDrawMarkers(false);
            _terminalsPieChart.SetDrawEntryLabels(false);
            _terminalsPieChart.SetDrawSlicesUnderHole(false);
            _terminalsPieChart.SetTransparentCircleColor(Color.Transparent);

            _terminalsPieChart.HoleRadius = 90;
            _terminalsPieChart.SetHoleColor(Color.Transparent);

            SetTerminalsChartData();
        }

        private void SetTotalSalesChart()
        {
            _totalSalesContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var salesVsShrinkageContainerLp = _totalSalesContainer.LayoutParameters as MarginLayoutParams;
            salesVsShrinkageContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - salesVsShrinkageContainerLp.MarginStart - salesVsShrinkageContainerLp.MarginEnd) * 0.5f);
            _totalSalesContainer.LayoutParameters = salesVsShrinkageContainerLp;

            LineChartUtils.SetChartProperties(_totalSalesChart);
            _totalSalesChart.AxisLeft.GridLineWidth = 1;
            _totalSalesChart.AxisLeft.GridColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            _totalSalesChart.AxisLeft.SetDrawGridLines(true);
            _totalSalesChart.AxisLeft.SetDrawGridLinesBehindData(true);

            var xAxis = _totalSalesChart.XAxis;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_totalSalesChart!.Data is LineData lineData &&
                        lineData.DataSets[0] is LineDataSet curentLineDataSet && curentLineDataSet.Values is JavaList lineEntriesJavaList)
                    {
                        var lineEntries = lineEntriesJavaList.Cast<Entry>();
                        var entry = lineEntries.FirstOrDefault(t => t.GetX() == value);
                        if (entry != null && entry.Data is Java.Lang.String data)
                        {
                            return data.ToString();
                        }
                    }

                    return value.ToString();
                }
            };

            _totalSalesChart!.AxisLeft.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    return $"${(int)(value * 0.001)}K".ToString();
                }
            };

            TotalSalesChartDataChanged();
        }

        private void SetAvgTransactionValueChart()
        {
            _avgTransactionValueContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var salesVsShrinkageContainerLp = _avgTransactionValueContainer.LayoutParameters as MarginLayoutParams;
            salesVsShrinkageContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - salesVsShrinkageContainerLp.MarginStart - salesVsShrinkageContainerLp.MarginEnd) * 0.5f);
            _avgTransactionValueContainer.LayoutParameters = salesVsShrinkageContainerLp;

            LineChartUtils.SetChartProperties(_avgTransactionValueChart);
            _avgTransactionValueChart.AxisLeft.GridLineWidth = 1;
            _avgTransactionValueChart.AxisLeft.GridColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            _avgTransactionValueChart.AxisLeft.SetDrawGridLines(true);
            _avgTransactionValueChart.AxisLeft.SetDrawGridLinesBehindData(true);

            var xAxis = _avgTransactionValueChart.XAxis;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_avgTransactionValueChart!.Data is LineData lineData &&
                        lineData.DataSets[0] is LineDataSet curentLineDataSet && curentLineDataSet.Values is JavaList lineEntriesJavaList)
                    {
                        var lineEntries = lineEntriesJavaList.Cast<Entry>();
                        var entry = lineEntries.FirstOrDefault(t => t.GetX() == value);
                        if (entry != null && entry.Data is Java.Lang.String data)
                        {
                            return data.ToString();
                        }
                    }

                    return value.ToString();
                }
            };

            _avgTransactionValueChart!.AxisLeft.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    return $"${(int)value}".ToString();
                }
            };

            AvgTransactionValueChartDataChanged();
        }
    }
}
