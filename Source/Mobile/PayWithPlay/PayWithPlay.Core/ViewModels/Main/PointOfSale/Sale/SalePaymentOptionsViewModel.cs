using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.PointOfSale;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SalePaymentOptionsViewModel : BaseViewModel
    {
        public SalePaymentOptionsViewModel()
        {
            TotalAmount = 793.76m;

            Options.Add(new PaymentOptionItemModel { Type = Enums.PaymentOptionType.Card, Title = Resource.Card });
            Options.Add(new PaymentOptionItemModel { Type = Enums.PaymentOptionType.Cash, Title = Resource.Cash });

            // TODO:: uncomment when it need, this is hidden for now
            //Options.Add(new PaymentOptionItemModel { Type = Enums.PaymentOptionType.Link, Title = Resource.Link });
        }

        public string Title => Resource.Payment;
        public string Subtitle => Resource.PaymentOptionsSubtitle;

        public decimal TotalAmount { get; set; }

        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";

        public MvxObservableCollection<PaymentOptionItemModel> Options { get; set; } = new MvxObservableCollection<PaymentOptionItemModel>();

        public void OnOptionItem(PaymentOptionItemModel option)
        {
            if (option.Type == PaymentOptionType.Card)
            {
                NavigationService.Navigate<PaymentViewModel>();
            }
            else if (option.Type == PaymentOptionType.Cash)
            {
                NavigationService.Navigate<CashPaymentViewModel>();
            }
        }
    }
}
