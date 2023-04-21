using MvvmCross.ViewModels;
using PayWithPlay.Core.Models.Loyalty;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class LoyaltyMemberViewModel : BaseViewModel
    {
        public LoyaltyMemberViewModel()
        {
            Points = 34572;

            Discounts.Add(new DiscountModel
            {
                Value = 10
            });
            Discounts.Add(new DiscountModel
            {
                Value = 15
            });

            Purchases.Add(new PurchaseModel
            {
                Amount = 23.97m,
                Date = DateTime.Now.AddYears(-1),
            });
            Purchases.Add(new PurchaseModel
            {
                Amount = 47.33m,
                Date = DateTime.Now.AddMonths(-9),
            });
            Purchases.Add(new PurchaseModel
            {
                Amount = 19.33m,
                Date = DateTime.Now.AddMonths(-3),
            });

            for (int i = 0; i < 100; i++)
            {
                Purchases.Add(new PurchaseModel
                {
                    Amount = 19.33m,
                    Date = DateTime.Now.AddMonths(-3),
                });
            }

        }

        public string Title => "Ralph Nader";
        public string PointsText => Resource.Points;
        public string PurchaseHistoryText => Resource.PurchaseHistory;
        public string NoPurchasesText => Resource.NoPurchases;

        public int Points { get; set; }

        public MvxObservableCollection<DiscountModel> Discounts { get; set; } = new MvxObservableCollection<DiscountModel>();
        public MvxObservableCollection<PurchaseModel> Purchases { get; set; } = new MvxObservableCollection<PurchaseModel>();

        public void OnBack()
        {
            NavigationService.Close(this);
        }
    }
}
