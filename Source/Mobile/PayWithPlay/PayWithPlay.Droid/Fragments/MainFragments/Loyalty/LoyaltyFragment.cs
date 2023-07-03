using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using PayWithPlay.Core.ViewModels.Main.Loyalty;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils.Chart;
using static Android.Views.ViewGroup;

namespace PayWithPlay.Droid.Fragments.MainFragments.Loyalty
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(LoyaltyViewModel))]
    public class LoyaltyFragment : BaseFragment<LoyaltyViewModel>
    {
        private LinearLayoutCompat? _newAccountsContainer;
        private BarChart? _newAccountsBarChart;

        private LinearLayoutCompat? _salesVsRedeemedContainer;
        private LineChart? _salesVsRedeemedLineChart;

        public override int LayoutId => Resource.Layout.fragment_loyalty;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.SalesVsReddeemedChartModel.ChartEntriesChangedAction = SalesVsRedeemedChartDataChanged;
            ViewModel.NewAccountsChartModel.ChartEntriesChangedAction = NewAccountChartDataChanged;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = base.OnCreateView(inflater, container, savedInstanceState);

            InitViews(rootView);
            SetNewLoyaltyAccountsChart();
            SetSalesVsShrinkageChart();

            return rootView;
        }

        private void NewAccountChartDataChanged() 
        {
            BarChartUtils.SetBarEntries(ViewModel.NewAccountsChartModel.Entries, _newAccountsBarChart, false);
        }

        private void SalesVsRedeemedChartDataChanged() 
        {
            var sales = LineChartUtils.GetLineDataSet(ViewModel.SalesVsReddeemedChartModel.SalesEntries, Resource.Color.chart_primary_color);
            var redeemed = LineChartUtils.GetLineDataSet(ViewModel.SalesVsReddeemedChartModel.RedeemedEntries, Resource.Color.chart_secondary_color);

            LineChartUtils.SetDataSets(_salesVsRedeemedLineChart, sales, redeemed);
        }

        private void InitViews(View root) 
        {
            var actionsView = root.FindViewById<LinearLayoutCompat>(Resource.Id.actions_container);
            actionsView.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());

            _newAccountsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.new_accounts_container)!;
            _newAccountsBarChart = root.FindViewById<BarChart>(Resource.Id.new_accounts_bar_chart)!;

            _salesVsRedeemedContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.sales_vs_redeemed_container)!;
            _salesVsRedeemedLineChart = root.FindViewById<LineChart>(Resource.Id.sales_vs_redeemed_line_chart)!;
        }

        private void SetNewLoyaltyAccountsChart()
        {
            _newAccountsContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var newAccountsContainerLp = _newAccountsContainer.LayoutParameters as MarginLayoutParams;
            newAccountsContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - newAccountsContainerLp.MarginStart - newAccountsContainerLp.MarginEnd) * 0.54f);
            _newAccountsContainer.LayoutParameters = newAccountsContainerLp;

            BarChartUtils.SetChartProperties(_newAccountsBarChart);

            var xAxis = _newAccountsBarChart.XAxis;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_newAccountsBarChart!.Data is BarData lineData &&
                        lineData.DataSets[0] is BarDataSet curentBarDataSet && curentBarDataSet.Values is JavaList barEntriesJavaList)
                    {
                        var barEntries = barEntriesJavaList.Cast<BarEntry>();
                        var entry = barEntries.FirstOrDefault(t => t.GetX() == value);
                        if (entry != null && entry.Data is Java.Lang.String data)
                        {
                            return data.ToString();
                        }
                    }

                    return value.ToString();
                }
            };

            NewAccountChartDataChanged();
        }

        private void SetSalesVsShrinkageChart()
        {
            _salesVsRedeemedContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var salesVsShrinkageContainerLp = _salesVsRedeemedContainer.LayoutParameters as MarginLayoutParams;
            salesVsShrinkageContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - salesVsShrinkageContainerLp.MarginStart - salesVsShrinkageContainerLp.MarginEnd) * 0.5f);
            _salesVsRedeemedContainer.LayoutParameters = salesVsShrinkageContainerLp;

            LineChartUtils.SetChartProperties(_salesVsRedeemedLineChart);
            _salesVsRedeemedLineChart.AxisLeft.GridLineWidth = 1;
            _salesVsRedeemedLineChart.AxisLeft.GridColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            _salesVsRedeemedLineChart.AxisLeft.SetDrawGridLines(true);
            _salesVsRedeemedLineChart.AxisLeft.SetDrawGridLinesBehindData(true);

            var xAxis = _salesVsRedeemedLineChart.XAxis;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_salesVsRedeemedLineChart!.Data is LineData lineData &&
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

            _salesVsRedeemedLineChart!.AxisLeft.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    return $"${(int)(value * 0.001)}K".ToString();
                }
            };

            SalesVsRedeemedChartDataChanged();
        }
    }
}
