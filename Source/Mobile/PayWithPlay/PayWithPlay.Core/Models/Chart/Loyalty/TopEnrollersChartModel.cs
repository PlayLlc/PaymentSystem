using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.Loyalty;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Loyalty
{
    public class TopEnrollersChartModel : MvxNotifyPropertyChanged
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

        public TopEnrollersChartModel()
        {
            ReloadData();
        }

        public string TopLoyaltyEnrollersText => Resource.TopLoyaltyEnrollers;

        public List<RadioButtonModel> ChartStepButtons => _chartStepButtons;

        public int SelectedChartStep
        {
            get => _selectedChartStep;
            set => SetProperty(ref _selectedChartStep, value, ReloadData);
        }

        public MvxObservableCollection<EnrollerModel> Enrollers { get; private set; } = new MvxObservableCollection<EnrollerModel>();

        public void ReloadData()
        {
            Enrollers.Clear();
            Enrollers.AddRange(MockDataUtils.RandomEnrollersChartData((ChartStepType)SelectedChartStep));
        }
    }
}
