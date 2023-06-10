using MvvmCross.Commands;
using PayWithPlay.Core.Models.Chart;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.ViewModels.Main.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private IMvxAsyncCommand? _refreshCommand;
        private bool _isRefreshing;

        private int _dailySalesTargetValue;
        private decimal _dailySalesValue;
        private decimal _avgSPHValue;


        public HomeViewModel()
        {
            MockData();
        }

        public string DailySalesText => Resource.DailySales;
        public string SPHText => Resource.SPH;
        public string AvgSPHText => Resource.AvgSPH;
        public string OnlineTerminalsText => Resource.OnlineTerminals;
        public string OnlineText => Resource.Online;
        public string OfflineText => Resource.Offline;

        public string? UserPictureUrl { get; set; }
        public string? UserFullName { get; set; } = "Matt Jerred";
        public string? UserLocation { get; set; } = "Dublin, Ireland";

        public IMvxAsyncCommand? RefreshCommand => _refreshCommand ??= new MvxAsyncCommand(OnRefresh);

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public string DailySalesTargetValueDisplayed => $"{Resource.Target}: ${DailySalesTargetValue}";
        public int DailySalesTargetValue
        {
            get => _dailySalesTargetValue;
            set => SetProperty(ref _dailySalesTargetValue, value, () => RaisePropertyChanged(() => DailySalesTargetValueDisplayed));
        }

        public string DailySalesValueDisplayed => $"${DailySalesValue:0.00}";
        public decimal DailySalesValue
        {
            get => _dailySalesValue;
            set => SetProperty(ref _dailySalesValue, value, () => RaisePropertyChanged(() => DailySalesValueDisplayed));
        }

        public MiniChartModel? DailySalesMiniChartModel { get; set; }

        public string AvgSPHValueDisplayed => $"${AvgSPHValue:0.00}";
        public decimal AvgSPHValue
        {
            get => _avgSPHValue;
            set => SetProperty(ref _avgSPHValue, value, () => RaisePropertyChanged(() => AvgSPHValueDisplayed));
        }

        public MiniChartModel? AvgSPHMiniChartModel { get; set; }

        public void OnNotifications()
        {
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

            DailySalesTargetValue = random.Next(500, 10000);
            DailySalesValue = MockDataUtils.RandomDecimal();
            DailySalesMiniChartModel = MockDataUtils.RandomDataMiniChart(DailySalesMiniChartModel);
            DailySalesMiniChartModel.IsPositive = DailySalesValue >= DailySalesTargetValue;
            DailySalesMiniChartModel.Value = DailySalesMiniChartModel.IsPositive ?
                (float)(DailySalesValue - DailySalesTargetValue) / DailySalesTargetValue :
                (float)Math.Abs((DailySalesValue - DailySalesTargetValue)) / DailySalesTargetValue;
            RaisePropertyChanged(() => DailySalesMiniChartModel);

            AvgSPHValue = MockDataUtils.RandomDecimal();
            AvgSPHMiniChartModel = MockDataUtils.RandomDataMiniChart(AvgSPHMiniChartModel);
            RaisePropertyChanged(() => AvgSPHMiniChartModel);
        }
    }
}