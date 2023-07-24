using Android.Graphics;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Google.Android.Material.Button;
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

        private LinearLayoutCompat? _topSellersContainer;

        private LinearLayoutCompat? _avgTransactionValueContainer;
        private LineChart? _avgTransactionValueChart;

        private LinearLayoutCompat? _transactionsContainer;
        private BarChart? _transactionsChart;

        private bool _resumedFirstTime = true;

        public override int LayoutId => Resource.Layout.fragment_home;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.OnlineTerminalsAction = SetTerminalsChartData;
            ViewModel.TotalSalesChartModel.ChartEntriesChangedAction = () => TotalSalesChartDataChanged(true);
            ViewModel.AvgTransactionsValueChartModel.ChartEntriesChangedAction = () => AvgTransactionValueChartDataChanged(true);
            ViewModel.TransactionsChartModel.ChartEntriesChangedAction = TransactionsChartDataChanged;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = base.OnCreateView(inflater, container, savedInstanceState);

            InitViews(root);
            SetTerminalsPieChart();
            SetTotalSalesChart();
            SetAvgTransactionValueChart();
            SetTransactionsChart();

            _resumedFirstTime = true;

            return root;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (_resumedFirstTime)
            {
                _resumedFirstTime = false;

                _totalSalesChart!.SetNoDataText(string.Empty);
                _avgTransactionValueChart!.SetNoDataText(string.Empty);
                _transactionsChart!.SetNoDataText(string.Empty);

                ViewModel.ReloadChartsData();
            }
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
                if (pieData.DataSets![0] is PieDataSet pieDataSet)
                {
                    pieDataSet.Clear();
                    pieDataSet.Entries = entries;

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

            _totalSalesChart!.SetNoDataText("No chart data available");

            RequireActivity().RunOnUiThread(() =>
            {
                LineChartUtils.SetDataSets(_totalSalesChart, animate, totalSales);

                if (animate)
                {
                    _totalSalesChart.AnimateX(800);
                }
            });
        }

        private void AvgTransactionValueChartDataChanged(bool animate = false)
        {
            var avgTransactionValue = LineChartUtils.GetLineDataSet(ViewModel.AvgTransactionsValueChartModel.Entries, Resource.Color.chart_secondary_color);

            _avgTransactionValueChart!.SetNoDataText("No chart data available");

            RequireActivity().RunOnUiThread(() =>
            {
                LineChartUtils.SetDataSets(_avgTransactionValueChart, animate, avgTransactionValue);

                if (animate)
                {
                    _avgTransactionValueChart.AnimateX(800);
                }
            });
        }

        private void TransactionsChartDataChanged()
        {
            _transactionsChart!.SetNoDataText("No chart data available");
            BarChartUtils.SetBarEntries(ViewModel.TransactionsChartModel.Entries, _transactionsChart, false, false, 0.4f);
        }

        private void InitViews(View root)
        {
            _terminalsPieChart = root.FindViewById<PieChart>(Resource.Id.terminals_pie_chart)!;

            _totalSalesContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.total_sales_container)!;
            _totalSalesChart = root.FindViewById<LineChart>(Resource.Id.total_sales_line_chart)!;

            _topSellersContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.top_sellers_container)!;

            _avgTransactionValueContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.avg_transaction_value_container)!;
            _avgTransactionValueChart = root.FindViewById<LineChart>(Resource.Id.avg_transaction_value_line_chart)!;

            _transactionsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.transactions_container)!;
            _transactionsChart = root.FindViewById<BarChart>(Resource.Id.transactions_bar_chart)!;

            _totalSalesContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var totalSalesContainerLp = _totalSalesContainer.LayoutParameters as MarginLayoutParams;
            totalSalesContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - totalSalesContainerLp.MarginStart - totalSalesContainerLp.MarginEnd) * 0.5f);
            _totalSalesContainer.LayoutParameters = totalSalesContainerLp;

            _avgTransactionValueContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var avgTransactionContainerLp = _avgTransactionValueContainer.LayoutParameters as MarginLayoutParams;
            avgTransactionContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - avgTransactionContainerLp.MarginStart - avgTransactionContainerLp.MarginEnd) * 0.5f);
            _avgTransactionValueContainer.LayoutParameters = avgTransactionContainerLp;

            _transactionsContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var transactionsContainerLp = _transactionsContainer.LayoutParameters as MarginLayoutParams;
            transactionsContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - transactionsContainerLp.MarginStart - transactionsContainerLp.MarginEnd) * 0.54f);
            _transactionsContainer.LayoutParameters = transactionsContainerLp;

            SetLayoutForNarrowSizes(root);
        }

        private void SetTerminalsPieChart()
        {
            _terminalsPieChart!.SetPadding(0, 0, 0, 0);
            _terminalsPieChart.SetExtraOffsets(0, 0, 0, 0);
            _terminalsPieChart.Description!.Enabled = false;
            _terminalsPieChart.SetDrawCenterText(false);
            _terminalsPieChart.Legend!.Enabled = false;
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
            LineChartUtils.SetChartProperties(_totalSalesChart);
            _totalSalesChart.AxisLeft!.GridLineWidth = 1;
            _totalSalesChart.AxisLeft.GridColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            _totalSalesChart.AxisLeft.SetDrawGridLines(true);
            _totalSalesChart.AxisLeft.SetDrawGridLinesBehindData(true);
            _totalSalesChart!.SetNoDataText(string.Empty);

            var xAxis = _totalSalesChart.XAxis!;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_totalSalesChart!.Data is LineData lineData &&
                        lineData.DataSets![0] is LineDataSet curentLineDataSet && curentLineDataSet.Entries is JavaList lineEntriesJavaList)
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
        }

        private void SetAvgTransactionValueChart()
        {
            LineChartUtils.SetChartProperties(_avgTransactionValueChart);
            _avgTransactionValueChart.AxisLeft!.GridLineWidth = 1;
            _avgTransactionValueChart.AxisLeft.GridColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            _avgTransactionValueChart.AxisLeft.SetDrawGridLines(true);
            _avgTransactionValueChart.AxisLeft.SetDrawGridLinesBehindData(true);
            _avgTransactionValueChart!.SetNoDataText(string.Empty);

            var xAxis = _avgTransactionValueChart.XAxis!;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_avgTransactionValueChart!.Data is LineData lineData &&
                        lineData.DataSets![0] is LineDataSet curentLineDataSet && curentLineDataSet.Entries is JavaList lineEntriesJavaList)
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
        }

        private void SetTransactionsChart()
        {
            BarChartUtils.SetChartProperties(_transactionsChart);
            _transactionsChart!.SetNoDataText(string.Empty);
            _transactionsChart.AxisLeft!.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (value == 0)
                    {
                        return string.Empty;
                    }

                    return value.ToString();
                }
            };

            _transactionsChart.XAxis!.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_transactionsChart!.Data is BarData lineData &&
                     lineData.DataSets![0] is BarDataSet curentBarDataSet && curentBarDataSet.Entries is JavaList barEntriesJavaList)
                    {
                        var barEntries = barEntriesJavaList.Cast<BarEntry>();
                        var entry = barEntries.FirstOrDefault(t => t.GetX() == value);
                        if (entry != null && entry.Data is Java.Lang.String data)
                        {
                            return data.ToString();
                        }
                    }

                    return string.Empty;
                },
            };
        }

        private void SetLayoutForNarrowSizes(View root)
        {
            var width = Context!.Resources!.DisplayMetrics!.WidthPixels;
            var density = Context!.Resources!.DisplayMetrics!.Density;
            if (width / density < 340f)
            {
                var topCardsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.top_cards_container);
                topCardsContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());

                var card1 = root.FindViewById<View>(Resource.Id.view1);
                var card2 = root.FindViewById<View>(Resource.Id.view2);
                var card3 = root.FindViewById<View>(Resource.Id.view3);
                card1.SetMargins(end: 3f.ToPx());
                card2.SetMargins(start: 3f.ToPx(), end: 3f.ToPx());
                card3.SetMargins(start: 3f.ToPx());

                _totalSalesContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());
                _topSellersContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());
                _avgTransactionValueContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());
                _transactionsContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());
            }
        }
    }
}
