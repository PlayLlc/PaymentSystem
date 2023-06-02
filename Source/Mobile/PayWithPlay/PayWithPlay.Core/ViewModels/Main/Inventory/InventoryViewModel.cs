using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.ViewModels.Main.Inventory
{
    public class InventoryViewModel : BaseViewModel
    {
        public InventoryViewModel()
        {
        }

        public string Title => Resource.Inventory;
        public string Subtitle => Resource.InventorySubtitle;
        public string SearchButtonText => Resource.Search;
        public string AddButtonText => Resource.Add;
        public string ScanButtonText => Resource.Scan;

        public void OnSearch() 
        {
            NavigationService.Navigate<SearchInventoryViewModel>();
        }

        public void OnAdd()
        {
            NavigationService.Navigate<CreateItemViewModel>();
        }

        public void OnScan() 
        {
            NavigationService.Navigate<InventoryScanItemViewModel>();
        }
    }
}
