using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Inventory
{
    public class TopSellingProductsChartModel : MvxNotifyPropertyChanged
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

        public TopSellingProductsChartModel()
        {
            ReloadData();
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string TopSellingItemsText => Resource.TopSellingItems;

        public List<RadioButtonModel> ChartStepButtons => _chartStepButtons;

        public List<ChartEntry>? Entries { get; set; }

        public int SelectedChartStep
        {
            get => _selectedChartStep;
            set => SetProperty(ref _selectedChartStep, value, ReloadData);
        }

        public void ReloadData()
        {
            Entries = MockDataUtils.RandomTopSellingChartData((ChartStepType)SelectedChartStep);

            ChartEntriesChangedAction?.Invoke();
        }
    }
}
