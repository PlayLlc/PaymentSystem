using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;
using PayWithPlay.Core.ViewModels.Main.Inventory;
using PayWithPlay.Droid.Extensions;
using PayWithPlay.Droid.Utils.Chart;
using static Android.Views.ViewGroup;
using static MikePhil.Charting.Components.YAxis;

namespace PayWithPlay.Droid.Fragments.MainFragments.Inventory
{
    //[MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.fragment_container, ViewModelType = typeof(InventoryViewModel))]
    public class InventoryFragment : BaseFragment<InventoryViewModel>
    {
        private LinearLayoutCompat? _topSellingItemsContainer;
        private BarChart? _topSellingBarChart;

        private LinearLayoutCompat? _shrinkageRateContainer;
        private BarChart? _shrinkageRateBarChart;

        private LinearLayoutCompat? _inventoryOnHandContainer;
        private BarChart? _inventoryOnHandBarChart;

        public override int LayoutId => Resource.Layout.fragment_inventory;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.TopSellingPorductsChartModel.ChartEntriesChangedAction = TopSellingItemsChanged;
            ViewModel.ShrinkageRateChartModel.ChartEntriesChangedAction = ShrinkageRateChartDataChanged;
            ViewModel.InventoryOnHandChartModel.ChartEntriesChangedAction = InventoryOnHandChartDataChanged;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = base.OnCreateView(inflater, container, savedInstanceState);

            InitViews(rootView);
            SetTopSellingItemsChart();
            SetShrinkageRateChart();
            SetDaysInventoryOnHandChart();

            return rootView;
        }

        private void TopSellingItemsChanged()
        {
            BarChartUtils.SetBarEntries(ViewModel.TopSellingPorductsChartModel.Entries, _topSellingBarChart, true);
        }

        private void ShrinkageRateChartDataChanged()
        {
            BarChartUtils.SetBarEntries(ViewModel.ShrinkageRateChartModel.Entries, _shrinkageRateBarChart, false);
        }

        private void InventoryOnHandChartDataChanged()
        {
            BarChartUtils.SetBarEntries(ViewModel.InventoryOnHandChartModel.Entries, _inventoryOnHandBarChart, false);
        }

        private void InitViews(View root)
        {
            var actionsView = root.FindViewById<LinearLayoutCompat>(Resource.Id.actions_container)!;
            actionsView.SetBackground(Resource.Color.secondary_color, bottomLeft: 5f.ToFloatPx(), bottomRight: 5f.ToFloatPx());

            _topSellingItemsContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.top_selling_items_container)!;
            _topSellingBarChart = root.FindViewById<BarChart>(Resource.Id.top_selling_items_bar_chart)!;

            _shrinkageRateContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.shrinkage_rate_container)!;
            _shrinkageRateBarChart = root.FindViewById<BarChart>(Resource.Id.shrinkage_rate_bar_chart)!;

            _inventoryOnHandContainer = root.FindViewById<LinearLayoutCompat>(Resource.Id.inventory_on_hand_container)!;
            _inventoryOnHandBarChart = root.FindViewById<BarChart>(Resource.Id.inventory_on_hand_bar_chart)!;
        }

        private void SetTopSellingItemsChart()
        {
            _topSellingItemsContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var topSellingContainerLp = _topSellingItemsContainer.LayoutParameters as MarginLayoutParams;
            topSellingContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - topSellingContainerLp.MarginStart - topSellingContainerLp.MarginEnd) * 0.54f);
            _topSellingItemsContainer.LayoutParameters = topSellingContainerLp;

            BarChartUtils.SetChartProperties(_topSellingBarChart);
            _topSellingBarChart.Renderer = new VerticalLabelBarChartRenderer(
                _topSellingBarChart,
                _topSellingBarChart.Animator,
                _topSellingBarChart.ViewPortHandler);

            _topSellingBarChart.XAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_topSellingBarChart!.Data is BarData lineData &&
                     lineData.DataSets[0] is BarDataSet curentBarDataSet && curentBarDataSet.Values is JavaList barEntriesJavaList)
                    {
                        var barEntries = barEntriesJavaList.Cast<BarEntry>();
                        var entry = barEntries.FirstOrDefault(t => t.GetX() == value);
                        if (entry != null)
                        {
                            return entry.GetY().ToString();
                        }
                    }

                    return value.ToString();
                }
            };

            TopSellingItemsChanged();
        }

        private void SetShrinkageRateChart()
        {
            _shrinkageRateContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var shrinkageRateContainerLp = _shrinkageRateContainer.LayoutParameters as MarginLayoutParams;
            shrinkageRateContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - shrinkageRateContainerLp.MarginStart - shrinkageRateContainerLp.MarginEnd) * 0.62f);
            _shrinkageRateContainer.LayoutParameters = shrinkageRateContainerLp;

            BarChartUtils.SetChartProperties(_shrinkageRateBarChart);

            var xAxis = _shrinkageRateBarChart.XAxis;
            xAxis.LabelRotationAngle = -33f;
            xAxis.YOffset = 4f.ToFloatPx();
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_shrinkageRateBarChart!.Data is BarData lineData &&
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

            _shrinkageRateBarChart.SetXAxisRenderer(new ObliqueLabelXAxisRenderer(_shrinkageRateBarChart.ViewPortHandler, _shrinkageRateBarChart.XAxis, _shrinkageRateBarChart.GetTransformer(AxisDependency.Left)));

            _shrinkageRateBarChart.AxisLeft.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    return $"{(int)((value * 100) + 0.5f)}%".ToString();
                }
            };

            ShrinkageRateChartDataChanged();
        }

        private void SetDaysInventoryOnHandChart() 
        {
            _inventoryOnHandContainer.SetBackground(Resource.Color.third_color, 2f.ToPx(), Resource.Color.hint_text_color, 5f.ToFloatPx());
            var inventoryOnHandContainerLp = _inventoryOnHandContainer.LayoutParameters as MarginLayoutParams;
            inventoryOnHandContainerLp!.Height = (int)((Context!.Resources!.DisplayMetrics!.WidthPixels - inventoryOnHandContainerLp.MarginStart - inventoryOnHandContainerLp.MarginEnd) * 0.5f);
            _inventoryOnHandContainer.LayoutParameters = inventoryOnHandContainerLp;

            BarChartUtils.SetChartProperties(_inventoryOnHandBarChart);

            var xAxis = _inventoryOnHandBarChart.XAxis;
            xAxis.ValueFormatter = new CustomValueFormatter()
            {
                OnAxisLabel = (value, axis) =>
                {
                    if (_inventoryOnHandBarChart!.Data is BarData lineData &&
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
            _inventoryOnHandBarChart.AxisLeft.GridLineWidth = 1;
            _inventoryOnHandBarChart.AxisLeft.GridColor = ContextCompat.GetColor(App.Context, Resource.Color.secondary_text_color);
            _inventoryOnHandBarChart.AxisLeft.SetDrawGridLines(true);
            _inventoryOnHandBarChart.AxisLeft.SetDrawGridLinesBehindData(true);

            InventoryOnHandChartDataChanged();
        }
    }
}