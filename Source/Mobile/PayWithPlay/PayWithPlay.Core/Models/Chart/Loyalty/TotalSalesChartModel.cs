﻿using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Resources;
using PayWithPlay.Core.Utils;

namespace PayWithPlay.Core.Models.Chart.Loyalty
{
    public class TotalSalesChartModel : MvxNotifyPropertyChanged
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

        public TotalSalesChartModel()
        {
            IsLoading = true;
        }

        public Action? ChartEntriesChangedAction { get; set; }

        public string TotalSalesText => Resource.TotalSales;
        public string NonLoyaltyText => Resource.NonLoyalty;
        public string LoyaltyCustomerText => Resource.LoyaltyCustomer;

        public List<RadioButtonModel> ChartStepButtons => _chartStepButtons;

        public List<ChartEntry>? NonLoyaltyEntries { get; set; }
        public List<ChartEntry>? LoyaltyEntries { get; set; }

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
            NonLoyaltyEntries = MockDataUtils.RandomLoyaltyTotalSalesChartData((ChartStepType)SelectedChartStep);
            LoyaltyEntries = MockDataUtils.RandomLoyaltyTotalSalesChartData((ChartStepType)SelectedChartStep);

            IsLoading = false;
            ChartEntriesChangedAction?.Invoke();
        }
    }
}
