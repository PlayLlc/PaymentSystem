using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.Loyalty;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.PointOfSale.Sale
{
    public class SaleSelectLoyaltyDiscountViewModel : BaseViewModel
    {
        public SaleSelectLoyaltyDiscountViewModel()
        {
            Points = 34572;

            Discounts.Add(new DiscountModel
            {
                Value = 15
            });
            Discounts.Add(new DiscountModel
            {
                Value = 20
            });
        }

        public string Title => "Ralph Nader";

        public string PointsText => Resource.Points;
        public string ContinueWithoutDiscountText => Resource.ContinueWithoutDiscount;
        public string ContinueButtonText => Resource.Continue;

        public int Points { get; set; }

        public MvxObservableCollection<DiscountModel> Discounts { get; set; } = new MvxObservableCollection<DiscountModel>();

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnDiscountItem(DiscountModel discountModel)
        {
            NavigationService.Navigate<SaleAddATipViewModel>();
        }

        public void OnContinue()
        {
            NavigationService.Navigate<SaleAddATipViewModel>();
        }
    }
}
