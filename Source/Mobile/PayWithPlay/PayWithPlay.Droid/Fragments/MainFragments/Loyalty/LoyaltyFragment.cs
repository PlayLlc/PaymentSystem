using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Google.Android.Material.Button;
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
        private LinearLayoutCompat? _totalSalesContainer;
        private LineChart? _totalSalesLineChart;

        private LinearLayoutCompat? _newAccountsContainer;
        private BarChart? _newAccountsBarChart;

        private LinearLayoutCompat? _salesVsRedeemedContainer;
        private LineChart? _salesVsRedeemedLineChart;

        private LinearLayoutCompat? _topLoyaltyEnrollersContainer;

        private bool _resumedFirstTime = true;

        public override int LayoutId => Resource.Layout.fragment_loyalty;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.TotalSalesChartModel.ChartEntriesChangedAction = () => TotalSalesChartDataChanged(true);
            ViewModel.SalesVsReddeemedChartModel.ChartEntriesChangedAction = () => SalesVsRedeemedChartDataChanged();
            ViewModel.NewAccountsChartModel.ChartEntriesChangedAction = () => NewAccountChartDataChanged(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = base.OnCreateView(inflater, container, savedInstanceState);

            InitViews(rootView);
            SetTotalSalesChart();
            SetNewLoyaltyAccountsChart();
            SetSalesVsRedeemedChart();

            _resumedFirstTime = true;

            return rootView;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (_resumedFirstTime)
            {
                _resumedFirstTime = false;

                _totalSalesLineChart!.SetNoDataText(string.Empty);
                _newAccountsBarChart!.SetNoDataText(string.Empty);
                _salesVsRedeemedLineChart!.SetNoDataText(string.Empty);

                ViewModel.ReloadChartsData();
            }
        }

        private void TotalSalesChartDataChanged(bool animate = false)
        {
            var primaryTopColor = new Color(ContextCompat.GetColor(Context, Resource.Color.chart_gradient_primary_top_color));
            var primaryBottomColor = new Color(ContextCompat.GetColor(Context, Resource.Color.chart_gradient_primary_bottom_color));

            var nonLoyalty = LineChartUtils.GetLineDataSet(
                ViewModel.TotalSalesChartModel.NonLoyaltyEntries,
                Resource.Color.chart_primary_color,
                true,
                0.3f,
                new GradientDrawable(GradientDrawable.Orientation.TopBottom, new int[] { primaryTopColor, primaryBottomColor }));

            var secondaryTopColor = new Color(ContextCompat.GetColor(Context, Resource.Color.chart_gradient_secondary_top_color));
            var secondaryBottomColor = new Color(ContextCompat.GetColor(Context, Resource.Color.chart_gradient_secondary_bottom_color));

            var loyaltyCustomers = LineChartUtils.GetLineDataSet(
                ViewModel.TotalSalesChartModel.LoyaltyEntries,
                Resource.Color.chart_secondary_color,
                true,
                0.3f,
                new GradientDrawable(GradientDrawable.Orientation.TopBottom, new int[] { secondaryTopColor, secondaryBottomColor }));

            _totalSalesLineChart!.SetNoDataText("No chart data available");

            RequireActivity().RunOnUiThread(() =>
            {
                LineChartUtils.SetDataSets(_totalSalesLineChart, animate, nonLoyalty, loyaltyCustomers);

                if (animate)
                {
                    _totalSalesLineChart.AnimateX(800);
                }
            });
        }

        private void NewAccountChartDataChanged(bool animate = false)
        {
            _newAccountsBarChart!.SetNoDataText("No chart data available");

            RequireActivity().RunOnUiThread(() =>
            {
                BarChartUtils.SetBarEntries(ViewModel.NewAccountsChartModel.Entries, _newAccountsBarChart, false, animate);

                if (animate)
                {
                    _newAccountsBarChart.AnimateX(800);
                }
            });
        }

        private void SalesVsRedeemedChartDataChanged(bool animate = false)
        {
            var sales = LineChartUtils.GetLineDataSet(ViewModel.SalesVsReddeemedChartModel.SalesEntries, Resource.Color.chart_primary_color);
            var redeemed = LineChartUtils.GetLineDataSet(ViewModel.SalesVsReddeemedChartModel.RedeemedEntries, Resource.Color.chart_secondary_color);

            _salesVsRedeemedLineChart!.SetNoDataText("No chart data available");

            RequireActivity().RunOnUiThread(() =>
            {
                LineChartUtils.SetDataSets(_salesVsRedeemedLineChart, animate, sales, redeemed);

                if (animate)
                {
                    _salesVsRedeemedLineChart.AnimateX(800);
                }
            });
        }

        private void InitViews(View root)
        {
            var actionsView = root.FindViewById<LinearLayoutCompat>(Resource.Id.actions_container);
            actionsView.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());

            _totalSalesContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.total_sales_container)!;
            _totalSalesLineChart = root.FindViewById<LineChart>(Resource.Id.total_sales_line_chart)!;

            _newAccountsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.new_accounts_container)!;
            _newAccountsBarChart = root.FindViewById<BarChart>(Resource.Id.new_accounts_bar_chart)!;

            _salesVsRedeemedContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.sales_vs_redeemed_container)!;
            _salesVsRedeemedLineChart = root.FindViewById<LineChart>(Resource.Id.sales_vs_redeemed_line_chart)!;

            _topLoyaltyEnrollersContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.top_loyalty_enrollers_container)!;

            SetLayoutForNarrowSizes(root);
        }

        private void SetTotalSalesChart()
        {
            _totalSalesContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var totalSalesContainerLp = _totalSalesContainer.LayoutParameters as MarginLayoutParams;
            totalSalesContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - totalSalesContainerLp.MarginStart - totalSalesContainerLp.MarginEnd) * 0.5f);
            _totalSalesContainer.LayoutParameters = totalSalesContainerLp;

            LineChartUtils.SetChartProperties(_totalSalesLineChart);
            _totalSalesLineChart!.SetNoDataText(string.Empty);
            _totalSalesLineChart.AxisLeft!.GridLineWidth = 1;
            _totalSalesLineChart.AxisLeft.GridColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            _totalSalesLineChart.AxisLeft.SetDrawGridLines(true);
            _totalSalesLineChart.AxisLeft.SetDrawGridLinesBehindData(false);

            var xAxis = _totalSalesLineChart.XAxis!;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_totalSalesLineChart!.Data is LineData lineData &&
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

            _totalSalesLineChart!.AxisLeft.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    return $"${(int)(value * 0.001)}K".ToString();
                }
            };
        }

        private void SetNewLoyaltyAccountsChart()
        {
            _newAccountsContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var newAccountsContainerLp = _newAccountsContainer.LayoutParameters as MarginLayoutParams;
            newAccountsContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - newAccountsContainerLp.MarginStart - newAccountsContainerLp.MarginEnd) * 0.54f);
            _newAccountsContainer.LayoutParameters = newAccountsContainerLp;

            BarChartUtils.SetChartProperties(_newAccountsBarChart);
            _newAccountsBarChart!.SetNoDataText(string.Empty);

            var xAxis = _newAccountsBarChart.XAxis!;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_newAccountsBarChart!.Data is BarData barData &&
                        barData.DataSets![0] is BarDataSet curentBarDataSet && curentBarDataSet.Entries is JavaList barEntriesJavaList)
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
        }

        private void SetSalesVsRedeemedChart()
        {
            _salesVsRedeemedContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var salesVsShrinkageContainerLp = _salesVsRedeemedContainer.LayoutParameters as MarginLayoutParams;
            salesVsShrinkageContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - salesVsShrinkageContainerLp.MarginStart - salesVsShrinkageContainerLp.MarginEnd) * 0.5f);
            _salesVsRedeemedContainer.LayoutParameters = salesVsShrinkageContainerLp;

            LineChartUtils.SetChartProperties(_salesVsRedeemedLineChart);
            _salesVsRedeemedLineChart!.SetNoDataText(string.Empty);
            _salesVsRedeemedLineChart.AxisLeft!.GridLineWidth = 1;
            _salesVsRedeemedLineChart.AxisLeft.GridColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            _salesVsRedeemedLineChart.AxisLeft.SetDrawGridLines(true);
            _salesVsRedeemedLineChart.AxisLeft.SetDrawGridLinesBehindData(true);

            var xAxis = _salesVsRedeemedLineChart.XAxis!;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_salesVsRedeemedLineChart!.Data is LineData lineData &&
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

            _salesVsRedeemedLineChart!.AxisLeft.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    return $"${(int)(value * 0.001)}K".ToString();
                }
            };
        }

        private void SetLayoutForNarrowSizes(View root)
        {
            var width = Context!.Resources!.DisplayMetrics!.WidthPixels;
            var density = Context!.Resources!.DisplayMetrics!.Density;
            if (width / density < 340f)
            {
                var actionsView = root.FindViewById<LinearLayoutCompat>(Resource.Id.actions_container)!;
                actionsView.SetPadding(10f.ToPx(), actionsView.PaddingTop, 10f.ToPx(), actionsView.PaddingBottom);

                var searchBtn = root.FindViewById<MaterialButton>(Resource.Id.search_btn)!;
                var createBtn = root.FindViewById<MaterialButton>(Resource.Id.create_btn)!;
                var manageBtn = root.FindViewById<MaterialButton>(Resource.Id.manage_btn)!;
                searchBtn.SetMargins(end: 4f.ToPx());
                createBtn.SetMargins(start: 4f.ToPx(), end: 4f.ToPx());
                manageBtn.SetMargins(start: 4f.ToPx());

                var topCardsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.top_cards_container);
                topCardsContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());

                var card1 = root.FindViewById<View>(Resource.Id.view1);
                var card2 = root.FindViewById<View>(Resource.Id.view2);
                var card3 = root.FindViewById<View>(Resource.Id.view3);
                card1.SetMargins(end: 3f.ToPx());
                card2.SetMargins(start: 3f.ToPx(), end: 3f.ToPx());
                card3.SetMargins(start: 3f.ToPx());

                _totalSalesContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());
                _newAccountsContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());
                _salesVsRedeemedContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());
                _topLoyaltyEnrollersContainer.SetMargins(start: 10f.ToPx(), end: 10f.ToPx());
            }
        }
    }
}
