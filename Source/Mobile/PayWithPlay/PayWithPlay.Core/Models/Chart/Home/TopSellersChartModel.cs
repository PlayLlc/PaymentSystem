using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.Home;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Home
{
    public class TopSellersChartModel : MvxNotifyPropertyChanged
    {
        private readonly List<RadioButtonModel> _chartStepButtons = new()
        {
            new RadioButtonModel
            {
                Title = Resource.Day,
                Type = (int)ChartStepType.Day,
                Color = RadioButtonModel.ColorType.Transparent
            },
            new RadioButtonModel
            {
                Title = Resource.Week,
                Type = (int)ChartStepType.Week,
                Color = RadioButtonModel.ColorType.Transparent
            },
            new RadioButtonModel
            {
                Title = Resource.Month,
                Type = (int)ChartStepType.Month,
                Color = RadioButtonModel.ColorType.Transparent
            },
        };

        private int _selectedChartStep = (int)ChartStepType.Day;
        private bool _isLoading;

        public TopSellersChartModel()
        {
            IsLoading = true;
        }

        public string TopSellersText => Resource.TopSellers;

        public List<RadioButtonModel> ChartStepButtons => _chartStepButtons;

        public MvxObservableCollection<SellerModel> Sellers { get; private set; } = new MvxObservableCollection<SellerModel>();

        public int SelectedChartStep
        {
            get => _selectedChartStep;
            set => SetProperty(ref _selectedChartStep, value, ReloadData);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public void ReloadData()
        {
            InvokeOnMainThread(() => 
            {
                Sellers.Clear();

                IsLoading = false;
                Sellers.AddRange(MockDataUtils.RandomTopSellersChartData((ChartStepType)SelectedChartStep));
            });
        }
    }
}
