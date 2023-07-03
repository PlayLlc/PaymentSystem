﻿using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Loyalty
{
    public class NewAccountsChartModel : MvxNotifyPropertyChanged
    {
        private readonly List<RadioButtonModel> _chartStepButtons = new()
        {
            new RadioButtonModel
            {
                Title = Resource.Daily,
                Type = (int)ChartStepType.Day,
                Color = RadioButtonModel.ColorType.Transparent
            },
            new RadioButtonModel
            {
                Title = Resource.Weekly,
                Type = (int)ChartStepType.Week,
                Color = RadioButtonModel.ColorType.Transparent
            },
            new RadioButtonModel
            {
                Title = Resource.Monthly,
                Type = (int)ChartStepType.Month,
                Color = RadioButtonModel.ColorType.Transparent
            },
        };

        private int _selectedChartStep = (int)ChartStepType.Day;

        public NewAccountsChartModel()
        {
            ReloadData();
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string NewLoyaltyAccountsText => Resource.NewLoyaltyAccounts;

        public List<RadioButtonModel> ChartStepButtons => _chartStepButtons;

        public List<ChartEntry>? Entries { get; set; }

        public int SelectedChartStep
        {
            get => _selectedChartStep;
            set => SetProperty(ref _selectedChartStep, value, ReloadData);
        }
        public void ReloadData()
        {
            Entries = MockDataUtils.RandomNewLoyaltyAccountsChartData((ChartStepType)SelectedChartStep);

            ChartEntriesChangedAction?.Invoke();
        }
    }
}
