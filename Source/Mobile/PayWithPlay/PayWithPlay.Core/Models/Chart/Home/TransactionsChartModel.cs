using MvvmCross.ViewModels;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Home
{
    public class TransactionsChartModel : MvxNotifyPropertyChanged
    {
        private int _transactionsCount;
        private bool _isLoading;

        public TransactionsChartModel()
        {
            IsLoading = true;
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string TransactionsDisplayed => $"{TransactionsCount} {Resource.Transactions}";

        public int TransactionsCount
        {
            get => _transactionsCount;
            set => SetProperty(ref _transactionsCount, value, () => RaisePropertyChanged(() => TransactionsDisplayed));
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<ChartEntry>? Entries { get; set; }

        public void ReloadData()
        {
            Entries = MockDataUtils.RandomTransactionsChartData();
            TransactionsCount = (int)Entries.Sum(t => t.Y);

            IsLoading = false;
            ChartEntriesChangedAction?.Invoke();
        }
    }
}
