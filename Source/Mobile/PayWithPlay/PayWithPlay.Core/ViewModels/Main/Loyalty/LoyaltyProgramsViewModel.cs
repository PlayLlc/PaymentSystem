using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class LoyaltyProgramsViewModel : BaseViewModel
    {
        public string Title => Resource.LoyaltyPrograms;
        public string Subtitle => Resource.LoyaltyProgramsSubtitle;
        public string DiscountsButtonText => Resource.Discounts;
        public string RewardsButtonText => Resource.Rewards;

        public void OnBack()
        {
            NavigationService.Close(this);
        }

        public void OnDiscounts()
        {
            NavigationService.Navigate<LoyaltyDiscountsViewModel>();
        }

        public void OnRewards()
        {
            NavigationService.Navigate<LoyaltyRewardsViewModel>();
        }
    }
}
