using MvvmCross.Commands;
using PayWithPlay.Core.Models.Chart;
using PayWithPlay.Core.Models.Chart.Loyalty;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class LoyaltyViewModel : BaseViewModel
    {
        private IMvxAsyncCommand? _refreshCommand;
        private bool _isRefreshing;

        private int _totalRewardMembers;
        private decimal _loyaltySalesValue;
        private decimal _redeemedValue;

        public LoyaltyViewModel()
        {
            MockData();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            TotalSalesChartModel.ChartEntriesChangedAction = null;
            NewAccountsChartModel.ChartEntriesChangedAction = null;
            SalesVsReddeemedChartModel.ChartEntriesChangedAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public string Title => Resource.Loyalty;
        public string Subtitle => Resource.LoyaltySubtitle;
        public string SearchButtonText => Resource.Search;
        public string CreateButtonText => Resource.Create;
        public string ManageButtonText => Resource.Manage;
        public string TotalRewardMembersText => Resource.TotalRewardMembers;
        public string LoyaltySalesText => Resource.LoyaltySales;
        public string DailyText => Resource.Daily;
        public string RedeemedText => Resource.Redeemed;

        public TotalSalesChartModel TotalSalesChartModel { get; set; } = new TotalSalesChartModel();

        public SalesVsReddeemedChartModel SalesVsReddeemedChartModel { get; set; } = new SalesVsReddeemedChartModel();

        public NewAccountsChartModel NewAccountsChartModel { get; set; } = new NewAccountsChartModel();

        public IMvxAsyncCommand? RefreshCommand => _refreshCommand ??= new MvxAsyncCommand(OnRefresh);

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public int TotalRewardMembers
        {
            get => _totalRewardMembers;
            set => SetProperty(ref _totalRewardMembers, value);
        }

        public string LoyaltySalesValueDisplay => $"${LoyaltySalesValue:0.00}";
        public decimal LoyaltySalesValue
        {
            get => _loyaltySalesValue;
            set => SetProperty(ref _loyaltySalesValue, value, () => RaisePropertyChanged(() => LoyaltySalesValueDisplay));
        }

        public MiniChartModel? LoyaltySalesChartModel { get; set; }

        public string RedeemedValueDisplay => $"${RedeemedValue:0.00}";
        public decimal RedeemedValue
        {
            get => _redeemedValue;
            set => SetProperty(ref _redeemedValue, value, () => RaisePropertyChanged(() => RedeemedValueDisplay));
        }

        public MiniChartModel? RedeemedChartModel { get; set; }

        public void OnSearch()
        {
            NavigationService.Navigate<SearchLoyaltyMemberViewModel>();
        }

        public void OnCreate()
        {
            NavigationService.Navigate<CreateLoyaltyMemberViewModel>();
        }

        public void OnManage()
        {
            NavigationService.Navigate<LoyaltyProgramsViewModel>();
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
            TotalRewardMembers = random.Next(0, 1000); 
            LoyaltySalesValue = MockDataUtils.RandomDecimal();
            RedeemedValue = MockDataUtils.RandomDecimal();

            LoyaltySalesChartModel = MockDataUtils.RandomDataMiniChart(LoyaltySalesChartModel);
            RaisePropertyChanged(() => LoyaltySalesChartModel);

            RedeemedChartModel = MockDataUtils.RandomDataMiniChart(RedeemedChartModel);
            RaisePropertyChanged(() => RedeemedChartModel);

            SalesVsReddeemedChartModel.ReloadData();
            NewAccountsChartModel.ReloadData();
            TotalSalesChartModel.ReloadData();
        }
    }
}