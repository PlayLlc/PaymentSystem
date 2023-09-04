using MvvmCross.ViewModels;
using PayWithPlay.Core.Enums;
using PayWithPlay.Core.Models.PointOfSale;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale.Receipt
{
    public class ReceiptOptionsViewModel : BaseViewModel
    {
        public ReceiptOptionsViewModel()
        {
            TotalAmount = 793.76m;

            Options.Add(new ReceiptOptionItemModel { Type = ReceiptOptionType.Email, Title = Resource.Email });
            Options.Add(new ReceiptOptionItemModel { Type = ReceiptOptionType.TextMessage, Title = Resource.TextMessage });
            Options.Add(new ReceiptOptionItemModel { Type = ReceiptOptionType.NoReceipt, Title = Resource.NoReceipt });
        }

        public string Title => Resource.Receipt;
        public string Subtitle => Resource.ReceiptOptionsSubtitle;

        public decimal TotalAmount { get; set; }
        public string TotalDisplayed => $"{Resource.Total}\n${TotalAmount}";


        public MvxObservableCollection<ReceiptOptionItemModel> Options { get; set; } = new MvxObservableCollection<ReceiptOptionItemModel>();

        public void OnOptionItem(ReceiptOptionItemModel option)
        {
            if (option.Type == ReceiptOptionType.Email)
            {
                NavigationService.Navigate<ReceiptToEmailViewModel>();
            }
            else if (option.Type == ReceiptOptionType.TextMessage)
            {
                NavigationService.Navigate<ReceiptToPhoneNumberViewModel>();
            }
            else if (option.Type == ReceiptOptionType.NoReceipt)
            {
                NavigationService.Navigate<PurchaseSuccessfulViewModel>();
            }
        }
    }
}
