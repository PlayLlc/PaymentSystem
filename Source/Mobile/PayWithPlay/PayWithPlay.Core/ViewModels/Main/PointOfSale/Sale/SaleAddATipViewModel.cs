using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.PointOfSale;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleAddATipViewModel : BaseViewModel
    {
        public SaleAddATipViewModel()
        {
            TotalAmount = 793.76m;

            TipItems.Add(new TipItemModel
            {
                PercentageValue = 10,
                TipValue = $"${TotalAmount / 10:0.00}",
                Type = TipType.Percentage
            });
            TipItems.Add(new TipItemModel
            {
                PercentageValue = 15,
                TipValue = $"${TotalAmount / 15:0.00}",
                Type = TipType.Percentage
            });
            TipItems.Add(new TipItemModel
            {
                PercentageValue = 20,
                TipValue = $"${TotalAmount / 20:0.00}",
                Type = TipType.Percentage
            });
            TipItems.Add(new TipItemModel
            {
                Type = TipType.Custom
            });
        }

        public string Title => Resource.AddATip;
        public string Subtitle => Resource.AddATipSubtitle;
        public string SkipButtonText => Resource.Skip;

        public decimal TotalAmount { get; set; }

        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public MvxObservableCollection<TipItemModel> TipItems { get; set; } = new MvxObservableCollection<TipItemModel>();

        public void OnTipItem(TipItemModel tipItemModel)
        {
            if (tipItemModel.Type == TipType.Custom)
            {
                NavigationService.Navigate<SaleCustomTipDialogViewModel, Tuple<string, Action<string>>>(Tuple.Create<string, Action<string>>(string.Empty,
                (value) =>
                {
                    NavigationService.Navigate<SalePaymentOptionsViewModel>();
                }));

                return;
            }

            NavigationService.Navigate<SalePaymentOptionsViewModel>();
        }

        public void OnSkip()
        {
            NavigationService.Navigate<SalePaymentOptionsViewModel>();
        }
    }
}
