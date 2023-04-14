using PayWithPlay.Core.Resources;
using PayWithPlay.Core.ViewModels.Main.Inventory;

namespace PayWithPlay.Core.ViewModels.Main.Loyalty
{
    public class LoyaltyViewModel : BaseViewModel
    {
        public LoyaltyViewModel()
        {
        }

        public string Title => Resource.Loyalty;
        public string Subtitle => Resource.LoyaltySubtitle;
        public string SearchButtonText => Resource.Search;
        public string CreateButtonText => Resource.Create;
        public string ManageButtonText => Resource.Manage;

        public void OnSearch()
        {
            NavigationService.Navigate<SearchLoyaltyMemberViewModel>();
        }

        public void OnCreate()
        {
            NavigationService.Navigate<CreateLoyaltyMemberViewModel>();
        }

        public void OnManage()
        {
            NavigationService.Navigate<LoyaltyProgramsViewModel>();
        }
    }
}