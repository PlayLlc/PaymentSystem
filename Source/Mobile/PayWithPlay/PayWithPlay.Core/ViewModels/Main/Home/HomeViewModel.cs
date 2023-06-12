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
        private int _onlineTerminals;
        private int _totalTerminals;

        public HomeViewModel()
        {
            MockData();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            OnlineTerminalsAction = null;

            base.ViewDestroy(viewFinishing);
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

        public Action? OnlineTerminalsAction { get; set; }

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

        public int OnlineTerminals
        {
            get => _onlineTerminals;
            set => SetProperty(ref _onlineTerminals, value);
        }

        public string TotalTerminalsText => $"out of {TotalTerminals}";
        public int TotalTerminals
        {
            get => _totalTerminals;
            set => SetProperty(ref _totalTerminals, value, () => RaisePropertyChanged(() => TotalTerminalsText));
        }

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

            TotalTerminals = random.Next(10, 1000);
            OnlineTerminals = random.Next(1, TotalTerminals);

            OnlineTerminalsAction?.Invoke();
        }
    }
}