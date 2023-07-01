using MvvmCross.Commands;
using PayWithPlay.Core.Models.Chart;
using PayWithPlay.Core.Models.Chart.Inventory;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class InventoryViewModel : BaseViewModel
    {
        private IMvxAsyncCommand? _refreshCommand;
        private bool _isRefreshing;

        private float _strValue;
        private int _newInventoryAddedToday;
        private decimal _avgRevenuePerUnitValue;

        public InventoryViewModel()
        {
            MockData();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            TopSellingPorductsChartModel.ChartEntriesChangedAction = null;
            ShrinkageRateChartModel.ChartEntriesChangedAction = null;
            InventoryOnHandChartModel.ChartEntriesChangedAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public string Title => Resource.Inventory;
        public string Subtitle => Resource.InventorySubtitle;
        public string SearchButtonText => Resource.Search;
        public string AddButtonText => Resource.Add;
        public string ScanButtonText => Resource.Scan;

        public string STRText => Resource.STRFullText;
        public string NewInventoryTodayText => Resource.NewInventoryToday;
        public string AvgRevenuePerUnitText => Resource.AvgRevenuePerUnit;

        public IMvxAsyncCommand? RefreshCommand => _refreshCommand ??= new MvxAsyncCommand(OnRefresh);

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public string STRDisplayValue => $"{STRValue * 100:0.00}%";
        public float STRValue
        {
            get => _strValue;
            set => SetProperty(ref _strValue, value, () => RaisePropertyChanged(() => STRDisplayValue));
        }

        public int NewInventoryAddedToday
        {
            get => _newInventoryAddedToday;
            set => SetProperty(ref _newInventoryAddedToday, value);
        }

        public string AvgRevenuePerUnitDisplayValue => $"${AvgRevenuePerUnitValue:0.00}";
        public decimal AvgRevenuePerUnitValue
        {
            get => _avgRevenuePerUnitValue;
            set => SetProperty(ref _avgRevenuePerUnitValue, value, () => RaisePropertyChanged(() => AvgRevenuePerUnitDisplayValue));
        }

        public MiniChartModel? AvgRevenuePerUnitChartModel { get; set; }

        public TopSellingProductsChartModel TopSellingPorductsChartModel { get; set; } = new TopSellingProductsChartModel();
        public ShrinkageRateChartModel ShrinkageRateChartModel { get; set; } = new ShrinkageRateChartModel();
        public InventoryOnHandChartModel InventoryOnHandChartModel { get; set; } = new InventoryOnHandChartModel();

        public void OnSearch()
        {
            NavigationService.Navigate<SearchInventoryViewModel>();
        }

        public void OnAdd()
        {
            NavigationService.Navigate<CreateItemViewModel>();
        }

        public void OnScan()
        {
            NavigationService.Navigate<InventoryScanItemViewModel>();
        }

        private async Task OnRefresh()
        {
            IsRefreshing = true;

            await Task.Delay(1000);

            MockData();

            IsRefreshing = false;
        }

        private void MockData()
        {
            var random = new Random();
            STRValue = (float)random.NextDouble();
            NewInventoryAddedToday = random.Next(0, 1000);
            AvgRevenuePerUnitValue = MockDataUtils.RandomDecimal();

            AvgRevenuePerUnitChartModel = MockDataUtils.RandomDataMiniChart(AvgRevenuePerUnitChartModel);
            RaisePropertyChanged(() => AvgRevenuePerUnitChartModel);

            TopSellingPorductsChartModel.ReloadData();
            ShrinkageRateChartModel.ReloadData();
            InventoryOnHandChartModel.ReloadData();
        }
    }
}
